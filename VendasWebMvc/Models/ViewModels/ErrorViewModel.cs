using System;

namespace VendasWebMvc.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string Message  { get; set; }  // Acrescentado. // Propriedade que permite acrescentar mensagem costumizada neste objeto.

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}