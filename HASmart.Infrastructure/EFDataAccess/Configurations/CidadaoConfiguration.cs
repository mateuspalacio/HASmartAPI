using HASmart.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CidadaoConfiguration : IEntityTypeConfiguration<Cidadao>
{
    public void Configure(EntityTypeBuilder<Cidadao> builder)
    {
        builder.ToTable("Cidadao");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Cpf).IsRequired();
        builder.Property(c => c.Rg).IsRequired();
        builder.Property(c => c.DataNascimento).IsRequired();
        builder.Property(c => c.DataCadastro).IsRequired();
        builder.OwnsOne(c => c.DadosPessoais, b => b.OwnsOne(x => x.Endereco).WithOwner());
        builder.OwnsOne(c => c.IndicadorRiscoHAS).WithOwner();
        builder.HasMany(c => c.Medicoes).WithOne();
        builder.HasOne(c => c.medicoAtual).WithMany(m => m.cidadaosAtuais).IsRequired();
        //builder.HasMany(c => c.medicosAtendeu).WithOne();
        //builder.HasMany(c => c.Dispencacoes).WithOne();
    }
}