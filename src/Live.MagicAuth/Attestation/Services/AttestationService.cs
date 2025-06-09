using Fido2NetLib;
using Fido2NetLib.Development;
using Fido2NetLib.Objects;
using Live.MagicAuth.Attestation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Fido2NetLib.Fido2;

namespace Live.MagicAuth.Attestation.Services
{
    /// <summary>
    /// Represents the attestation service implementation
    /// </summary>
    public class AttestationService : IAttestationService
    {
        private readonly IFido2 fido2;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AttestationService(IFido2 fido2,
            IHttpContextAccessor httpContextAccessor)
        {
            this.fido2 = fido2;
            this.httpContextAccessor = httpContextAccessor;
        }
        public IResult MakeCredentialOptions(CredentialOptionsRequestModel credentialOptionsRequestModel)
        {
            // 1. Get user from db by username.

            var user = new Fido2User
            {
                DisplayName = credentialOptionsRequestModel.DisplayName,
                Name = credentialOptionsRequestModel.Email,
                Id = Encoding.UTF8.GetBytes(credentialOptionsRequestModel.Email)
            };

            // 2. Get the users existing keys by username

            var existingKeys = new List<PublicKeyCredentialDescriptor>();

            // 3. Create options

            var authenticatorSelection = new AuthenticatorSelection
            {
                RequireResidentKey = false,
                UserVerification = UserVerificationRequirement.Preferred
            };

            var exts = new AuthenticationExtensionsClientInputs()
            {
                Extensions = true,
                UserVerificationMethod = true
            };

            var options = fido2.RequestNewCredential(user, existingKeys, authenticatorSelection, AttestationConveyancePreference.Direct, exts);

            // 4. Store the options temporarily
            httpContextAccessor.HttpContext.Session.SetString("fido2.attestationOptions", options.ToJson());

            return Results.Ok(options);
        }

        public async Task<IResult> RegisterCredentials(AuthenticatorAttestationRawResponse authenticatorAttestationRawResponse, CancellationToken cancellationToken)
        {
            var jsonOptions = httpContextAccessor.HttpContext.Session.GetString("fido2.attestationOptions");
            var options = CredentialCreateOptions.FromJson(jsonOptions);

            IsCredentialIdUniqueToUserAsyncDelegate callback = async static (args, cancellationToken) =>
            {
                return true;
            };

            var success = await fido2.MakeNewCredentialAsync(authenticatorAttestationRawResponse, options, callback, cancellationToken: cancellationToken);

            var x = new StoredCredential
            {
                Descriptor = new PublicKeyCredentialDescriptor(success.Result.CredentialId),
                PublicKey = success.Result.PublicKey,
                UserHandle = success.Result.User.Id,
                SignatureCounter = success.Result.Counter,
                CredType = success.Result.CredType,
                RegDate = DateTime.Now,
                AaGuid = success.Result.Aaguid
            };

            return Results.Ok(x);
        }
    }
}
