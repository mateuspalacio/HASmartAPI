using HASmart.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MedicaoConfiguration : IEntityTypeConfiguration<Medicao> {
    public void Configure(EntityTypeBuilder<Medicao> builder) {
        builder.ToTable("Medicao");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.DataHora).IsRequired();
        builder.Property(c => c.Peso).IsRequired();
        builder.HasOne<Cidadao>().WithMany(c => c.Medicoes);
        builder.OwnsMany(m => m.Afericoes,
            b => {
                b.ToTable("Afericao");
                b.HasKey(a => a.Id);
                b.Property(a => a.Sistolica).IsRequired();
                b.Property(a => a.Diastolica).IsRequired();
            });
        builder.OwnsMany(c => c.Medicamentos, b => {
            b.ToTable("Medicamento");
            b.HasKey(m => m.Id);
            b.Property(m => m.Nome).IsRequired();
        });
        //builder.Property(c => c.CodigoFarm).IsRequired();
    }
}