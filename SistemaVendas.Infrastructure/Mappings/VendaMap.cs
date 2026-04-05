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

            builder.Property(v => v.ClienteId)
                .IsRequired(false);

            builder.Property(v => v.DataVenda);

            builder.Property(v => v.ValorTotal)
                .HasColumnType("decimal(18,2)");

            builder.Property(v => v.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.HasOne(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
