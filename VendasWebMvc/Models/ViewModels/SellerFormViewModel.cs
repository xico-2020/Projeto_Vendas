using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VendasWebMvc.Models.ViewModels
{
    public class SellerFormViewModel
    {
        public Seller Seller { get; set; }
        public ICollection<Department> Departments { get; set; }  // Lista de Departamentos para poder selecionar na Caixa do Vendedor. Icollection por ser fonte de Dados + Genérica.

        /*[Required(ErrorMessage = "Your elegant error message goes here")]
        public int Salary { get; set; }*/
    }
}
