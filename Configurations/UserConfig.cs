using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectWithOutArck.Models;
namespace ProjectWithOutArck.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).HasMaxLength(50).IsRequired();
            builder.Property(a => a.Id).UseIdentityColumn(10, 10);
            


            //Relation Between User and Appointment
            builder.HasMany(a => a.Appointments).WithOne(a => a.User).HasForeignKey(a => a.UserId);


            


        }
    }
}
