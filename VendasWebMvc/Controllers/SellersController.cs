using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VendasWebMvc.Models;
using VendasWebMvc.Models.ViewModels;
using VendasWebMvc.Services;
using VendasWebMvc.Services.Exceptions;

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

        public IActionResult Details(int? id)
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

        public IActionResult Edit(int? id)  // Este método serve para abrir o ecran de vendedor para o editar.
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value);  //  obj recebe o _sellerService passando como argumento o id
            if (obj == null)
            {
                return NotFound();
            }

            List<Department> departments = _departmentService.FindAll(); // Se passou nos testes anteriores(Testes de não existe) leio os departamentos.
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments }; // Preencho o SellerFormViewModel com os dados de Seller do obj que acima fomos ler à Base de Dados. Preencho tambem os Departamentos.
            return View(viewModel);  // retornar a View preenchida com os dados de viewModel.
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id)  // Testar se o id passado no método é diferente do vendedor. O Id do vendedor não pode ser |= do do URL da requisição.
            {
                return BadRequest();
            }

            try
            {
                _sellerService.Update(seller);  // Atualiza o vendedor
                return RedirectToAction(nameof(Index));  // Redirecionar para a página inicial que é a Index.

            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            catch (DbConcurrecyException)
            {
                return BadRequest();
            }
        }
    }
}