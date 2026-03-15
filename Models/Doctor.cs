namespace ProjectWithOutArck.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }= string.Empty;
        public string Specialty {  get; set; }=string.Empty;
        public TimeSpan WorkStart {  get; set; }
        public TimeSpan WorkEnd { get; set; }
        public int TimeSlot {  get; set; }
        //----------------------------------------
        public ICollection<Appointment> Appointments {  get; set; }

    }
}
