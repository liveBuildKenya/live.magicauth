using Fido2NetLib;
using Live.MagicAuth.Attestation.Models;
using Live.MagicAuth.Attestation.Services;
using Microsoft.AspNetCore.Builder;
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
            //RouteGroupBuilder credentialsGroupBuilder = endpointRouteBuilder.MapGroup("/credentials");

            endpointRouteBuilder.MapGet("/credentials/options",
                ([FromQuery] string email,
                [FromQuery] string displayName,
                [FromServices] IAttestationService attestationService) =>
                attestationService.MakeCredentialOptions(new CredentialOptionsRequestModel { Email = email, DisplayName = displayName }));

            endpointRouteBuilder.MapPost("/credentials",
                async ([FromBody]AuthenticatorAttestationRawResponse authenticatorAttestationRawResponse,
                [FromServices]IAttestationService attestationService,
                CancellationToken cancellationToken) => await attestationService.RegisterCredentials(authenticatorAttestationRawResponse, cancellationToken));
        }
    }
}
