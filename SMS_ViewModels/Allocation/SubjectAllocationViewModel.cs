using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_ViewModels.Allocation
{
    public class SubjectAllocationViewModel
    {
        public long TeacherID { get; set; }

        [DisplayName("Teacher Name")]
        public string TeacherName { get; set; }

        [DisplayName("Teacher Reg No")]
        public string TeacherRegNo { get; set; }


        public long SubjectAllocationID { get; set; }

        public long StudentAllocationID { get; set; }

        public long SubjectID { get; set; }

        [DisplayName("Subject Code")]
        public string SubjectCode { get; set; }

        [DisplayName("Subject Name")]
        public string SubjectName { get; set; }
        
    }
}
