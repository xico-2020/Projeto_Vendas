using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;  // Necessário para operações assincronas
using VendasWebMvc.Models;
using Microsoft.EntityFrameworkCore; // Para poder usar o ToListAsync que é operação do EntityFramework, ao passo que o ToList é operação do Link.

namespace VendasWebMvc.Services
{
    public class DepartmentService
    {
        private readonly VendasWebMvcContext _context;  // Dependencia para o DBContext (Data -> VendasWebMvcContext).

        public DepartmentService(VendasWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> FindAllAsync()  // Async: Tornar a consulta asincrona para melhorar a perfomance da aplicação.
        {
            //return _context.Department.OrderBy(x => x.Name).ToList();  // Antes de tornar a consulta asincrona
            return await _context.Department.OrderBy(x => x.Name).ToListAsync(); // await e ToListAsync para tornar a consulta à Base de Dados Asincrona.
                   // Assincrona causa a consulta num processo diferente da Aplicação. Quando termina retorna a consulta e volta ao ponto onde estava. A Aplicação fica mais rápida.
        }
    }
}
