using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Services.Interfaces
{
    public interface ICriptoService
    {
        string GerarHash(string senha);
        bool VerificarHash(string senhaDigitada, string hashSalvo);
    }
}
