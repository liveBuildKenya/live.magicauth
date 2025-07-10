using Live.MagicAuth.Domain.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Live.MagicAuth.Domain.Credentials.Services
{
    /// <summary>
    /// Represents a credential service implementation.
    /// </summary>
    public class CredentialService : ICredentialService
    {
        #region Field

        private readonly IRepository<Credential> credentialRepository;

        #endregion

        #region Constructor

        public CredentialService(IRepository<Credential> credentialRepository)
        {
            this.credentialRepository = credentialRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a credential by the credential identifier in the descriptor
        /// </summary>
        /// <param name="credentialId">Credential identifier</param>
        /// <returns>Credential</returns>
        public Credential GetCredentialByCredentialId(byte[] credentialId)
        {
            var base64Id = Convert.ToBase64String(credentialId);
            var jsonFragment = $@"{{""Id"":""{base64Id}""}}";

            return (from credential in credentialRepository.Table
                    where EF.Functions.JsonContains(credential.Descriptor, jsonFragment)
                    select credential)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Gets credentials by customer identifier
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Credentials</returns>
        public List<Credential> GetCredentialsByCustomerId(byte[] customerId)
        {
            return (from credential in credentialRepository.Table
                    where credential.CustomerId == customerId
                    select credential).ToList();
        }

        /// <summary>
        /// Inserts a new credential
        /// </summary>
        /// <param name="credential">Credential</param>
        /// <exception cref="ArgumentNullException">Throw null exception if Credential is null</exception>
        public void InsertCredential(Credential credential)
        {
            if (credential == null) 
                throw new ArgumentNullException(nameof(Credential));

            credentialRepository.Insert(credential);
        }

        #endregion
    }
}
