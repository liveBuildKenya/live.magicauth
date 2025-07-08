using Live.MagicAuth.Domain.Infrastructure.Data;

namespace Live.MagicAuth.Domain.Customers.Services
{
    /// <summary>
    /// Represents the customer service implementation.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> customerRepository;

        public CustomerService(IRepository<Customer> customerRepository)
        {
            this.customerRepository = customerRepository;
        }
    }
}
