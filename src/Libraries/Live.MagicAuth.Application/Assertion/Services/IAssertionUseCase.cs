using Fido2NetLib;
using Live.MagicAuth.Application.Assertion.Models;
using Microsoft.AspNetCore.Http;

namespace Live.MagicAuth.Application.Assertion.Services
{
    /// <summary>
    /// Represents an assertion use case interface.
    /// </summary>
    public interface IAssertionUseCase
    {
        IResult MakeAssertionOptions(AssertionOptionsRequestModel assertionOptionsRequestModel);
        Task<IResult> RegisterCredentials(AuthenticatorAssertionRawResponse authenticatorAssertionRawResponse, CancellationToken cancellationToken);
    }
}
