using System;
using System.Collections.Generic;
using System.Linq;

namespace VendasWebMvc.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();  // Ligação à Classe Seller (relação tem muitos...)

        public Department()
        {

        }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);  // Recebe como argumento o Vendedor da Coleção/ Lista Sellers adiciono o seller recebido.
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sellers.Sum(seller => seller.TotalSales(initial, final));  // Percorre a lista de vendedores e soma os totais de vendas de cada vendedor num determinado periodo.
        }

    }
}
