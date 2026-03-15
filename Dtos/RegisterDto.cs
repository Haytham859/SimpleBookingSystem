using System.ComponentModel.DataAnnotations;

namespace ProjectWithOutArck.Dtos
{
    public class RegisterDto
    {
        [Required,MaxLength(50)]
        public string Name { get; set; }
        [Required,EmailAddress]
        public string Email {  get; set; }
        [Required,MinLength(6)]
        public string Password { get; set; }    

                
    }
}
