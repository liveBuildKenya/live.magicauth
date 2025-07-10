namespace Live.MagicAuth.Domain.Credentials.Services
{
    /// <summary>
    /// Represents a credential service interface.
    /// </summary>
    public interface ICredentialService
    {
        /// <summary>
        /// Gets credentials by customer identifier
        /// </summary>
        /// <param name="customerId">Customer Identifier</param>
        /// <returns>Credentials</returns>
        List<Credential> GetCredentialsByCustomerId(byte[] customerId);

        /// <summary>
        /// Gets a credential by credential identifier
        /// </summary>
        /// <param name="credentialId">Credential Identifier</param>
        /// <returns>Credential</returns>
        Credential GetCredentialByCredentialId(byte[] credentialId);

        /// <summary>
        /// Gets credentials by credential identifier
        /// </summary>
        /// <param name="credentialId">Credential id</param>
        /// <returns>Credentials</returns>
        List<Credential> GetCredentialsByCredentialId(byte[] credentialId);

        /// <summary>
        /// Inserts a credential
        /// </summary>
        /// <param name="credential">Credential</param>
        void InsertCredential(Credential credential);
    }
}
