using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendasWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using VendasWebMvc.Models.Enums;
using System.Security.Cryptography.X509Certificates;
using VendasWebMvc.Services.Exceptions;

namespace VendasWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly VendasWebMvcContext _context;  // Dependencia para o DBContext (Data -> VendasWebMvcContext).

        public SalesRecordService(VendasWebMvcContext context)
        {
            _context = context;
        }

        /*
        public async Task<List<SalesRecord>> FindAllAsync()
        {
            return await _context.SalesRecord.ToListAsync(); // Acede à tabela de vendas e converte para uma lista. De forma assincrona
        }
        */
        public async Task InsertAsync(SalesRecord obj)  // Melhorado para método assincrono
        {
            /*bool statusAll = returnedStatus.Equals(SaleStatus.All);
            
            if (!statusAll)
            {
                _context.Add(obj);   // A operação Add é feita apenas na memória.
                await _context.SaveChangesAsync();  // Como só a operação SaveChanges é que acede à Base de Dados, apenas esta fica assincrona.
            } */

            _context.Add(obj);   // A operação Add é feita apenas na memória.
            await _context.SaveChangesAsync();  // Como só a operação SaveChanges é que acede à Base de Dados, apenas esta fica assincrona.

        }

        public async Task<SalesRecord> FindByIdAsync(int id)  // Método assincrono
        {
            return await _context.SalesRecord.Include(obj => obj.Seller).FirstOrDefaultAsync(obj => obj.Id == id);  // Include(obj => obj.Seller -> Para que na View SalesRecord em Details seja possivel ver o Vendedor.
                                                                                                                   // Com o Include o EntityFrameWork junta os dados de duas tabelas.  Alterado para assincrono.
        }
        
        public async Task RemoveAsync(int id)
        {
            try  // bloco try para tratar exeção personalizada ao apagar vendedor com vendas. Não permitido pela BD e causa exceção de violação de integridade.
            {
                var obj = await _context.SalesRecord.FindAsync(id);  // Alterado para sincrono. (adicionado o await e alterado para FindAsync em vez de Find.
                _context.SalesRecord.Remove(obj);
                await _context.SaveChangesAsync();  // Assincrono
            }
            catch (DbUpdateException e)
            {
                //throw new IntergrityException(e.Message); // Mensagem do sistema
                throw new IntergrityException("Cant't delete seller because he/she has sales"); // Mensagem personalizada
            }
        }

        public async Task UpdateAsync(SalesRecord obj)  // recebe um objeto do tipo Seller. Assincrono
        {
            bool hasAny = await _context.SalesRecord.AnyAsync(x => x.Id == obj.Id);  // Modificado por causa de método sincrono. O teste é feito antes do if.
            // if (!_context.SalesRecordr.Any(x => x.Id == obj.Id))  // Verificar se na Base de Dados não existe um vendedor igual ao do objeto recebido no método.
            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);  // Atualiza o objeto SalesRecord na Base de Dados
                await _context.SaveChangesAsync(); // Guarda as alterações. Assincrono.

            }
            catch (DbConcurrecyException e)  // Intercepta a exceção do nível de acesso a dados e  relanço-a através da que criei a nível de serviço.
                                             // Organização por camadas. Tratamento a nível se serviço. O Controlador(SalesRecordController)  só trata a exceção lançada pelo serviço.
            {
                throw new DbConcurrecyException(e.Message);
            }
        }

        
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate, SaleStatus returnedStatus)
        {
            bool statusAll = returnedStatus.Equals(SaleStatus.All); 
            var result = from obj in _context.SalesRecord select obj;  // Vai ler um SalesRecord que é do tipo DbSet e vai construir um objeto result do tipo IQueryable(onde se pode construir as consultas).
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
                if (!statusAll)
                {
                    result = result.Where(x => x.Status == returnedStatus);
                }
                
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
                if (!statusAll)
                {
                    result = result.Where(x => x.Status == returnedStatus);
                }
            }

            return await result
                .Include(x => x.Seller)  // Faz o Join das tabelas
                .Include(x => x.Seller.Department)  // Faz o Join das tabelas
                .OrderByDescending(x => x.Date)  // Ordena por ordem decrescente
                .ToListAsync();  // Recebe a lista
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate, SaleStatus returnedStatus)  // Como é agrupamento de dados, não é List mas Igrouping.
        {
            bool statusAll = returnedStatus.Equals(SaleStatus.All);
            var result = from obj in _context.SalesRecord select obj;  // Vai ler um SalesRecord que é do tipo DbSet e vai construir um objeto result do tipo IQueryable(onde se pode construir as consultas).
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
                if (!statusAll)
                {
                    result = result.Where(x => x.Status == returnedStatus);
                }
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
                if (!statusAll)
                {
                    result = result.Where(x => x.Status == returnedStatus);
                }
            }

            return await result
                .Include(x => x.Seller)  // Faz o Join das tabelas
                .Include(x => x.Seller.Department)  // Faz o Join das tabelas
                .OrderByDescending(x => x.Date)  // Ordena por ordem decrescente
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();  // Recebe a lista
        }

        public async Task<List<IGrouping< Seller, SalesRecord>>> FindByDate1GroupingAsync(DateTime? minDate, DateTime? maxDate, SaleStatus returnedStatus)  // Como é agrupamento de dados, não é List mas Igrouping.
        {
            bool statusAll = returnedStatus.Equals(SaleStatus.All);
            var result = from obj in _context.SalesRecord select obj;  // Vai ler um SalesRecord que é do tipo DbSet e vai construir um objeto result do tipo IQueryable(onde se pode construir as consultas).
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
                if (!statusAll)
                {
                    result = result.Where(x => x.Status == returnedStatus);
                }
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
                if (!statusAll)
                {
                    result = result.Where(x => x.Status == returnedStatus);
                }
            }

            return await result
                //.Include(x => x.Seller)  // Faz o Join das tabelas
                .Include(x => x.Seller.Department)  // Faz o Join das tabelas
                .OrderByDescending(x => x.Date)  // Ordena por ordem decrescente
                .GroupBy(x => x.Seller)
                .ToListAsync();  // Recebe a lista
        }

        /*
        public  bool VerifyStatus()
        {
            bool hasStatus = _context.SalesRecord.Any(x => x.Status == SaleStatus.All);
            return hasStatus;
        }*/
    }
}
