using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Infrastructure.Mappings
{
    public class RotaMap : IEntityTypeConfiguration<Rota>
    {
        public void Configure(EntityTypeBuilder<Rota> builder)
        {
            builder.ToTable("Rotas");

            builder.HasKey(r => r.RotaId);

            builder.Property(r => r.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(r => r.CriadoEm)
                .IsRequired();

            builder.HasOne(r => r.CriadoPeloUsuario)
                .WithMany()
                .HasForeignKey(r => r.CriadoPeloUsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Entregador)
                .WithMany()
                .HasForeignKey(r => r.AssociadoAoEntregadorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.Paradas)
                .WithOne(p => p.Rota)
                .HasForeignKey(p => p.RotaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
