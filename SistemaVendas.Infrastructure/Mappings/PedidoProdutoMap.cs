using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Infrastructure.Mappings
{
    public class PedidoProdutoMap : IEntityTypeConfiguration<PedidoProduto>
    {
        public void Configure(EntityTypeBuilder<PedidoProduto> builder)
        {
            builder.ToTable("PedidoProdutos");

            builder.HasKey(p => p.PedidoProdutoId);

            builder.Property(p => p.Quantidade)
                .IsRequired();

            builder.Property(p => p.PrecoUnitario)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PrecoTotal)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.CriadoEm)
                .IsRequired();

            builder.HasOne(p => p.Produto)
                .WithMany()
                .HasForeignKey(p => p.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
