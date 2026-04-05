using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Infrastructure.Mappings
{
    public class ClienteEnderecoMap : IEntityTypeConfiguration<ClienteEndereco>
    {
        public void Configure(EntityTypeBuilder<ClienteEndereco> builder)
        {
            builder.ToTable("ClienteEnderecos");

            builder.HasKey(e => e.ClienteEnderecoId);

            builder.Property(e => e.Cep)
                .IsRequired()
                .HasMaxLength(9);

            builder.Property(e => e.Logradouro)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Bairro)
                .HasMaxLength(100);

            builder.Property(e => e.Cidade)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Estado)
                .IsRequired()
                .HasMaxLength(2);

            builder.Property(e => e.Numero)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Complemento)
                .HasMaxLength(200);

            builder.Property(e => e.CriadoEm)
                .IsRequired();
        }
    }
}
