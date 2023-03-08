using Microsoft.EntityFrameworkCore;
using UnitTest.Business;
using UntTest.Data.DbContexts;
using UntTest.Data.Repository;
using UntTest.Data.RepositoryService;
using UntTest.Domain.Services;

namespace MyUnitTestExperience.MiddelwareAndServiceRegister
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection RegisterBusinessService(this IServiceCollection service)
        {
            service.AddScoped<IEmployeeService, EmployeeService>();
            service.AddScoped<EmployeeFactory>();

            return service;
        }

        public static IServiceCollection RegisterDataServices(this IServiceCollection service, IConfiguration configuration)
        {
            // add the DbContext
            var ConnectionStr = configuration.GetConnectionString("DefaultConnection");
            service.AddDbContext<EmployeeDbContext>
                (options => options.UseSqlServer(ConnectionStr));

            // register the repository
            service.AddScoped<IEmployeeRepository, EmployeeRepository>();

            return service;
        }

    }
}
