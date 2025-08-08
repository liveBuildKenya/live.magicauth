using Fido2NetLib;
using Fido2NetLib.Objects;
using Live.MagicAuth.Application.Assertion.Models;
using Live.MagicAuth.Application.Credentials;
using Live.MagicAuth.Application.Customers;
using Live.MagicAuth.Application.Shared;
using Microsoft.AspNetCore.Http;

namespace Live.MagicAuth.Application.Assertion.Services
{
    /// <summary>
    /// Represents an assertion use case implementation.
    /// </summary>
    public class AssertionUseCase : IAssertionUseCase
    {
        #region Fields

        private readonly ICustomerFactory customerFactory;
        private readonly ICredentialFactory credentialFactory;
        private readonly IFido2 fido2;
        private readonly IHttpContextAccessor httpContextAccessor;

        #endregion

        #region Constructor

        public AssertionUseCase(ICustomerFactory customerFactory,
            ICredentialFactory credentialFactory,
            IFido2 fido2,
            IHttpContextAccessor httpContextAccessor)
        {
            this.customerFactory = customerFactory;
            this.credentialFactory = credentialFactory;
            this.fido2 = fido2;
            this.httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Makes assertion options based on the provided request model.
        /// </summary>
        /// <param name="assertionOptionsRequestModel">Assertion options request modlel</param>
        /// <returns>Assertion options</returns>
        /// <exception cref="ArgumentException"></exception>
        public IResult MakeAssertionOptions(AssertionOptionsRequestModel assertionOptionsRequestModel)
        {
            var existingCredentials = new List<PublicKeyCredentialDescriptor>();

            if (!string.IsNullOrEmpty(assertionOptionsRequestModel.Username))
            {
                // 1. Get user from DB
                CustomerModel user = customerFactory.GetCustomerWithCredentials(assertionOptionsRequestModel.Username) ?? throw new ArgumentException("Username was not registered");

                // 2. Get registered credentials from database
                existingCredentials = credentialFactory.BuildPublicKeyCredentialDescriptors(user.Credentials);
            }

            var exts = new AuthenticationExtensionsClientInputs()
            {
                UserVerificationMethod = true
            };

            // 3. Create options
            var uv = string.IsNullOrEmpty(assertionOptionsRequestModel.UserVerification) ? UserVerificationRequirement.Discouraged : assertionOptionsRequestModel.UserVerification.ToEnum<UserVerificationRequirement>();
            var options = fido2.GetAssertionOptions(
                existingCredentials,
                uv,
                exts
            );

            // 4. Temporarily store options, session/in-memory cache/redis/db
            httpContextAccessor.HttpContext.Session.SetString("fido2.assertionOptions", options.ToJson());

            // 5. Return options to client
            return Results.Ok(options);
        }

        /// <summary>
        /// Makes assertion based on the provided authenticator assertion raw response.
        /// </summary>
        /// <param name="authenticatorAssertionRawResponse">Authenticator assertion raw response</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Assertion result</returns>
        public async Task<IResult> MakeAssertion(AuthenticatorAssertionRawResponse authenticatorAssertionRawResponse, CancellationToken cancellationToken)
        {
            // 1. Get the assertion options we sent the client
            var jsonOptions = httpContextAccessor.HttpContext.Session.GetString("fido2.assertionOptions");
            var options = AssertionOptions.FromJson(jsonOptions);

            // 2. Get Credential by credentialId and Get credential counter from database
            (var creds, uint storedCounter, byte[] pubKey) = credentialFactory.GetCredentialByCredentialIdAndSignatureCount(authenticatorAssertionRawResponse.Id);

            // 4. Create callback to check if userhandle owns the credentialId
            IsUserHandleOwnerOfCredentialIdAsync callback = async (args, cancellationToken) =>
            {
                return await credentialFactory.IsUserHandleOwnerOfCredentialId(args.CredentialId, args.UserHandle);
            };

            // 5. Make the assertion
            var res = await fido2.MakeAssertionAsync(authenticatorAssertionRawResponse, options, pubKey, storedCounter, callback, cancellationToken: cancellationToken);

            // 6. Store the updated counter
            credentialFactory.UpdateCounter(res.CredentialId, res.Counter);

            // 7. return OK to client
            return Results.Ok(res);
        }

        #endregion
    }
}
