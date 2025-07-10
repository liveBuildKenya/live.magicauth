using Fido2NetLib;
using Live.MagicAuth.Application.UseCases.Attestation.Models;
using Microsoft.AspNetCore.Http;

namespace Live.MagicAuth.Application.UseCases.Attestation.Services
{
    /// <summary>
    /// Represents the attestation service interface
    /// </summary>
    public interface IAttestationUseCase
    {
        IResult MakeCredentialOptions(CredentialOptionsRequestModel credentialOptionsRequestModel);
        Task<IResult> RegisterCredentials(AuthenticatorAttestationRawResponse authenticatorAttestationRawResponse, CancellationToken cancellationToken);
    }
}
