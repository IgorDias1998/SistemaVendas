using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Infrastructure.Mappings
{
    public class VendaMap : IEntityTypeConfiguration<Venda>
    {
        public void Configure(EntityTypeBuilder<Venda> builder)
        {
            builder.ToTable("Vendas");

            builder.HasKey(v => v.VendaId);

            builder.Property(v => v.PessoaId)
                .IsRequired(false);

            builder.Property(v => v.DataVenda);

            builder.Property(v => v.ValorTotal)
                .HasColumnType("decimal(18,2)");

            builder.Property(v => v.Status);

            builder.HasOne(v => v.Pessoa)
                .WithMany()
                .HasForeignKey(v => v.PessoaId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
