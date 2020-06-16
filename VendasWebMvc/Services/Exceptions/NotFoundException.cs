using System;


namespace VendasWebMvc.Services.Exceptions
{
    public class NotFoundException : ApplicationException   // Esta Classe herda de ApplicationException
    {
        public NotFoundException(string message) : base(message)   // Construtor que recebe message e repassa para a classe base
        {

        }
    }
}
