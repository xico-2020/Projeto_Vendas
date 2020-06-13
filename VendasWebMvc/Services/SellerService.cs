using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendasWebMvc.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public void Insert(Seller obj)  // Para inserir o Vendedor na Base de Dados
        {
            _context.Add(obj);
            _context.SaveChanges();
        }

        public Seller FindById(int id)
        {
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);  // Include(obj => obj.Department -> Para que na View Seller em Details seja possivel ver o Departamento.
                                                    // Com o Include o EntityFrameWork junta os dados de duas tabelas.   
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }
    }
}
