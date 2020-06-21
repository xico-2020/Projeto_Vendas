﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendasWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using VendasWebMvc.Models.Enums;

namespace VendasWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly VendasWebMvcContext _context;  // Dependencia para o DBContext (Data -> VendasWebMvcContext).

        public SalesRecordService(VendasWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindAllAsync()
        {
            return await _context.SalesRecord.ToListAsync(); // Acede à tabela de vendas e converte para uma lista. De forma assincrona
        }

        public async Task InsertAsync(SalesRecord obj)  // Melhorado para método assincrono
        {
            _context.Add(obj);   // A operação Add é feita apenas na memória.
            await _context.SaveChangesAsync();  // Como só a operação SaveChanges é que acede à Base de Dados, apenas esta fica assincrona.
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

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)  // Como é agrupamento de dados, não é List mas Igrouping.
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
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();  // Recebe a lista
        }
    }
}
