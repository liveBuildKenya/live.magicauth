using Live.MagicAuth.Domain.Credentials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Live.MagicAuth.Domain.Customers
{
    /// <summary>
    /// Represents the mapping configuration for a customer.
    /// </summary>
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(nameof(Customer));

            builder.HasKey(customer => customer.Id);

            builder.HasIndex(customer => customer.Name)
                .IsUnique();

            builder.HasMany<Credential>()
                .WithOne()
                .HasForeignKey(credential => credential.CustomerId);
        }
    }
}
