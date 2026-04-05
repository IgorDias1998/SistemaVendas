using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Infrastructure.Mappings
{
    public class PedidoMap : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");

            builder.HasKey(p => p.PedidoId);

            builder.Property(p => p.Tipo)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(p => p.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(p => p.Observacao)
                .HasMaxLength(500);

            builder.Property(p => p.CriadoEm)
                .IsRequired();

            builder.HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.CriadoPeloUsuario)
                .WithMany()
                .HasForeignKey(p => p.CriadoPeloUsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Itens)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Delivery)
                .WithOne(d => d.Pedido)
                .HasForeignKey<Delivery>(d => d.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
