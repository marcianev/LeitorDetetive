using AppMaui.Core.DTOs;
using AppMaui.Core.Models;

namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Gerencia a geração e aplicação de cifras de substituição para desafios de criptografia.
    /// </summary>
    public class CriptogramaService
    {      
        /// <summary>
        /// Gera mapa de criptografia mapeando cada letra a um símbolo.
        /// TODO: Quando banco estender, modificar para cálculo que divida o id do livro pela quantidade de símbolos.
        /// </summary>
        public static List<LetraCriptografada> Gerar(int desafio)
        {
            List<LetraCriptografada> mapa = [];            

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

        /// <summary>Criptografa uma palavra usando o mapa gerado para o desafio.</summary>
        public static async Task<string> Criptografar(string palavra, int livro)
        {
            var mapa = Gerar(livro);

            var dicionario = mapa.ToDictionary(
                x => x.LetraOriginal,
                x => x.Simbolo
            );
            return string.Join("", palavra.ToUpper().Select(c => dicionario[c.ToString()]));
        }
    }
}
