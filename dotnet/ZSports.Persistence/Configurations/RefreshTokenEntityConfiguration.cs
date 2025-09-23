using Microsoft.EntityFrameworkCore;
using ZSports.Domain.Entities;

namespace ZSports.Persistence.Configurations;

public class RefreshTokenEntityConfiguration: IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Token).IsRequired();
        builder.Property(rt => rt.Expiration).IsRequired();
        builder.Property(rt => rt.Revoked);
        builder.HasOne(rt => rt.Usuario)
               .WithMany()
               .HasForeignKey(rt => rt.UsuarioId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
