using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Models.Subject
{
    public class SubjectBO
    {
        public long SubjectID { get; set; }
       
        [DisplayName("Subject Code"),Required]
        public string SubjectCode { get; set; }

        [DisplayName("Subject Name"),Required]
        public string Name { get; set; }


        [DisplayName("Status")]
        public bool IsEnable { get; set; }

        public string DisplayIsEnable
        {

            get
            {
                return (IsEnable) ? "Yes" : "No";
            }

        }

    }
}
