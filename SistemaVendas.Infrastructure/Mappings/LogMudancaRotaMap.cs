using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Infrastructure.Mappings
{
    public class LogMudancaRotaMap : IEntityTypeConfiguration<LogMudancaRota>
    {
        public void Configure(EntityTypeBuilder<LogMudancaRota> builder)
        {
            builder.ToTable("LogsMudancaRota");

            builder.HasKey(l => l.LogMudancaRotaId);

            builder.Property(l => l.TipoMudanca)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(l => l.OldValue)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(l => l.NewValue)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(l => l.MudouEm)
                .IsRequired();

            builder.HasOne(l => l.Rota)
                .WithMany()
                .HasForeignKey(l => l.RotaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.AlteradoPeloUsuario)
                .WithMany()
                .HasForeignKey(l => l.AlteradoPeloUsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
