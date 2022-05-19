using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YogaApp.Areas.Identity.Data;

namespace YogaApp.Data;

public class UserDbContext : IdentityDbContext<YogaAppUser>
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.ApplyConfiguration(new YogaAppUserEntityConfiguration());

    }

}


public class YogaAppUserEntityConfiguration : IEntityTypeConfiguration<YogaAppUser>
{
    public void Configure(EntityTypeBuilder<YogaAppUser> builder)
    {
                builder.Property(u => u.FirstName).HasMaxLength(255);
              builder.Property(u => u.LastName).HasMaxLength(255);
            builder.Property(u => u.UserType).IsRequired();
        builder.Property(u => u.YogaUserID).ValueGeneratedOnAdd();
        builder.Property(u => u.YogaUserID).Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);


    }



}
