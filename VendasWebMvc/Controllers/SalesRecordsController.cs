using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using VendasWebMvc.Models;
using VendasWebMvc.Models.Enums;
using VendasWebMvc.Models.ViewModels;
using VendasWebMvc.Services;
using VendasWebMvc.Services.Exceptions;

namespace VendasWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;

        public SalesRecordsController(SalesRecordService salesRecordService, SellerService sellerService)
        {
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var sellers = await _sellerService.FindAllAsync();
            var viewModel = new SalesRecordFormViewModel { Sellers = sellers }; // Inicia o SalesRecordFormViewModels com a Lista de Vendedores da linha anterior.
            return View(viewModel);  // Quando o Ecran de Registo de Vendas pela primeira vez, já tem os dados dos Vendedores existentes.  
        }

        [HttpPost]    // Indica que  o Create vai ser uma ação de Post e não de Get.
        [ValidateAntiForgeryToken]  // Para prevenir ataques CSRF - Ataques maliciosos que aproveitam a sessão aberta.
        public async Task<IActionResult> Create(SalesRecord salesRecord)
        {
            if (!ModelState.IsValid)
            {
                var sellers = await _sellerService.FindAllAsync();  // Le os vendedores
                var viewModel = new SalesRecordFormViewModel { SalesRecord = salesRecord, Sellers = sellers };  // Gera lista de Vendedores
                return View(viewModel);  // Se o modelo (valores introduzidos) não for válido, retorna a view enquanto o formulário não for corretamente preenchido.
                                         // É uma segunda validação para o caso de o navegador ter o JavaScrip desabilitado não cumprindo os testes definidos para os campos na Views -> Create
            }

            await _salesRecordService.InsertAsync(salesRecord);
            return RedirectToAction(nameof(Index));  // Redirecionar para a ação Index que é a que vai mostrar o Ecran de Vendas.
        }

        public async Task<IActionResult> Delete(int? id)  // int? -> Indica que é opcional. 
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _salesRecordService.FindByIdAsync(id.Value);  // id.Value -> Porque id é Nullable (porque é opcional). Só recebe o valor caso exista.
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try   // Bloco try para capturar excewção de violação de integridade. (Não pode apagar vendedor se este tiver vendas).
            {
                await _salesRecordService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntergrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> Edit(int? id)  // Este método serve para abrir o ecran de vendedor para o editar.
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _salesRecordService.FindByIdAsync(id.Value);  //  obj recebe o _sellerService passando como argumento o id
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            List<Seller> sellers = await _sellerService.FindAllAsync(); // Se passou nos testes anteriores(Testes de não existe) leio os departamentos.

            SalesRecordFormViewModel viewModel = new SalesRecordFormViewModel { SalesRecord = obj, Sellers = sellers }; // Preencho o SellerFormViewModel com os dados de Seller do obj que acima fomos ler à Base de Dados. Preencho tambem os Departamentos.
            return View(viewModel);  // retornar a View preenchida com os dados de viewModel.
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalesRecord salesRecord)
        {
            if (!ModelState.IsValid)
            {
                var sellers = await _sellerService.FindAllAsync();
                var viewModel = new SalesRecordFormViewModel { SalesRecord = salesRecord, Sellers = sellers };
                return View(viewModel);  // Se o modelo (valores introduzidos) não for válido, retorna a view enquanto o formulário não for corretamente preenchido.
                                         // É uma segunda validação para o caso de o navegador ter o JavaScrip desabilitado não cumprindo os testes definidos para os campos na Views -> Create
            }

            if (id != salesRecord.Id)  // Testar se o id passado no método é diferente do vendedor. O Id do vendedor não pode ser |= do do URL da requisição.
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });  // Id não coincide
            }

            try
            {
                await _salesRecordService.UpdateAsyc(salesRecord);  // Atualiza o vendedor
                return RedirectToAction(nameof(Index));  // Redirecionar para a página inicial que é a Index.

            }
            catch (ApplicationException e) // Como a exceção por si já tem uma mensagem, então para a tratar crio um apelido ( neste caso "e").
            {
                return RedirectToAction(nameof(Error), new { message = e.Message }); // retorna a mensagem da exceção.
            }

        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate, SaleStatus returnedStatus)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);  // Se não for indicada data minima, inicializo a pesquisa com o dia 01/01/Ano atual.
            }

            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;  // Se não for indicada data máxima, inicializo a pesquisa com a data atual.
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");  // passar os valores de minDate e maxDate para a view
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate, returnedStatus);
            return View(result);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate, SaleStatus returnedStatus)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);  // Se não for indicada data minima, inicializo a pesquisa com o dia 01/01/Ano atual.
            }

            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;  // Se não for indicada data máxima, inicializo a pesquisa com a data atual.
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");  // passar os valores de minDate e maxDate para a view
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate, returnedStatus);
            return View(result);
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

