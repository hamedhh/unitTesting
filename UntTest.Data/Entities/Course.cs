using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntTest.Data.Entities
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool IsNew { get; set; } = true;
        public string Title { get; set; }
        public List<InternalEmployee> EmployeesThatAttended { get; set; }
            = new List<InternalEmployee>();

        public Course(string title)
        {
            Title = title;
        }
    }
}
