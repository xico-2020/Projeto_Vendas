using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VendasWebMvc.Services;

namespace VendasWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;

        public SalesRecordsController(SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }

        public IActionResult Index()
        {
            return View();
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