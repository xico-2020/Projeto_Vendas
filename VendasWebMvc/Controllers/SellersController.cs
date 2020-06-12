using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    }
}