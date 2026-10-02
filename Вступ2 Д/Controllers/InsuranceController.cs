using Microsoft.AspNetCore.Mvc;
using InsuranceApp.Models;

namespace InsuranceApp.Controllers
{
    public class InsuranceController : Controller
    {
        private const decimal BaseRate = 1000m;

        [HttpGet]
        public IActionResult Index()
        {
            var model = new InsuranceViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(InsuranceViewModel model)
        {
            if (model.EngineCapacity <= 0 || model.DriverExperienceYears < 0)
            {
                model.ErrorMessage = "Перевірте дані: об'єм двигуна має бути більшим за нуль, а стаж — невід'ємним.";
                model.CalculatedPrice = 0;
                return View(model);
            }

            decimal engineCoefficient = model.EngineCapacity <= 1600
                ? 1.0m
                : model.EngineCapacity <= 2000
                    ? 1.3m
                    : 1.6m;

            decimal cityCoefficient = model.CityType switch
            {
                "Kyiv" => 1.8m,
                "LargeCity" => 1.3m,
                _ => 1.0m
            };

            decimal price = BaseRate * engineCoefficient * cityCoefficient;

            if (model.DriverExperienceYears > 3)
            {
                price *= 0.9m;
            }

            if (model.HasDiscountCategory)
            {
                price *= 0.8m;
            }

            model.CalculatedPrice = price;
            model.ErrorMessage = null;

            return View(model);
        }
    }
}
