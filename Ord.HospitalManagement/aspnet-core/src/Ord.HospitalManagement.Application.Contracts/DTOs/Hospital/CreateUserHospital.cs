using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ord.HospitalManagement.DTOs.Hospital
{
    public class CreateUserHospital
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [MaxLength(128, ErrorMessage = "Hospital Name cannot exceed 100 characters.")]
        public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Email Address must be in a valid format.")]
        public string EmailAddress { get; set; } = null!;
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [MaxLength(32, ErrorMessage = "Password cannot exceed 32 characters.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
        ErrorMessage = "Password must contain at least one letter, one number, and one special character.")]
        public string Password { get; set; } = null!;
    }
}
