using Microsoft.AspNetCore.Mvc.Rendering;
using PayCompute.Entity;
using PayCompute.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCompute.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        // Conection with DBContext
        private readonly ApplicationDbContext _context;
        private decimal studentLoanAmount;
        

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Employee newEmployee)
        {
            // Add's a new Employee
            await _context.Employees.AddAsync(newEmployee);
            await _context.SaveChangesAsync();
        }
        // Gets the employee that is passing through this method
        public Employee GetById(int employeeId) => _context.Employees.Where(e => e.Id == employeeId).FirstOrDefault();
        

        public async Task Delete(int employeeId)
        {
            // gets the employee by the method
            var employee = GetById(employeeId);
            // removes the employee and saves into database
            _context.Remove(employee);
            await _context.SaveChangesAsync();
        }
        
        public IEnumerable<Employee> GetAll() => _context.Employees;

        // updates the info of the employee
        public async Task UpdateAsync(Employee employee)
        {
            _context.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id)
        {
            var employee = GetById(id);
            _context.Update(employee);
            await _context.SaveChangesAsync();
        }

        // How to calculate student Loan Repayment  (UK)
        //https://www.gov.uk/repaying-your-student-loan/what-you-pay
        /*
         * You pay 9% of your income over the threshold
         * (Monthly Salary)
         * (Monthly Repayment)
         * 
         */
        public decimal StudenLoanRepaymentAmount(int id, decimal totalAmount)
        {
            var employee = GetById(id);

            if (employee.studentLoan == StudentLoan.Yes && totalAmount > 1750 && totalAmount < 2000)
            {
                studentLoanAmount = 15m;
            }
            else if (employee.studentLoan == StudentLoan.Yes && totalAmount > 2000 && totalAmount < 2250)
            {
                studentLoanAmount = 38m;
            }
            else if (employee.studentLoan == StudentLoan.Yes && totalAmount >= 2500)
            {
                studentLoanAmount = 83m;
            }
            else
            {
                studentLoanAmount = 0m;
            }

            return studentLoanAmount;

        }

        public decimal UnionFees(int id)
        {
            var employee = GetById(id);
            var fee = employee.unionMember == UnionMember.Yes ? 10m : 0m;

            return fee;
        }

        public IEnumerable<SelectListItem> GetAllEmployeesForPayroll()
        {
            return GetAll().Select(emp => new SelectListItem()
            {
                Text = emp.FullName,
                Value = emp.Id.ToString()
            });
        }
    }
}
