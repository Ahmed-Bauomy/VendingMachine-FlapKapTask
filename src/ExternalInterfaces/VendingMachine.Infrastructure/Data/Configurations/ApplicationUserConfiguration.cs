using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Infrastructure.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.Deposit)
                   .IsRequired()
                   .HasDefaultValue(0);

            // Optional: keep this only if ApplicationUser.Products is used
            builder.HasMany(u => u.Products)
                   .WithOne() // <-- No navigation property in Product
                   .HasForeignKey(p => p.SellerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
