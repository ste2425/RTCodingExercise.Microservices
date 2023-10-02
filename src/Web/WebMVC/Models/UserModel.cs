using System.ComponentModel.DataAnnotations;

namespace RTCodingExercise.Microservices.Models
{
    public class UserModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}