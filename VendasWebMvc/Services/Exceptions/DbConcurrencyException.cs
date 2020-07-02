//using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;


namespace VendasWebMvc.Services.Exceptions
{
    public class DbConcurrencyException : ApplicationException   // Esta Classe herda de ApplicationException
    {
        public DbConcurrencyException(string message) : base(message)    // Construtor que recebe message e repassa para a classe base
        {
        }
    }
}
