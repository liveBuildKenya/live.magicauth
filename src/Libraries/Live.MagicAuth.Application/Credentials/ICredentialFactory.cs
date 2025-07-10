using Fido2NetLib;
using Fido2NetLib.Development;
using Fido2NetLib.Objects;
using Live.MagicAuth.Domain.Credentials;

namespace Live.MagicAuth.Application.Credentials
{
    /// <summary>
    /// Represents the credentials factory interface.
    /// </summary>
    public interface ICredentialFactory
    {
        /// <summary>
        /// Builds a list of PublicKeyCredentialDescriptor from a list of Credentials.
        /// </summary>
        /// <param name="credentials">Credentials</param>
        /// <returns>A list of public key credential descriptors</returns>
        List<PublicKeyCredentialDescriptor> BuildPublicKeyCredentialDescriptors(ICollection<Credential> credentials);

        /// <summary>
        /// Checks if a credential is unique to a customer.
        /// </summary>
        /// <param name="credentialId">Credential identifier</param>
        /// <returns>True, if unique false otherwise</returns>
        Task<bool> IsCredentialUniqueToCustomer(byte[] credentialId);

        /// <summary>
        /// Inserts a credential to storage
        /// </summary>
        /// <param name="user">Fido User</param>
        /// <param name="credential">Credential</param>
        /// <returns>Stored Credential</returns>
        StoredCredential InsertCredential(Fido2User user, StoredCredential credential);
    }
}
