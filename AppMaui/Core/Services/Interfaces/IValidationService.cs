using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Interfaces
{
    public interface IValidationService
    {
        bool ValidarCPF(string cpf);
        bool ValidarEmail(string email);
    }
}
