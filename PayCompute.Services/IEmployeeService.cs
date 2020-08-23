using Microsoft.AspNetCore.Mvc.Rendering;
using PayCompute.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayCompute.Services
{
    public interface IEmployeeService
    {
        Task CreateAsync(Employee newEmployee);
        Employee GetById(int employeeId);
        Task UpdateAsync(Employee employee);
        Task UpdateAsync(int id);
        Task Delete(int employeeId);
        decimal UnionFees(int id);
        decimal StudenLoanRepaymentAmount(int id, decimal totalAmount); // Calculates the Repayment of the Student Loan
        IEnumerable<Employee> GetAll(); // Get All Employees
        IEnumerable<SelectListItem> GetAllEmployeesForPayroll(); // Get all the employee in another view for dropdown, that's why SelectListItem
    }
}
