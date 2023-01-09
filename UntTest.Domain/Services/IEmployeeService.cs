﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntTest.Data.Entities;
using UntTest.Domain.EventArgument;


namespace UntTest.Domain.Services
{
    public interface IEmployeeService
    {
        event EventHandler<EmployeeIsAbsentEventArgs>? EmployeeIsAbsent;

        Task AddInternalEmployeeAsync(InternalEmployee internalEmployee);
        Task AttendCourseAsync(InternalEmployee employee, Course attendedCourse);
        ExternalEmployee CreateExternalEmployee(string firstName,
            string lastName, string company);
        InternalEmployee CreateInternalEmployee(string firstName, string lastName);
        Task<InternalEmployee> CreateInternalEmployeeAsync(string firstName,
            string lastName);
        InternalEmployee? FetchInternalEmployee(Guid employeeId);
        Task<InternalEmployee?> FetchInternalEmployeeAsync(Guid employeeId);
        Task<IEnumerable<InternalEmployee>> FetchInternalEmployeesAsync();
        Task GiveMinimumRaiseAsync(InternalEmployee employee);
        Task GiveRaiseAsync(InternalEmployee employee, int raise);
        void NotifyOfAbsence(Employee employee);

    }
}
