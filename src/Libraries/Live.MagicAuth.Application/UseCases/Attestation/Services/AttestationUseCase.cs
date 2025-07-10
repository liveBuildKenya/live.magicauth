using Fido2NetLib;
using Fido2NetLib.Development;
using Fido2NetLib.Objects;
using Live.MagicAuth.Application.Credentials;
using Live.MagicAuth.Application.Customers;
using Live.MagicAuth.Application.UseCases.Attestation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Fido2NetLib.Fido2;

namespace Live.MagicAuth.Application.UseCases.Attestation.Services
{
    /// <summary>
    /// Represents the attestation service implementation
    /// </summary>
    public class AttestationUseCase : IAttestationUseCase
    {
        #region Fields

        private readonly IFido2 fido2;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICustomerFactory customerFactory;
        private readonly ICredentialFactory credentialFactory;

        #endregion

        #region Constructors

        public AttestationUseCase(IFido2 fido2,
            IHttpContextAccessor httpContextAccessor,
            ICustomerFactory customerFactory,
            ICredentialFactory credentialFactory)
        {
            this.fido2 = fido2;
            this.httpContextAccessor = httpContextAccessor;
            this.customerFactory = customerFactory;
            this.credentialFactory = credentialFactory;
        }

        #endregion

        public IResult MakeCredentialOptions(CredentialOptionsRequestModel credentialOptionsRequestModel)
        {
            //Get user from db by username.
            var customerModel = customerFactory.GetCustomerWithCredentials(credentialOptionsRequestModel.Email);
            //Create a new Fido2User object
            var user = new Fido2User
            {
                DisplayName = customerModel.Customer.DisplayName,
                Name = customerModel.Customer.Name,
                Id = customerModel.Customer.Id
            };
            //Get the users existing keys by username
            var existingKeys = credentialFactory.BuildPublicKeyCredentialDescriptors(customerModel.Credentials);
            //Create options
            var authenticatorSelection = new AuthenticatorSelection
            {
                RequireResidentKey = false,
                UserVerification = UserVerificationRequirement.Preferred
            };
            // Set up extensions if needed
            var exts = new AuthenticationExtensionsClientInputs()
            {
                Extensions = true,
                UserVerificationMethod = true
            };
            // Create the options for the new credential
            var options = fido2.RequestNewCredential(user, existingKeys, authenticatorSelection, AttestationConveyancePreference.Direct, exts);

            //Store the options temporarily
            httpContextAccessor.HttpContext.Session.SetString("fido2.attestationOptions", options.ToJson());

            return Results.Ok(options);
        }

        public async Task<IResult> RegisterCredentials(AuthenticatorAttestationRawResponse authenticatorAttestationRawResponse, CancellationToken cancellationToken)
        {
            var jsonOptions = httpContextAccessor.HttpContext.Session.GetString("fido2.attestationOptions");
            var options = CredentialCreateOptions.FromJson(jsonOptions);
            //Create callback to check if the credential is unique to the customer
            IsCredentialIdUniqueToUserAsyncDelegate callback = async (args, cancellationToken) =>
            {
                var response = await credentialFactory.IsCredentialUniqueToCustomer(args.CredentialId);
                Console.WriteLine($"Unique {response}");
                return response;
            };
            //Request for a new credential
            var success = await fido2.MakeNewCredentialAsync(authenticatorAttestationRawResponse, options, callback, cancellationToken: cancellationToken);

            var storedCredential = credentialFactory.InsertCredential(options.User, new StoredCredential
            {
                Descriptor = new PublicKeyCredentialDescriptor(success.Result.CredentialId),
                PublicKey = success.Result.PublicKey,
                UserHandle = success.Result.User.Id,
                SignatureCounter = success.Result.Counter,
                CredType = success.Result.CredType,
                RegDate = DateTime.UtcNow,
                AaGuid = success.Result.Aaguid
            });

            return Results.Ok(storedCredential);
        }
    }
}
