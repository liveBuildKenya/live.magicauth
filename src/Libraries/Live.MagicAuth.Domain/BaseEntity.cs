namespace Live.MagicAuth.Domain
{
    /// <summary>
    /// Represents the base class
    /// </summary>
    public class BaseEntity
    {
        public BaseEntity() 
        {
            this.DateCreated = DateTime.UtcNow;
            this.DateUpdated = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets or sets the date created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the date updated
        /// </summary>
        public DateTime DateUpdated { get; set; }
    }
}
