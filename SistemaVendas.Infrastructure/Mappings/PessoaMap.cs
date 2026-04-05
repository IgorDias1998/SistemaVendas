using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Infrastructure.Mappings
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");

            builder.HasKey(c => c.ClienteId);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Telefone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(c => c.Documento)
                .HasMaxLength(20);

            builder.Property(c => c.EstaAtivo)
                .HasDefaultValue(true);

            builder.Property(c => c.CriadoEm)
                .IsRequired();

            builder.Property(c => c.AlteradoEm)
                .IsRequired();

            builder.HasIndex(c => c.Documento)
                .IsUnique()
                .HasFilter("[Documento] IS NOT NULL AND [Documento] <> ''");

            builder.HasMany(c => c.Enderecos)
                .WithOne(e => e.Cliente)
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
