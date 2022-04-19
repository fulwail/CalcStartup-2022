using CalcStartup.Identity.Model;
using CalcStartup.Service;
using CalcStartup.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalcStartup.Controllers
{
    public class NsiController : Controller
    {
        private readonly INsiService _nsiService;

        public NsiController(INsiService nsiService)
        {
            _nsiService = nsiService;
        }


        [Authorize(Roles = Role.GlobalAdmin)]
        public IActionResult Holidays ()
        {
            var errorMessage = TempData["Message"] as string;
            var holidays= _nsiService.GetHolidays();
            var holidaysViewModel = new HolidayViewModel { Holidays = holidays, Message= errorMessage };
            return View(holidaysViewModel);
        }
        [Authorize(Roles = Role.GlobalAdmin)]
        [HttpPost]
        public IActionResult DeleteHoliday(Guid id)
        {
             _nsiService.DeleteHoliday(id);
            return RedirectToAction("Holidays");
        }
        [Authorize(Roles = Role.GlobalAdmin)]
        [HttpPost]
        public IActionResult CreateHoliday(CreateHolidayViewModel createHoliday)
        {
            if (ModelState.IsValid)
            {
                _nsiService.CreateHoliday(createHoliday);
            }
            else
            {
                TempData["Message"] = "Все поля обязательны для заполнения";
            }
            return RedirectToAction("Holidays");
        }
        [Authorize(Roles = Role.GlobalAdmin)]
        [HttpPost]
        public IActionResult DeleteTariffRate(Guid id)
        {
            _nsiService.DeleteTariffRate(id);
            return RedirectToAction("TariffRates");
        }
        [Authorize(Roles = Role.GlobalAdmin)]
        [HttpPost]
        public IActionResult CreateTariffRate(CreateTariffRateViewModel tariffRateViewModel)
        {
            if (ModelState.IsValid)
            {
                _nsiService.CreateTariffRate(tariffRateViewModel);
            }
            else
            {
                TempData["Message"] = "Все поля обязательны для заполнения";
            }
          
            return RedirectToAction("TariffRates");

        }
        [Authorize(Roles = Role.GlobalAdmin)]
        public IActionResult TariffRates()
        {
            var errorMessage = TempData["Message"] as string;
            var tarriffRates = _nsiService.GetTariffRates();
            var tariffRatesView = new TariffRateViewModel
            {
                Message = errorMessage,
                TariffRates = tarriffRates
            };
            return View(tariffRatesView);
        }


        [Authorize(Roles = Role.GlobalAdmin)]
        public IActionResult DistrictCoefficients()
        {
            var errorMessage = TempData["Message"] as string;
            var districtCoefficients = _nsiService.GetDistrictCoefficient();
            var districtCoefficientsView = new DistrictCoefficientViewModel
            {
                Message = errorMessage,
                DistrictCoefficients = districtCoefficients
            };
            return View(districtCoefficientsView);
        }

        [Authorize(Roles = Role.GlobalAdmin)]
        [HttpPost]
        public IActionResult DeleteDistrictCoefficient(Guid id)
        {
            _nsiService.DeleteDistrictCoefficient(id);
            return RedirectToAction("DistrictCoefficients");
        }
        [Authorize(Roles = Role.GlobalAdmin)]
        [HttpPost]
        public IActionResult CreateDistrictCoefficient(CreateDistrictCoefficientsViewModel createDistrictCoefficientsViewModel)
        {
            if (ModelState.IsValid)
            {
                _nsiService.CreateDistrictCoefficient(createDistrictCoefficientsViewModel);
            }
            else
            {
                TempData["Message"] = "Все поля обязательны для заполнения";
            }

            return RedirectToAction("DistrictCoefficients");

        }
    }
}
