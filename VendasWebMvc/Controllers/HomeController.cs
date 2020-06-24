using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VendasWebMvc.Models.ViewModels;

namespace VendasWebMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Vendas Web MVC App from C# Course";
            return View();
        }

        public IActionResult Index1()
        {
            return View();
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Vendas Web MVC App from C# Course"; 
            ViewData["Professor"] = "Nelio Alves";  // Acrescentado

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Página de contacto";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
