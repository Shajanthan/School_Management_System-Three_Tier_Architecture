﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_ViewModels.Allocation
{
    public class StudentSubjectAllocationGroupByStudentViewModel
    {
        [DisplayName("Student Name")]
        public string StudentName{ get; set; }

        [DisplayName("Student Reg No")]
        public string StudentRegNo { get; set;}

        public IEnumerable<SubjectAllocationGroupByTeacherViewModel> TeacherAllocation { get; set; }
    }
}
