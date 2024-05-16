/// <summary>
///
/// </summary>
/// <author>Shajanthan</author>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SMS_Models.Teacher
{
    public class TeacherBO
    {
        public long TeacherID { get; set; }
        [Required(ErrorMessage = "Registration Number is required")]
        [DisplayName("Registration Number")]
        public string TeacherRegNo { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Display Name is required")]
        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "E-mail is required")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address format")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [DisplayName("Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "DOB is required")]
        [DisplayName("DOB")]
        public System.DateTime DOB { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [DisplayName("Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        [DisplayName("Contact Number")]
        public string ContactNo { get; set; }


        [DisplayName("Status")]
        public bool IsEnable { get; set; }

    }
}
