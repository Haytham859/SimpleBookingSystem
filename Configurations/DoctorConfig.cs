using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectWithOutArck.Models;

namespace ProjectWithOutArck.Configurations
{
    public class DoctorConfig : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(a => a.Id).UseIdentityColumn();
            builder.Property(a => a.Name).IsRequired().HasMaxLength(50);



            builder.HasMany(a => a.Appointments).WithOne(a => a.Doctor).HasForeignKey(a => a.DoctorId);

        }
    }
}
