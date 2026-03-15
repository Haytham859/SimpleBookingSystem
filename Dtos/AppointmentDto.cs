namespace ProjectWithOutArck.Dtos
{
    public class AppointmentDto
    {
        public int Id { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string Status { get; set; } = null!;

        public string DoctorName { get; set; } = null!;

        public string UserName { get; set; } = null!;
    }
}
