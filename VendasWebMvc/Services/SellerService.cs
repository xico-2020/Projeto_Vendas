using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendasWebMvc.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VendasWebMvc.Services.Exceptions;
using VendasWebMvc.Services.Exceptions;

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

        public void Update(Seller obj)  // recebe um objeto do tipo Seller
        {   
            if (!_context.Seller.Any(x => x.Id == obj.Id))  // Verificar se na Base de Dados não existe um vendedor igual ao do objeto recebido no método.
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);  // Atualiza o objeto Seller na Base de Dados
                _context.SaveChanges(); // Guarda as alterações.

            }
            catch(DbConcurrecyException e)  // Intercepta a exceção do nível de acesso a dados e  relanço-a através da que criei a nível de serviço.
                                            // Organização por camadas. Tratamento a nível se serviço. O Controlador(SellersController)  só trata a exceção lançada pelo serviço.
            {
                throw new DbConcurrecyException(e.Message);
            }
        }
    }
}
