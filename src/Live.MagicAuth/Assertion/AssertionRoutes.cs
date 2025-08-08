using Fido2NetLib;
using Live.MagicAuth.Application.Assertion.Models;
using Live.MagicAuth.Application.Assertion.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading;

namespace Live.MagicAuth.Assertion
{
    /// <summary>
    /// Represents the assertion routes.
    /// </summary>
    public static class AssertionRoutes
    {
        public static void MapAssertionRoutes(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var tag = "Assertion";
            endpointRouteBuilder.MapGet("/assertion/option", ([FromServices] IAssertionUseCase assertionUseCase,
                [FromQuery] string username,
                [FromQuery] string userVerification) => assertionUseCase.MakeAssertionOptions(new AssertionOptionsRequestModel { Username = username, UserVerification = userVerification}))
                .WithTags(tag);

            endpointRouteBuilder.MapPost("/assertion", async ([FromServices] IAssertionUseCase assertionUseCase,
                [FromBody] AuthenticatorAssertionRawResponse authenticatorAssertionRawResponse,
                CancellationToken cancellationToken) => await assertionUseCase.MakeAssertion(authenticatorAssertionRawResponse, cancellationToken))
                .WithTags(tag);
        }
    }
}
