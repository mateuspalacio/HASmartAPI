using HASmart.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MedicoConfiguration : IEntityTypeConfiguration<Medico>
{
    public void Configure(EntityTypeBuilder<Medico> builder){

        builder.ToTable("Medico");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Nome).IsRequired();
        //builder.Property(m => m.Crm).IsRequired();
        builder.HasMany(m => m.cidadaosAtuais).WithOne();
        //builder.HasMany(m => m.cidadaosAtendidos).WithOne();
    }
}
