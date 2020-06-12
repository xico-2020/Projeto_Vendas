using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VendasWebMvc.Models;
using VendasWebMvc.Services;

namespace VendasWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;

        public SellersController(SellerService sellerService)  // Construtor
        {
            _sellerService = sellerService;
        }
        public IActionResult Index()  // Tem que chamar a FindAll do SellerService. Para isso crio acima uma dependencia para ele.
        {
            var list = _sellerService.FindAll();  // retorna lista de Seller
            return View(list);  // passo a lista como argumento do metodo View para gerar um IActionResult contendo esta lista.
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]    // Indica que  o Create vai ser uma ação de Post e não de Get.
        [ValidateAntiForgeryToken]  // Para prevenir ataques CSRF - Ataques maliciosos que aproveitam a sessão aberta.
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));  // REdirecionar para a ação Index que é a que vai mostrar o Ecran de Vendedores.
        }
    }
}