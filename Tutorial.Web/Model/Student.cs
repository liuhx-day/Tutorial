using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial.Web.Model
{
    public class Student
    {
        public int Id { get; set; }
        [Display(Name = "名")]
        public string FirstName { get; set; }
        [Display(Name = "姓")]
        public string LastName { get; set; }
        [Display(Name = "出生日期")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "性别")]
        public Gender Gender { get; set; }
    }
}
