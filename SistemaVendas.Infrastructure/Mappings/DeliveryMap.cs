using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Infrastructure.Mappings
{
    public class DeliveryMap : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.ToTable("Deliveries");

            builder.HasKey(d => d.DeliveryId);

            builder.Property(d => d.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(d => d.CriadoEm)
                .IsRequired();

            builder.HasIndex(d => d.PedidoId)
                .IsUnique();

            builder.HasOne(d => d.ClienteEndereco)
                .WithMany()
                .HasForeignKey(d => d.ClienteEnderecoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
