using System;

namespace VendasWebMvc.Services.Exceptions
{
    public class IntergrityException : ApplicationException
    {
        public IntergrityException(string message) : base(message)  // construtor
        {
        }
    }
}
