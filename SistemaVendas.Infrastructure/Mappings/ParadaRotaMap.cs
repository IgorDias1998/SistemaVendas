using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Infrastructure.Mappings
{
    public class ParadaRotaMap : IEntityTypeConfiguration<ParadaRota>
    {
        public void Configure(EntityTypeBuilder<ParadaRota> builder)
        {
            builder.ToTable("ParadasRota");

            builder.HasKey(p => p.ParadaRotaId);

            builder.Property(p => p.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(p => p.StopOrder)
                .IsRequired();

            builder.HasOne(p => p.Delivery)
                .WithMany(d => d.ParadasRota)
                .HasForeignKey(p => p.DeliveryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
