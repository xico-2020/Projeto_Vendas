﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  // Permite definir anotações nos campos a mostrar no Ecran. Através de [Display]

using System.Linq;

namespace VendasWebMvc.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} Obrigatório introduzir um valor!")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Apenas letras são permitidas!")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} : O nome tem ser maior que {2}  e menor que {1}")]
        [Display(Name = "Nome Departamento")]
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
