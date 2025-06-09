using Fido2NetLib;
using Live.MagicAuth.Attestation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Live.MagicAuth.Attestation.Services
{
    /// <summary>
    /// Represents the attestation service interface
    /// </summary>
    public interface IAttestationService
    {
        IResult MakeCredentialOptions(CredentialOptionsRequestModel credentialOptionsRequestModel);
        Task<IResult> RegisterCredentials(AuthenticatorAttestationRawResponse authenticatorAttestationRawResponse, CancellationToken cancellationToken);
    }
}
