using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local
{
    public class CriptogramaService
    {
        //
        public List<LetraCriptografada> Gerar (string palavra)
        {
            var resultado = new List<LetraCriptografada>();
            Dictionary<char, string> mapa = new();
            int indice = 0;

            foreach (char letra in palavra.ToUpper())
            {
                if (!mapa.ContainsKey(letra))
                {
                    mapa[letra] = CriptogramaSimbolos.Lista[indice];
                    indice++;
                }
                resultado.Add(new LetraCriptografada
                {
                    LetraOriginal = letra.ToString(),
                    Simbolo = mapa[letra]
                });
            }
            return resultado;           
        }
    }
}
