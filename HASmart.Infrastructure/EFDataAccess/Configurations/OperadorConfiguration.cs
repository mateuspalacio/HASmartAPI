using HASmart.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OperadorConfiguration : IEntityTypeConfiguration<Operador>
{
    public void Configure(EntityTypeBuilder<Operador> builder)
    {
        builder.ToTable("Operador");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nome).IsRequired();
        //builder.Property(c => c.Cpf).IsRequired();
    }
}