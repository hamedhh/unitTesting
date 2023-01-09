using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntTest.Data.Entities
{
    public class ExternalEmployee:Employee
    {
        public string Company { get; set; }

        public ExternalEmployee(
            string firstName,
            string lastName,
            string company)
            :base(firstName, lastName)
        {
            Company = company;
        }
    }
}
