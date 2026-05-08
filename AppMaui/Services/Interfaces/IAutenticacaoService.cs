using AppMaui.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Services.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<Usuario?> Autenticar(string usuario, string senha);
    }
}
