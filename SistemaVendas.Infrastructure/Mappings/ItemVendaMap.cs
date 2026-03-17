using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Infrastructure.Mappings
{
    public class ItemVendaMap : IEntityTypeConfiguration<ItemVenda>
    {
        public void Configure(EntityTypeBuilder<ItemVenda> builder)
        {
            builder.ToTable("ItensVenda");

            builder.HasKey(iv => iv.ItemVendaId);

            builder.Property(iv => iv.Quantidade)
                .IsRequired();

            builder.Property(iv => iv.ValorUnitario)
                .HasColumnType("decimal(18,2)");

            builder.Property(iv => iv.ValorTotal)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(iv => iv.Venda)
                .WithMany(v => v.ItensVenda)
                .HasForeignKey(iv => iv.VendaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(iv => iv.Produto);

            builder.HasOne(iv => iv.Produto)
                .WithMany(Produto => Produto.ItensVenda)
                .HasForeignKey(iv => iv.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
