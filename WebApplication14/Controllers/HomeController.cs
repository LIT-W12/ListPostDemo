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

//create an application that displays a list of people. On top of the list
//have a link that takes you to a page that allows the user to enter multiple
//people. When that page loads, there should be one row of textboxes (first name,
//lastname and age) with a button on top that says "Add". When Add is clicked,
//another row of textboxes should appear. Beneath those textboxes, there should
//be a submit button, that when clicked, adds all those people to the database
//and then redirects the user back to the home page (with a success message showing on top).
//On the home page, in the first
//column of every row, have a checkbox. On top of that column (in the header) have a
//"Delete all" button. When this button is clicked, all the people that were checked
//off, should get deleted. The user should get redirected back to the home page, and
//a message should be displayed on top.
