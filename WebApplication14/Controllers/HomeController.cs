using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication14.Data;
using WebApplication14.Models;

namespace WebApplication14.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString =
           "Data Source=.\\sqlexpress;Initial Catalog=FurnitureStore;Integrated Security=True;TrustServerCertificate=true;";
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PostNumbers(List<int> numbers)
        {
            return View(new HomeViewModel
            {
                Numbers = numbers
            });
        }


        public IActionResult ShowFurniture()
        {
            var db = new FurnitureDb(_connectionString);
            var vm = new ShowFurnitureViewModel
            {
                FurnitureItems = db.GetAll()
            };

            if (TempData["message"] != null)
            {
                vm.Message = (string)TempData["message"];
            }
            return View(vm);
        }

        [HttpPost]
        public IActionResult DeleteMany(List<int> ids)
        {
            var db = new FurnitureDb(_connectionString);
            db.DeleteMultiple(ids);
            TempData["message"] = $"{ids.Count} items deleted successfully";
            return Redirect("/home/ShowFurniture");
        }
        public IActionResult ShowFurnitureAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddFurniture(List<FurnitureItem> items)
        {
            var db = new FurnitureDb(_connectionString);
            db.AddMultiple(items);
            TempData["message"] = $"{items.Count} items added successfully";
            return Redirect("/home/ShowFurniture");
        }
    }
}
