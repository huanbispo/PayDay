using PayCompute.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace PayCompute.Models
{
    public class PaymentRecordCreateViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public string FullName { get; set; }

        public string NiNo { get; set; } // National Insurance Number
        [DataType(DataType.Date), Display(Name = "Pay Date")]
        public DateTime PayDate { get; set; } = DateTime.UtcNow;
        [Display(Name = "Pay Month")]
        public string PayMonth { get; set; } = DateTime.Today.Month.ToString();

        public TaxYear taxYear { get; set; }

        [Display(Name = "Tax Year")]
        public int TaxYearId { get; set; }
        // Default Tax Code
        public string TaxCode { get; set; } = "1250L";

        [Display(Name = "Hourly Rate")]
        public decimal HourlyRate { get; set; }
        [Display(Name = "Hours Worked")]
        public decimal HoursWorked { get; set; }
        [Display(Name = "Contractual Hours")]
        public decimal ContractualHours { get; set; } = 144m;
        public decimal OvertimeHours { get; set; }
        public decimal ContractualEarnings { get; set; }
        public decimal OvertimeEarning { get; set; }
        public decimal Tax { get; set; }
        public decimal NIC { get; set; } // National Insurance Contribution
        public decimal? UnionFee { get; set; }
        public Nullable<decimal> SLC { get; set; } // Student Loan (The student payment will not apply to all employees, so it is optional)
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal NetPayment { get; set; }
    }
}
