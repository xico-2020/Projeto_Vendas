using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendasWebMvc.Models;

namespace VendasWebMvc.Services
{
    public class SellerService
    {
        private readonly VendasWebMvcContext _context;  // Dependencia para o DBContext (Data -> VendasWebMvcContext).

        public SellerService(VendasWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList(); // Acede à tabela de vendedores e converte para uma lista.
        }
    }
}
