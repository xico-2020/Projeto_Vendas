using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendasWebMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace VendasWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly VendasWebMvcContext _context;  // Dependencia para o DBContext (Data -> VendasWebMvcContext).

        public SalesRecordService(VendasWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;  // Vai ler um SalesRecord que é do tipo DbSet e vai construir um objeto result do tipo IQueryable(onde se pode construir as consultas).
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller)  // Faz o Join das tabelas
                .Include(x => x.Seller.Department)  // Faz o Join das tabelas
                .OrderByDescending(x => x.Date)  // Ordena por ordem decrescente
                .ToListAsync();  // Recebe a lista
        }
    }
}
