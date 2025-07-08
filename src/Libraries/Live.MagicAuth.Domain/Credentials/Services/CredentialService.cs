using Live.MagicAuth.Domain.Infrastructure.Data;

namespace Live.MagicAuth.Domain.Credentials.Services
{
    /// <summary>
    /// Represents a credential service implementation.
    /// </summary>
    public class CredentialService : ICredentialService
    {
        private readonly IRepository<Credential> credentialRepository;

        public CredentialService(IRepository<Credential> credentialRepository)
        {
            this.credentialRepository = credentialRepository;
        }
    }
}
