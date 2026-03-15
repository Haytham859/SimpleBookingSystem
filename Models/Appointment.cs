namespace ProjectWithOutArck.Models
{
    public class Appointment
    {

        public int Id { get; set; }
        //----------------------------------
        public int UserId {  get; set; }
        public User User { get; set; }
        //---------------------------------


        //--------------------------------

        public int DoctorId {  get; set; }
        public Doctor Doctor { get; set; }

        //--------------------------------
    
    public DateTime AppointmentDate {  get; set; }
        public string Status {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    
    }
}
