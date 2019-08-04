using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCDemoEntityFramework.Models
{
    [MetadataType(typeof(Employee))]
    public partial class EmployeeDetail
    {
    }

    public class Employee
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public Nullable<System.DateTime> DateOfBirth { get; set; }
      
    }
}