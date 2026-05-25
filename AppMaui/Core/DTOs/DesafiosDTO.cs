using System.Collections.ObjectModel;

namespace AppMaui.Core.DTOs
{
    /// <summary>
    /// DTO utilizado para consolidar e transportar os dados
    /// exibidos na pagina de desafio    
    /// </summary>
    public class DesafiosDTO
    {
        //dados vinculados a entidade desafio
        public int IdDesafio { get; set; }
        public int IdLivro { get; set; }
        public string Pergunta { get; set; } = string.Empty;
        public string Palavra { get; set; } = string.Empty;

        //dados vinculados a entidade resposta
        public int IdAluno { get; set; }
        public int IdResposta { get; set; } 
        public string RespostaCorreta { get; set; } = string.Empty;           
      
        /// <summary>
        /// cria ambiente para carregar letras e simbolos do criptograma
        /// e validar as respostas digitas
        /// </summary>
        public ObservableCollection<LetraDTO> Letras { get; set; } = [];
    }
}
