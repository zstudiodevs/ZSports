using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZSports.Domain.Constants;
using ZSports.Domain.Entities;

namespace ZSports.Persistence.Configurations;

public class DeporteEntityConfiguration : IEntityTypeConfiguration<Deporte>
{
    public void Configure(EntityTypeBuilder<Deporte> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Nombre)
            .IsRequired()
            .HasMaxLength(DeportesConstants.MaxNombreLength);

        builder.Property(d => d.Codigo)
            .IsRequired()
            .HasMaxLength(DeportesConstants.MaxCodigoLength);

        builder.Property(d => d.Activo)
            .IsRequired()
            .HasDefaultValue(true);
    }
}
