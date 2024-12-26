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
        [Required(ErrorMessage = "Tên Admin là bắt buộc")]
        [MaxLength(128, ErrorMessage = "Tên không được quá 100 kí tự")]
        public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "Email Address là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email Address phải đúng tiêu chuẩn.")]
        public string EmailAddress { get; set; } = null!;
        [Required(ErrorMessage = "Password là trường bắt buộc.")]
        [MinLength(6, ErrorMessage = "Password ít nhất phải có 6 kí tự.")]
        [MaxLength(32, ErrorMessage = "Password không quá 32 kí tự.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
        ErrorMessage = "Password phải có số, kí tự viết hoa và kí tự đặc biệt")]
        public string Password { get; set; } = null!;
    }
}
