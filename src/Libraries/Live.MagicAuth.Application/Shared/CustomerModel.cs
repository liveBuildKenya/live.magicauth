using Live.MagicAuth.Domain.Credentials;
using Live.MagicAuth.Domain.Customers;

namespace Live.MagicAuth.Application.Shared
{
    /// <summary>
    /// Represents a customer model
    /// </summary>
    public class CustomerModel : BaseModel
    {
        private ICollection<Credential> _credentials;

        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the credentials
        /// </summary>
        public ICollection<Credential> Credentials 
        { 
            get { return _credentials ??= new List<Credential>(); }
            set { _credentials = value; }
        }
    }
}
