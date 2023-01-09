using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntTest.Domain.EventArgument
{
    public class EmployeeIsAbsentEventArgs:EventArgs
    {
        public Guid EmployeeId { get; private set; }

        public EmployeeIsAbsentEventArgs(Guid employyeId)
        {
            EmployeeId= employyeId;
        }
    }
}
