using PayCompute.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayCompute.Models
{
    // Our view model ( only used to display data )
    public class EmployeeDetailViewModel
    {
        public int Id { get; set; }

        public string EmployeeNo { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }

        public string ImageUrl { get; set; }

        public DateTime DOB { get; set; } // Date of Birth

        public DateTime DateJoined { get; set; }

        public string Phone { get; set; }

        public string Designation { get; set; }

        public string Email { get; set; }

        public string NationalInsuranceNo { get; set; } // The same of Social Security No

        public PaymentMethod paymentMethod { get; set; }

        public StudentLoan studentLoan { get; set; }

        public UnionMember unionMember { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }


    }
}
