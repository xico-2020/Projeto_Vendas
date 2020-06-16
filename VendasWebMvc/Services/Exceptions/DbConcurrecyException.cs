//using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;


namespace VendasWebMvc.Services.Exceptions
{
    public class DbConcurrecyException : ApplicationException
    {
        public DbConcurrecyException(string message) : base(message)
        {

        }
    }
}
