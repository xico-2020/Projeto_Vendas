using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VendasWebMvc.Models;
using VendasWebMvc.Models.ViewModels;
using VendasWebMvc.Services;

namespace VendasWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)  // Construtor
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }
        public IActionResult Index()  // Tem que chamar a FindAll do SellerService. Para isso crio acima uma dependencia para ele.
        {
            var list = _sellerService.FindAll();  // retorna lista de Seller
            return View(list);  // passo a lista como argumento do metodo View para gerar um IActionResult contendo esta lista.
        }

        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments }; // Inicia o SellerFormViewModels com a Lista de Departamentos da linha anterior.
            return View(viewModel);  // Quando o Ecran de Registo de Vendas pela primeira vez, já tem os dados dos Departamentos existentes.  
        }

        [HttpPost]    // Indica que  o Create vai ser uma ação de Post e não de Get.
        [ValidateAntiForgeryToken]  // Para prevenir ataques CSRF - Ataques maliciosos que aproveitam a sessão aberta.
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));  // REdirecionar para a ação Index que é a que vai mostrar o Ecran de Vendedores.
        }

        public IActionResult Delete(int? id)  // int? -> Indica que é opcional.
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value);  // id.Value -> Porque id é Nullable (porque é opcional). Só recebe o valor caso exista.
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}