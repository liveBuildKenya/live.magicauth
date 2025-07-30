using Live.MagicAuth.Domain.Shared.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Live.MagicAuth.Domain.Credentials
{
    /// <summary>
    /// Represents a credential mapping
    /// </summary>
    public class CredentialMap : IEntityTypeConfiguration<Credential>
    {
        public void Configure(EntityTypeBuilder<Credential> builder)
        {
            builder.ToTable(nameof(Credential));

            builder.HasKey(credential => credential.RegDate);

            builder.Property(credential => credential.Descriptor)
                .HasColumnType("jsonb");
        }
    }
}
