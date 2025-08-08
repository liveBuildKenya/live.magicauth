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
        /// <summary>
        /// Makes assertion options based on the provided request model.
        /// </summary>
        /// <param name="assertionOptionsRequestModel">Assertion options request model</param>
        /// <returns>Assertion options</returns>
        IResult MakeAssertionOptions(AssertionOptionsRequestModel assertionOptionsRequestModel);

        /// <summary>
        /// Makes an assertion based on the provided authenticator assertion raw response.
        /// </summary>
        /// <param name="authenticatorAssertionRawResponse">Authenticator assertion raw response</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IResult> MakeAssertion(AuthenticatorAssertionRawResponse authenticatorAssertionRawResponse, CancellationToken cancellationToken);
    }
}
