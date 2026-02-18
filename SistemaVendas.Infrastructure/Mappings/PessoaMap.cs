using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Infrastructure.Mappings
{
    public class PessoaMap : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("Pessoas");

            builder.HasKey(p => p.PessoaId);

            builder.Property(p => p.NomePessoa)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.EmailPessoa)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.TelefonePessoa)
                .HasMaxLength(20);

            builder.Property(p => p.DocumentoPessoa)
                .HasMaxLength(50);

            builder.Property(p => p.DataNascimento)
                .IsRequired();

            builder.HasIndex(p => p.DocumentoPessoa)
                .IsUnique();

            builder.HasIndex(p => p.EmailPessoa)
                .IsUnique();

            // 1:1 Pessoa -> Endereco, FK stored on Pessoa.EnderecoId
            builder.HasOne(p => p.EnderecoPessoa)
                .WithOne()
                .HasForeignKey<Pessoa>(p => p.EnderecoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
