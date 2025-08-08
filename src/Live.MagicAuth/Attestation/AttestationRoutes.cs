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

            endpointRouteBuilder.MapGet("/attestation/options",
                ([FromQuery] string username,
                [FromQuery] string displayName,
                [FromServices] IAttestationUseCase attestationUseCase) =>
                attestationUseCase.MakeAttestationOptions(new AttestationOptionsRequestModel { Username = username, DisplayName = displayName }))
                .WithTags(routeTag);

            endpointRouteBuilder.MapPost("/attestation",
                async ([FromBody]AuthenticatorAttestationRawResponse authenticatorAttestationRawResponse,
                [FromServices]IAttestationUseCase attestationService,
                CancellationToken cancellationToken) => await attestationService.MakeAttestation(authenticatorAttestationRawResponse, cancellationToken))
                .WithTags(routeTag);
        }
    }
}
