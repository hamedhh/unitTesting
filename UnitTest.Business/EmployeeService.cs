﻿using Microsoft.EntityFrameworkCore;
using MyUnitTestExperience.Business.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntTest.Data.Entities;
using UntTest.Data.Repository;
using UntTest.Domain.EventArgument;
using UntTest.Domain.Services;
using static System.Net.Mime.MediaTypeNames;

namespace UnitTest.Business
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly EmployeeFactory _employeeFactory;

        // Ids of obligatory courses: "Company Introduction" and "Respecting Your Colleagues" 
        private Guid[] _obligatoryCourseIds = {
            Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
            Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e") };


        public EmployeeService(IEmployeeRepository employeeRepository, EmployeeFactory employeeFactory)
        {
            _employeeRepository = employeeRepository;
            _employeeFactory = employeeFactory;
        }

        public event EventHandler<EmployeeIsAbsentEventArgs>? EmployeeIsAbsent;

        public async Task AddInternalEmployeeAsync(InternalEmployee internalEmployee)
        {
            _employeeRepository.AddInternalEmployee(internalEmployee);
            await _employeeRepository.SaveChangesAsync();
        }

        public async Task AttendCourseAsync(InternalEmployee employee, Course attendedCourse)
        {
            var alreadyAttendedCourse = employee.AttendedCourses
               .Any(c => c.Id == attendedCourse.Id);

            if (alreadyAttendedCourse)
            {
                return;
            }

            // add course 
            employee.AttendedCourses.Add(attendedCourse);

            // save changes 
            await _employeeRepository.SaveChangesAsync();

            // execute business logic: when a course is attended, 
            // the suggested bonus must be recalculated
            employee.SuggestedBonus = employee.YearsInService
                * employee.AttendedCourses.Count * 100;
        }

        public ExternalEmployee CreateExternalEmployee(string firstName, string lastName, string company)
        {
            // create a new external employee with default values 
            var employe = (ExternalEmployee)_employeeFactory.CreateEmployee(firstName, lastName, company, true);

            // no obligatory courses for external employees, return it
            return employe;
        }

        public InternalEmployee CreateInternalEmployee(string firstName, string lastName)
        {
            // use the factory to create the object 
            var employee = (InternalEmployee)_employeeFactory.CreateEmployee(firstName, lastName);

            // apply business logic 

            // add obligatory courses attended by all new employees
            // during vetting process

            // get those courses  
            var obligatoryCourses = _employeeRepository.GetCourses(_obligatoryCourseIds);

            // add them for this employee
            foreach (var obligatoryCourse in obligatoryCourses)
            {
                employee.AttendedCourses.Add(obligatoryCourse);
            }

            // calculate the suggested bonus
            employee.SuggestedBonus = CalculateSuggestedBonus(employee);
            return employee;
        }

        public async Task<InternalEmployee> CreateInternalEmployeeAsync(string firstName, string lastName)
        {

            // use the factory to create the object 
            var employee = (InternalEmployee)_employeeFactory.CreateEmployee(firstName, lastName);

            // apply business logic 

            // add obligatory courses attended by all new employees
            // during vetting process

            // get those courses  
            var obligatoryCourses = await _employeeRepository.GetCoursesAsync(_obligatoryCourseIds);

            // add them for this employee
            foreach (var obligatoryCourse in obligatoryCourses)
            {
                employee.AttendedCourses.Add(obligatoryCourse);
            }

            // calculate the suggested bonus
            employee.SuggestedBonus = CalculateSuggestedBonus(employee);
            return employee;


        }

        public InternalEmployee? FetchInternalEmployee(Guid employeeId)
        {
            var employee = _employeeRepository.GetInternalEmployee(employeeId);

            if (employee != null)
            {
                // calculate fields
                employee.SuggestedBonus = CalculateSuggestedBonus(employee);
            }
            return employee;
        }

        public async Task<InternalEmployee?> FetchInternalEmployeeAsync(Guid employeeId)
        {
            var employee = await _employeeRepository.GetInternalEmployeeAsync(employeeId);

            if (employee != null)
            {
                // calculate fields
                employee.SuggestedBonus = CalculateSuggestedBonus(employee);
            }
            return employee;
        }

        public async Task<IEnumerable<InternalEmployee>> FetchInternalEmployeesAsync()
        {
            var employees = await _employeeRepository.GetInternalEmployeesAsync();

            foreach (var employee in employees)
            {
                // calculate fields
                employee.SuggestedBonus = CalculateSuggestedBonus(employee);
            }

            return employees;
        }


        public async Task GiveMinimumRaiseAsync(InternalEmployee employee)
        {
            employee.Salary += 100;
            employee.MinimumRaiseGiven = true;

            // save this
            await _employeeRepository.SaveChangesAsync();
        }

        public async Task GiveRaiseAsync(InternalEmployee employee, int raise)
        {
            // raise must be at least 100
            if (raise < 100)
            {
                throw new EmployeeInvalidRaiseException(
                    "Invalid raise: raise must be higher than or equal to 100.", raise);
                //throw new Exception(
                //  "Invalid raise: raise must be higher than or equal to 100.");
            }

            // if minimum raise was previously given, the raise must 
            // be higher than the minimum raise
            if (employee.MinimumRaiseGiven && raise == 100)
            {
                throw new EmployeeInvalidRaiseException(
                    "Invalid raise: minimum raise cannot be given twice.", raise);
            }

            if (raise == 100)
            {
                await GiveMinimumRaiseAsync(employee);
            }
            else
            {
                employee.Salary += raise;
                employee.MinimumRaiseGiven = false;
                await _employeeRepository.SaveChangesAsync();
            }
        }

        public void NotifyOfAbsence(Employee employee)
        {
            //Employee is absent.Other parts of the application may
            // respond to this. Trigger the EmployeeIsAbsent event 
            // (via a virtual method so it can be overridden in subclasses)
            OnEmployeeIsAbsent(new EmployeeIsAbsentEventArgs(employee.Id));
        }

        private int CalculateSuggestedBonus(InternalEmployee employee)
        {
            if (employee.YearsInService == 0)
            {
                return employee.AttendedCourses.Count * 100;
            }
            else
            {
                return employee.YearsInService
                    * employee.AttendedCourses.Count * 100;
            }
        }

        protected virtual void OnEmployeeIsAbsent(EmployeeIsAbsentEventArgs eventArgs)
        {
            EmployeeIsAbsent?.Invoke(this, eventArgs);
        }



    }
}
