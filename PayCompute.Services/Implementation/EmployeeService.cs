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

        public decimal StudenLoanRepaymentAmount(int id, decimal totalAmount)
        {
            throw new NotImplementedException();
        }

        public decimal UnionFees(int id)
        {
            throw new NotImplementedException();
        }

    }
}
