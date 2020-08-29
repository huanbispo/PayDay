using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayCompute.Entity;
using PayCompute.Models;
using PayCompute.Services;
using RotativaCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCompute.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class PayController : Controller
    {
        private readonly IPayComputationService _payComputationService;
        private readonly IEmployeeService _employeeService;
        private readonly ITaxService _taxService;
        private readonly INationalInsuranceContributionService _nationalInsuranceContributionService;
        private decimal overtimeHrs;
        private decimal contractualEarnings;
        private decimal overtimeEarnings;
        private decimal totalEarning;
        private decimal tax;
        private decimal nic;
        private decimal unionFee;
        private decimal slc;
        private decimal totalDeduct;

        public PayController(IPayComputationService payComputationService,
            IEmployeeService employeeService,
            ITaxService taxService,
            INationalInsuranceContributionService nationalInsuranceContributionService)
        {
            _payComputationService = payComputationService;
            _employeeService = employeeService;
            _taxService = taxService;
            _nationalInsuranceContributionService = nationalInsuranceContributionService;
        }

        public IActionResult Index()
        {
            var payRecord = _payComputationService.GetAll().Select(pay => new PaymentRecordIndexViewModel 
            {
                Id = pay.Id,
                EmployeeId = pay.EmployeeId,
                FullName = pay.FullName,
                PayDate = pay.PayDate,
                PayMonth = pay.PayMonth,
                TaxYearId = pay.TaxYearId,
                Year = _payComputationService.GetTaxYearById(pay.TaxYearId).YearOfTax,
                TotalEarnings = pay.TotalEarnings,
                TotalDeduction = pay.TotalDeduction,
                NetPayment = pay.NetPayment,
                Employee = pay.Employee
            });

            return View(payRecord);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.employees = _employeeService.GetAllEmployeesForPayroll();
            ViewBag.taxYears = _payComputationService.GetAllTaxYear();
            var model = new PaymentRecordCreateViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PaymentRecordCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var payRecord = new PaymentRecord()
                {
                    Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    FullName = _employeeService.GetById(model.EmployeeId).FullName,
                    NiNo = _employeeService.GetById(model.EmployeeId).NationalInsuranceNo,
                    PayDate = model.PayDate,
                    PayMonth = model.PayMonth,
                    TaxYearId = model.TaxYearId,
                    TaxCode = model.TaxCode,
                    HourlyRate = model.HourlyRate,
                    HoursWorked = model.HoursWorked,
                    ContractualHours = model.ContractualHours,
                    ContractualEarnings = contractualEarnings =  _payComputationService.ContractualEarnings(model.ContractualHours, model.HoursWorked, model.HourlyRate),                    
                    OvertimeHours = overtimeHrs =  _payComputationService.OverTimeHours(model.HoursWorked, model.ContractualHours),
                    OvertimeEarning = overtimeEarnings = _payComputationService.OvertimeEarnings(_payComputationService.OvertimeRate(model.HourlyRate), overtimeHrs),
                    TotalEarnings = totalEarning = _payComputationService.TotalEarnings(overtimeEarnings, contractualEarnings),
                    Tax = tax = _taxService.TaxAmount(totalEarning),
                    UnionFee = unionFee = _employeeService.UnionFees(model.EmployeeId),
                    SLC = slc = _employeeService.StudenLoanRepaymentAmount(model.EmployeeId, totalEarning),
                    NIC = nic = _nationalInsuranceContributionService.NIContribution(totalEarning),
                    TotalDeduction = totalDeduct = _payComputationService.TotalDeduction(tax, nic, slc, unionFee),
                    NetPayment = _payComputationService.NetPay(totalEarning, totalDeduct)
                };

                await _payComputationService.CreateAsync(payRecord);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.employees = _employeeService.GetAllEmployeesForPayroll();
            ViewBag.taxYears = _payComputationService.GetAllTaxYear();

            return View();
        }

        public IActionResult Detail(int id)
        {
            var paymentRecord = _payComputationService.GetbyId(id);
            if (paymentRecord == null)
            {
                return NotFound();
            }

            var model = new PaymentRecordDetailViewModel()
            {
                Id = paymentRecord.Id,
                EmployeeId = paymentRecord.EmployeeId,
                FullName = paymentRecord.FullName,
                NiNo = paymentRecord.NiNo,
                PayDate = paymentRecord.PayDate,
                PayMonth = paymentRecord.PayMonth,
                TaxYearId = paymentRecord.TaxYearId,
                Year = _payComputationService.GetTaxYearById(paymentRecord.TaxYearId).YearOfTax,
                TaxCode = paymentRecord.TaxCode,
                HourlyRate = paymentRecord.HourlyRate,
                HoursWorked = paymentRecord.HoursWorked,
                ContractualHours = paymentRecord.ContractualHours,
                ContractualEarnings = paymentRecord.ContractualEarnings,
                OvertimeEarning = paymentRecord.OvertimeEarning,
                OvertimeHours = paymentRecord.OvertimeHours,
                OvertimeRate = _payComputationService.OvertimeRate(paymentRecord.HourlyRate),
                Tax = paymentRecord.Tax,
                NIC = paymentRecord.NIC,
                SLC = paymentRecord.SLC,
                UnionFee = paymentRecord.UnionFee,
                TotalEarnings = paymentRecord.TotalEarnings,
                TotalDeduction = paymentRecord.TotalDeduction,
                Employee = paymentRecord.Employee,
                taxYear = paymentRecord.taxYear,
                NetPayment = paymentRecord.NetPayment
            };

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Payslip(int id)
        {
            var paymentRecord = _payComputationService.GetbyId(id);
            if (paymentRecord == null)
            {
                return NotFound();
            }

            var model = new PaymentRecordDetailViewModel()
            {
                Id = paymentRecord.Id,
                EmployeeId = paymentRecord.EmployeeId,
                FullName = paymentRecord.FullName,
                NiNo = paymentRecord.NiNo,
                PayDate = paymentRecord.PayDate,
                PayMonth = paymentRecord.PayMonth,
                TaxYearId = paymentRecord.TaxYearId,
                Year = _payComputationService.GetTaxYearById(paymentRecord.TaxYearId).YearOfTax,
                TaxCode = paymentRecord.TaxCode,
                HourlyRate = paymentRecord.HourlyRate,
                HoursWorked = paymentRecord.HoursWorked,
                ContractualHours = paymentRecord.ContractualHours,
                ContractualEarnings = paymentRecord.ContractualEarnings,
                OvertimeEarning = paymentRecord.OvertimeEarning,
                OvertimeHours = paymentRecord.OvertimeHours,
                OvertimeRate = _payComputationService.OvertimeRate(paymentRecord.HourlyRate),
                Tax = paymentRecord.Tax,
                NIC = paymentRecord.NIC,
                SLC = paymentRecord.SLC,
                UnionFee = paymentRecord.UnionFee,
                TotalEarnings = paymentRecord.TotalEarnings,
                TotalDeduction = paymentRecord.TotalDeduction,
                Employee = paymentRecord.Employee,
                taxYear = paymentRecord.taxYear,
                NetPayment = paymentRecord.NetPayment
            };

            return View(model);
        }

        public IActionResult GeneratePayslipPDF(int id)
        {
            /* Rotativa Core Framework (3.0)
             * We gonna use the third overload (passing just two parameter, 
             *                                  The nameOfFile and the ID of the PayRecord)
             */
            var payslip = new ActionAsPdf("Payslip", new { id = id })
            {
                // The fileName that is gonna be downloaded
                FileName = "Payslip.pdf"
            };

            return payslip;
        }

    }
}
