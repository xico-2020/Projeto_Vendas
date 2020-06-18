using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  // Permite definir anotações nos campos a mostrar no Ecran. Através de [Display]
using System.Linq;

namespace VendasWebMvc.Models
{
    public class Seller
    {
        public int Id  { get; set; }
        [Required(ErrorMessage = "{0} required")]  // 0= Nome do atributo (Name)
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} e {1}")] // 0= parametro 1. 2= parametro 3 e 1= parametro 2 .
        public string Name  { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        [DataType(DataType.EmailAddress)]  // O email mostrado assume forma de link para poder diretamente envia mail.
        public string Email  { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [Display( Name = "Birth Date")]
        [DataType(DataType.Date)]  // Para pedir apenas a data na inserção de dados
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")] // mostra a data no formato especificado, em que 0 é o valor do campo.
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")]
        [Display(Name = "Base Salary")]
        [DisplayFormat(DataFormatString = "{0:0,0.00}")]  // O formato do salário tem duas casas decimais.
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
