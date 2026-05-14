using AppMaui.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services
{
    public class SessaoService
    {
        public static Usuario? UsuarioLogado { get; private set; }

        //abre sessao
        public static void Login(Usuario usuario)
        {
            UsuarioLogado = usuario;
        }

        //fecha sessao
        public static void Logout()
        {
            UsuarioLogado = null;
        }
    }
}
