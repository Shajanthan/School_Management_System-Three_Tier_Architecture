/// <summary>
///
/// </summary>
/// <author>Shajanthan</author>
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS_Models.Subject
{
    public class SubjectBO
    {

        public long SubjectID { get; set; }

        [DisplayName("Subject Code"), Required]
        public string SubjectCode { get; set; }

        [DisplayName("Subject Name"), Required]
        public string Name { get; set; }


        [DisplayName("Status")]
        public bool IsEnable { get; set; }


    }
}
