using Live.MagicAuth.Application.Assertion.Services;
using Live.MagicAuth.Application.Attestation.Services;
using Live.MagicAuth.Application.Credentials;
using Live.MagicAuth.Application.Customers;
using Live.MagicAuth.Domain.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Live.MagicAuth.Application.Infrastructure
{
    /// <summary>
    /// Represents the dependency injection class
    /// </summary>
    public static class Dependencyinjection
    {
        public static void AddMagicAuthApplicationServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddMagicAuthDomainServices(configuration);

            serviceCollection.AddTransient<ICustomerFactory, CustomerFactory>();
            serviceCollection.AddTransient<ICredentialFactory, CredentialFactory>();

            serviceCollection.AddTransient<IAttestationUseCase, AttestationUseCase>();
            serviceCollection.AddTransient<IAssertionUseCase, AssertionUseCase>();
        }
    }
}
