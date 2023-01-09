using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntTest.Data.Entities
{
    public class InternalEmployee: Employee
    {
        [Required]
        public int YearsInService { get; set; }

        [NotMapped]
        public Int32 SuggestedBonus { get; set; }

        [Required]
        public Int32 Salary { get; set; }

        [Required]
        public bool MinimumRaiseGiven { get; set; }

        public List<Course> AttendedCourses { get; set; } = new List<Course>();

        [Required]
        public int JobLevel { get; set; }

        public InternalEmployee(
            string firstName,
            string lastName,
            int yearsInService,
            Int32 salary,
            bool minimumRaiseGiven,
            int jobLevel)
            : base(firstName, lastName)
        {
            YearsInService = yearsInService;
            Salary = salary;
            MinimumRaiseGiven = minimumRaiseGiven;
            JobLevel = jobLevel;
        }
    }
}
