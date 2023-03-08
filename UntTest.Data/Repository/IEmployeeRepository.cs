using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntTest.Data.Entities;

namespace UntTest.Data.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<InternalEmployee>> GetInternalEmployeesAsync();

        InternalEmployee? GetInternalEmployee(Guid employeeId);

        Task<InternalEmployee?> GetInternalEmployeeAsync(Guid employeeId);

        Task<Course?> GetCourseAsync(Guid courseId);

        Course? GetCourse(Guid courseId);

        List<Course> GetCourses(params Guid[] courseIds);

        Task<List<Course>> GetCoursesAsync(params Guid[] courseIds);

        void AddInternalEmployee(InternalEmployee internalEmployee);

        Task SaveChangesAsync();
    }
}
