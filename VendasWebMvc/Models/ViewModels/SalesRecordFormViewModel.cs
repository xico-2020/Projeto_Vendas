using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendasWebMvc.Models.Enums;

namespace VendasWebMvc.Models.ViewModels
{
    public class SalesRecordFormViewModel
    {
        public SalesRecord SalesRecord { get; set; }

        public ICollection<Seller> Sellers { get; set; }  // Lista de Departamentos para poder selecionar na Caixa do Vendedor. Icollection por ser fonte de Dados + Genérica.
        //public ICollection<SaleStatus> SalesStatus { get; set; }  // Lista de Status para poder selecionar na Caixa do Status. Icollection por ser fonte de Dados + Genérica.
    }
}
