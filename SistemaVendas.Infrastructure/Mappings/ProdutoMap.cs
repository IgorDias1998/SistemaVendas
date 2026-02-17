using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Infrastructure.Mappings
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            // map to table
            builder.ToTable("Produtos", t =>
            {
                t.HasCheckConstraint("CK_Produtos_Preco_NonNegative", "PrecoProduto >= 0");
                t.HasCheckConstraint("CK_Produtos_Estoque_NonNegative", "EstoqueProduto >= 0");
            });

            // primary key
            builder.HasKey(p => p.ProdutoId);

            builder.Property(p => p.TituloProduto)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(p => p.DescricaoProduto)
                .HasMaxLength(500);

            builder.Property(p => p.PrecoProduto)
                .IsRequired()
                .HasPrecision(18, 2);

            // Estoque is an int in the model — no conversion needed. Set a default and ensure non-negative
            builder.Property(p => p.EstoqueProduto)
                .HasDefaultValue(0);

            builder.Property(p => p.CodigoProduto)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(p => p.CodigoProduto)
                .IsUnique();
        }
    }
}
