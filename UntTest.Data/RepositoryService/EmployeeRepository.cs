using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntTest.Data.DbContexts;
using UntTest.Data.Entities;
using UntTest.Data.Repository;

namespace UntTest.Data.RepositoryService
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _dbContext;
        public EmployeeRepository(EmployeeDbContext employeeDb)
        {
            _dbContext = employeeDb ?? throw new ArgumentException(nameof(employeeDb));
        }
        public void AddInternalEmployee(InternalEmployee internalEmployee)
        {
            _dbContext.internalEmployees.Add(internalEmployee);
        }

        public Course? GetCourse(Guid courseId)
        {
            return _dbContext.courses.FirstOrDefault(x => x.Id == courseId);
        }

        public async Task<Course?> GetCourseAsync(Guid courseId)
        {
            return await _dbContext.courses.FirstOrDefaultAsync<Course>(x => x.Id == courseId);
        }

        public List<Course> GetCourses(params Guid[] courseIds)
        {
            var courses = new List<Course>();
            foreach (var courseId in courseIds)
            {
                var course = GetCourse(courseId);
                if (course != null)
                    courses.Add(course);

            }
            return courses;
        }

        public async Task<List<Course>> GetCoursesAsync(params Guid[] courseIds)
        {
            var courses = new List<Course>();
            foreach (var courseId in courseIds)
            {
                var course = await GetCourseAsync(courseId);
                if (course != null)
                    courses.Add(course);

            }
            return courses;
        }

        public InternalEmployee? GetInternalEmployee(Guid employeeId)
        {
            return _dbContext.internalEmployees
                .Include(x=>x.AttendedCourses)
                .FirstOrDefault(x=>x.Id== employeeId);
        }

        public async Task<InternalEmployee?> GetInternalEmployeeAsync(Guid employeeId)
        {
            return await _dbContext.internalEmployees
                .Include(x => x.AttendedCourses)
                .FirstOrDefaultAsync(x => x.Id == employeeId);
        }

        public async Task<IEnumerable<InternalEmployee>> GetInternalEmployeesAsync()
        {
             return await _dbContext.internalEmployees
                .Include(x => x.AttendedCourses)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
           await _dbContext.SaveChangesAsync();
        }
    }
}
