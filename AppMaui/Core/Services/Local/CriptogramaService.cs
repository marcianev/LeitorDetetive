using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local
{
    public class CriptogramaService
    {      
        public List<LetraCriptografada> Gerar (int desafio)
        {
           // var resultado = new List<LetraCriptografada>();
            List<LetraCriptografada> mapa = new();            
           

            for (int i = 0; i < CriptogramaSimbolos.Alfabeto.Length; i++)
            {
                if (desafio >= 40)
                    desafio = 0;
                else
                    desafio++;               
               
                mapa.Add(new LetraCriptografada
                {
                    LetraOriginal = CriptogramaSimbolos.Alfabeto[i],

                    Simbolo = CriptogramaSimbolos.Lista[desafio]
                });
            }

            return mapa;      
        }

        public string Criptografar(string palavra, List<LetraCriptografada>mapa)
        {
            var dicionario = mapa.ToDictionary(
                x => x.LetraOriginal,
                x => x.Simbolo
            );
            return string.Join("", palavra.ToUpper().Select(c => dicionario[c.ToString()]));
        }
    }
}
