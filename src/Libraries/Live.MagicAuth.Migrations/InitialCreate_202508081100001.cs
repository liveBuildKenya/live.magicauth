
using FluentMigrator;

namespace Live.MagicAuth.Migrations
{
    [Migration(202508081100001, "Initial migration moving from EF Core Migrations")]
    public class InitialCreate : Migration
    {
        public override void Up()
        {
            // Customer Table
            Create.Table("Customer")
                .WithColumn("Id").AsBinary().NotNullable().PrimaryKey()
                .WithColumn("Name").AsCustom("text").Nullable()
                .WithColumn("DisplayName").AsCustom("text").Nullable()
                .WithColumn("DateCreated").AsDateTimeOffset().NotNullable()
                .WithColumn("DateUpdated").AsDateTimeOffset().NotNullable();

            // Credential Table
            Create.Table("Credential")
                .WithColumn("RegDate").AsDateTimeOffset().NotNullable().PrimaryKey()
                .WithColumn("CustomerId").AsBinary().Nullable()
                .WithColumn("Descriptor").AsCustom("jsonb").Nullable()
                .WithColumn("PublicKey").AsBinary().Nullable()
                .WithColumn("UserHandle").AsBinary().Nullable()
                .WithColumn("SignatureCounter").AsInt64().NotNullable()
                .WithColumn("CredType").AsCustom("text").Nullable()
                .WithColumn("AaGuid").AsGuid().NotNullable()
                .WithColumn("DateCreated").AsDateTimeOffset().NotNullable()
                .WithColumn("DateUpdated").AsDateTimeOffset().NotNullable();

            // Foreign Key
            Create.ForeignKey("FK_Credential_Customer_CustomerId")
                .FromTable("Credential").ForeignColumn("CustomerId")
                .ToTable("Customer").PrimaryColumn("Id");

            // Indexes
            Create.Index("IX_Credential_CustomerId")
                .OnTable("Credential")
                .OnColumn("CustomerId").Ascending();

            Create.UniqueConstraint("IX_Customer_Name")
                .OnTable("Customer")
                .Column("Name");
        }

        public override void Down()
        {
            Delete.UniqueConstraint("IX_Customer_Name").FromTable("Customer");
            Delete.ForeignKey("FK_Credential_Customer_CustomerId").OnTable("Credential");
            Delete.Index("IX_Credential_CustomerId").OnTable("Credential");
            Delete.Table("Credential");
            Delete.Table("Customer");
        }
    }
}
