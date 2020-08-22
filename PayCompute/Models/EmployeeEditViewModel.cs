using Microsoft.AspNetCore.Http;
using PayCompute.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayCompute.Models
{
    public class EmployeeEditViewModel
    {
        public int Id { get; set; }
        public string EmployeeNo { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }
        [Display(Name = "Photo")]
        public IFormFile ImageUrl { get; set; }

        [DataType(DataType.Date), Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; } // Date of Birth

        public DateTime DateJoined { get; set; }

        public string Phone { get; set; }

        [Required(ErrorMessage = "Job Role is required"), StringLength(100)]
        public string Designation { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string NationalInsuranceNo { get; set; } // The same of Social Security No

        [Display(Name = "Payment Method")]
        public PaymentMethod paymentMethod { get; set; }
        [Display(Name = "Student Loan")]
        public StudentLoan studentLoan { get; set; }
        [Display(Name = "Union Member")]
        public UnionMember unionMember { get; set; }

        [Required, StringLength(150)]
        public string Address { get; set; }

        [Required, StringLength(50)]
        public string City { get; set; }

        [Required, StringLength(50), Display(Name = "Post Code")]
        public string PostCode { get; set; }
    }
}
