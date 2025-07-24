using Fido2NetLib;
using Live.MagicAuth.Application.Attestation.Models;
using Live.MagicAuth.Application.Attestation.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading;

namespace Live.MagicAuth.Attestation
{
    /// <summary>
    /// Represents the registration of new credentials
    /// </summary>
    public static class AttestationRoutes
    {
        public static void MapAttestationRoutes(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var routeTag = "Attestation";

            endpointRouteBuilder.MapGet("/credentials/options",
                ([FromQuery] string email,
                [FromQuery] string displayName,
                [FromServices] IAttestationUseCase attestationUseCase) =>
                attestationUseCase.MakeAttestationOptions(new AttestationOptionsRequestModel { Email = email, DisplayName = displayName }))
                .WithTags(routeTag);

            endpointRouteBuilder.MapPost("/credentials",
                async ([FromBody]AuthenticatorAttestationRawResponse authenticatorAttestationRawResponse,
                [FromServices]IAttestationUseCase attestationService,
                CancellationToken cancellationToken) => await attestationService.RegisterCredentials(authenticatorAttestationRawResponse, cancellationToken))
                .WithTags(routeTag);
        }
    }
}
