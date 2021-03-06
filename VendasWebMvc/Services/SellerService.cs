﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendasWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using VendasWebMvc.Services.Exceptions;
using VendasWebMvc.Models.Enums;

namespace VendasWebMvc.Services
{
    public class SellerService
    {
        private readonly VendasWebMvcContext _context;  // Dependencia para o DBContext (Data -> VendasWebMvcContext). É readonly para que não possa ser alterada.

        public SellerService(VendasWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync(); // Acede à tabela de vendedores e converte para uma lista. De forma assincrona
        }

        // public void Insert(Seller obj)  // Para inserir o Vendedor na Base de Dados. Método Sincrono (mais lento)
        public async Task InsertAsync(Seller obj)  // Melhorado para método assincrono
        {
            _context.Add(obj);   // A operação Add é feita apenas na memória.
            await _context.SaveChangesAsync();  // Como só a operação SaveChanges é que acede à Base de Dados, apenas esta fica assincrona.
        }

        //public Seller FindByIdAsync(int id)  // Sincrono
        public async Task<Seller> FindByIdAsync(int id)  // Método assincrono
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);  // Include(obj => obj.Department -> Para que na View Seller em Details seja possivel ver o Departamento.
                                                    // Com o Include o EntityFrameWork junta os dados de duas tabelas.  Alterado para assincrono. FirstOrDefault é do LINQ. (Chama-se a esta operção de include eager loading.)
        }

        public async Task RemoveAsync(int id)
        {
            try  // bloco try para tratar exeção personalizada ao apagar vendedor com vendas. Não permitido pela BD e causa exceção de violação de integridade.
            {
                var obj = await _context.Seller.FindAsync(id);  // Alterado para sincrono. (adicionado o await e alterado para FindAsync em vez de Find.
                _context.Seller.Remove(obj);   // Remove do DBSet
                await _context.SaveChangesAsync();  // Assincrono
            }
            catch (DbUpdateException e)
            {
                //throw new IntergrityException(e.Message); // Mensagem do sistema
                throw new IntergrityException("Cant't delete seller because he/she has sales"); // Mensagem personalizada
            }
        }

        public async Task UpdateAsync(Seller obj)  // recebe um objeto do tipo Seller. Assincrono
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);  // Modificado por causa de método sincrono. O teste é feito antes do if.
            // if (!_context.Seller.Any(x => x.Id == obj.Id))  // Verificar se na Base de Dados não existe um vendedor igual ao do objeto recebido no método.
            if (!hasAny)
                {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);  // Atualiza o objeto Seller na Base de Dados
                await _context.SaveChangesAsync(); // Guarda as alterações. Assincrono.

            }
            catch(DbConcurrencyException e)  // Intercepta a exceção de conflito de concorrencia gerada pela Base de Dados que gera uma exceção com o nome DdConcurren, do nível de acesso a dados e  relanço-a através da que criei a nível de serviço.
                                            // Organização por camadas. Tratamento a nível se serviço. O Controlador(SellersController) só trata a exceção lançada pelo serviço.
            {
                throw new DbConcurrencyException(e.Message);   // Lança a exceção a nível de serviço ( a que criamos ) e envia a mensagem recebida da Base de Dados..
                            // Aqui intercetamos uma exceção do nível de acesso a dados e relancamo-la na exceção a nível de serviço e assim o controller lida apenas com a exceção de serviço.
            }
        }

    }
}
