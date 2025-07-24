using Fido2NetLib;
using Live.MagicAuth.Application.Attestation.Models;
using Microsoft.AspNetCore.Http;

namespace Live.MagicAuth.Application.Attestation.Services
{
    /// <summary>
    /// Represents the attestation service interface
    /// </summary>
    public interface IAttestationUseCase
    {
        IResult MakeAttestationOptions(AttestationOptionsRequestModel credentialOptionsRequestModel);
        Task<IResult> RegisterCredentials(AuthenticatorAttestationRawResponse authenticatorAttestationRawResponse, CancellationToken cancellationToken);
    }
}
