using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MunicipalServicesMVC.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "اسم المستخدم")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [Display(Name = "الدور")]
        public int RoleId { get; set; }

        public Role? Role { get; set; }
    }
}