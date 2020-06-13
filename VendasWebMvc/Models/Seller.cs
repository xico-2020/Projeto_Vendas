using System;
using System.Collections.Generic;
using System.Linq;

namespace VendasWebMvc.Models
{
    public class Seller
    {
        public int Id  { get; set; }
        public string Name  { get; set; }
        public string Email  { get; set; }
        public DateTime BirthDate { get; set; }
        public double Salary { get; set; }
        public Department Department { get; set; }  // Para ler a Classe Department.
        public int DepartmentId { get; set; }  // Para garantir ao EntityFramework que vai existir um Id de Departamento, uma vez que um int não pode ser nulo. Declarando DepartmentId o FrameWork consegue relacionar com o Id de Seller e criar a BD corretamente.
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>(); // Ligação à Classe SalesRecord(relação tem muitos...)


        public Seller()
        {

        }

        public Seller(int id, string name, string email, DateTime birthDate, double salary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            Salary = salary;
            Department = department;
        }


        public void AddSales(SalesRecord sr)  // Método recebe SalesRecord (Coleção / Lista)
        {
            Sales.Add(sr);  // Sales é o nome da coleção de vendas associada ao vendedor.
        }

        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr => sr.Amount); // Calcula a soma entre as datas do parametro Amount.
        }
    }
}
