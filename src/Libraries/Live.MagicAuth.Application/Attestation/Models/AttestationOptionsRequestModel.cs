namespace Live.MagicAuth.Application.Attestation.Models
{
    /// <summary>
    /// Represents the credential options request model
    /// </summary>
    public class AttestationOptionsRequestModel
    {
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        public string DisplayName { get; set; }
    }
}
