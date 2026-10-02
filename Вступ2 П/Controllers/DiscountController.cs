using Microsoft.AspNetCore.Mvc;
using DiscountApp.Models;

namespace DiscountApp.Controllers
{
    public class DiscountController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = new DiscountViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(DiscountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var discountAmount = model.Price * model.DiscountPercent / 100;
                model.FinalPrice = model.Price - discountAmount;
                model.Message = $"Ваша економія склала {discountAmount:F2} грн";
            }

            return View(model);
        }
    }
}
