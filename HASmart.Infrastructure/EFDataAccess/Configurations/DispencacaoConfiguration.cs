using HASmart.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DispencacaoConfiguration : IEntityTypeConfiguration<Dispencacao> {
    public void Configure(EntityTypeBuilder<Dispencacao> builder) {
        builder.ToTable("Dispencacao");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.DataHora).IsRequired();
        // builder.HasOne<Cidadao>().WithMany(c => c.Dispencacoes);
        builder.OwnsMany(c => c.Medicamentos, b => { 
            //b.HasKey(m => m.Id);
            b.Property(m => m.Nome).IsRequired();
            /*b.Property(m => m.Finalidade).IsRequired();
            b.Property(m => m.Dosagem).IsRequired();
            b.Property(m => m.Quantidade).IsRequired();*/
        });
    }
}