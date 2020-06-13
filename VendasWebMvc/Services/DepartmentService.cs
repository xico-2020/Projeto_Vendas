using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using VendasWebMvc.Models;

namespace VendasWebMvc.Services
{
    public class DepartmentService
    {
        private readonly VendasWebMvcContext _context;  // Dependencia para o DBContext (Data -> VendasWebMvcContext).

        public DepartmentService(VendasWebMvcContext context)
        {
            _context = context;
        }

        public List<Department> FindAll()
        {
            return _context.Department.OrderBy(x => x.Name).ToList();
        }
    }
}
