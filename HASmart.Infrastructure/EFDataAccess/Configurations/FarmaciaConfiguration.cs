using HASmart.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FarmaciaConfiguration : IEntityTypeConfiguration<Farmacia> {
    public void Configure(EntityTypeBuilder<Farmacia> builder) {
        builder.ToTable("Farmacia");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Cnpj).IsRequired();
        builder.Property(f => f.NomeFantasia).IsRequired();
        builder.Property(f => f.RazaoSocial).IsRequired();
        builder.OwnsOne(f => f.Endereco).WithOwner();
        builder.Property(f => f.Email).IsRequired();
        builder.Property(f => f.Telefone).IsRequired();
    }
}