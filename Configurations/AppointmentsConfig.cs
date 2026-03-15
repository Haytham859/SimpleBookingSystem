using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectWithOutArck.Models;

namespace ProjectWithOutArck.Configurations
{
    public class AppointmentsConfig : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.Property(a => a.CreatedAt).HasComputedColumnSql("GETDATE()");
        }
    }
}
