using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
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
        // public IActionResult Index()  // Tem que chamar a FindAll do SellerService. Para isso crio acima uma dependencia para ele. Não assincrono
        public async Task<IActionResult> Index()   // Assincrono
        {
            //var list = _sellerService.FindAll();  // retorna lista de Seller . Método sincrono
            var list = await _sellerService.FindAllAsync();  // Método assincrono
            return View(list);  // passo a lista como argumento do metodo View para gerar um IActionResult contendo esta lista.
        }

        public async Task <IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments }; // Inicia o SellerFormViewModels com a Lista de Departamentos da linha anterior.
            return View(viewModel);  // Quando o Ecran de Registo de Vendas pela primeira vez, já tem os dados dos Departamentos existentes.  
        }

        [HttpPost]    // Indica que  o Create vai ser uma ação de Post e não de Get.
        [ValidateAntiForgeryToken]  // Para prevenir ataques CSRF - Ataques maliciosos que aproveitam a sessão aberta.
        public async Task <IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();  // Le os departamentos
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };  // Gera lista de Vendedores
                return View(viewModel);  // Se o modelo (valores introduzidos) não for válido, retorna a view enquanto o formulário não for corretamente preenchido.
                                      // É uma segunda validação para o caso de o navegador ter o JavaScrip desabilitado não cumprindo os testes definidos para os campos na Views -> Create
            }

            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));  // REdirecionar para a ação Index que é a que vai mostrar o Ecran de Vendedores.
        }

        public async Task <IActionResult> Delete(int? id)  // int? -> Indica que é opcional. 
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new {message =  "Id not provided"} );
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);  // id.Value -> Porque id é Nullable (porque é opcional). Só recebe o valor caso exista.
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Delete(int id)
        {
            await _sellerService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task <IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);  // id.Value -> Porque id é Nullable (porque é opcional). Só recebe o valor caso exista.
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        public async Task <IActionResult> Edit(int? id)  // Este método serve para abrir o ecran de vendedor para o editar.
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);  //  obj recebe o _sellerService passando como argumento o id
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            List<Department> departments = await _departmentService.FindAllAsync(); // Se passou nos testes anteriores(Testes de não existe) leio os departamentos.
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments }; // Preencho o SellerFormViewModel com os dados de Seller do obj que acima fomos ler à Base de Dados. Preencho tambem os Departamentos.
            return View(viewModel);  // retornar a View preenchida com os dados de viewModel.
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);  // Se o modelo (valores introduzidos) não for válido, retorna a view enquanto o formulário não for corretamente preenchido.
                                         // É uma segunda validação para o caso de o navegador ter o JavaScrip desabilitado não cumprindo os testes definidos para os campos na Views -> Create
            }

            if (id != seller.Id)  // Testar se o id passado no método é diferente do vendedor. O Id do vendedor não pode ser |= do do URL da requisição.
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });  // Id não coincide
            }

            try
            {
                await _sellerService.UpdateAsyc(seller);  // Atualiza o vendedor
                return RedirectToAction(nameof(Index));  // Redirecionar para a página inicial que é a Index.

            }
            catch (ApplicationException e) // Como a exceção por si já tem uma mensagem, então para a tratar crio um apelido ( neste caso "e").
            {
                return RedirectToAction(nameof(Error), new { message = e.Message }); // retorna a mensagem da exceção.
            }

            // As duas condições abaixo foram substituidas pela acima pois ApplicationException é a superclasse das duas abaixo e então é feito o Upcasting.

            /*catch (NotFoundException e) // Como a exceção por si já tem uma mensagem, então para a tratar crio um apelido ( neste caso "e").
            {
                return RedirectToAction(nameof(Error), new { message = e.Message }); // retorna a mensagem da exceção.
            }

            catch (DbConcurrecyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message }); // retorna a mensagem da exceção.
            }*/
        }

        public IActionResult Error(string message)   // Como não tem acesso à Base de Dados não é necessário ser assincrona.
        {
            var viewModel = new ErrorViewModel()
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier // Código para achar o Id interno da requisição.
            };
            return View(viewModel);
        }
    }
}