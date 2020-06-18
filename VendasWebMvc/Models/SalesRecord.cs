using System;
using VendasWebMvc.Models.Enums;
using System.ComponentModel.DataAnnotations;  // Permite definir anotações nos campos a mostrar no Ecran. Através de [Display]

namespace VendasWebMvc.Models
{
    public class SalesRecord
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]  // Para pedir apenas a data na inserção de dados
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")] // mostra a data no formato especificado, em que 0 é o valor do campo.
        public DateTime Date { get; set; }
        [DisplayFormat(DataFormatString = "{0:0,0.00}")]  // O formato do salário tem duas casas decimais.
        public double Amount { get; set; }
        public SaleStatus Status { get; set; }
        public Seller Seller { get; set; } // Ligação à Classe Seller (Cada Registo de Venda possui um Vendedor).

        public SalesRecord()
        {

        }

        public SalesRecord(int id, DateTime date, double amount, SaleStatus status, Seller seller)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Status = status;
            Seller = seller;
        }
    }
}
