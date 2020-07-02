using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VendasWebMvc.Models.ViewModels
{
    public class SellerFormViewModel
    {
        public Seller Seller { get; set; }
        public ICollection<Department> Departments { get; set; }  // Lista de Departamentos para poder selecionar na Caixa do Vendedor. Icollection por ser fonte de Dados + Genérica.
                                                   // Departments: Importante. No plural pois ajuda o Framework a reconhecer os dados. Converte automáticamente de HTTP para objeto.     
        }
}
