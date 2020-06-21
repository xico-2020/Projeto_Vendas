    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using VendasWebMvc.Models;
    using VendasWebMvc.Models.Enums;
    using VendasWebMvc.Models.ViewModels;
    using VendasWebMvc.Services;

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
                    var viewModel = new SalesRecordFormViewModel {SalesRecord = salesRecord, Sellers = sellers };  // Gera lista de Vendedores
                    return View(viewModel);  // Se o modelo (valores introduzidos) não for válido, retorna a view enquanto o formulário não for corretamente preenchido.
                                             // É uma segunda validação para o caso de o navegador ter o JavaScrip desabilitado não cumprindo os testes definidos para os campos na Views -> Create
                }

                await _salesRecordService.InsertAsync(salesRecord);
                return RedirectToAction(nameof(Index));  // Redirecionar para a ação Index que é a que vai mostrar o Ecran de Vendas.
            }


            public async Task <IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
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

                var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
                return View(result);
            }

            public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
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

                var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);
                return View(result);
            }

        }
    }