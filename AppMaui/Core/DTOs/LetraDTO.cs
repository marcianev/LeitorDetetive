using CommunityToolkit.Mvvm.ComponentModel;

namespace AppMaui.Core.DTOs
{
    /// <summary>
    /// DTO consolida os dado para carregar letras e simbolos do criptograma
    /// e validar as respostas digitas
    /// </summary>
    public partial class LetraDTO : ObservableObject
    {       
        //receber letra digitada e validar com letra da resposta correta
        [ObservableProperty]
        private string letraDigitada = string.Empty;
        [ObservableProperty]
        private bool correta;

        //dados recebidos de desafio
        public int IdDesafio;
        public string LetraOriginal { get; set; } = string.Empty;

        //cripto vinculado a letra original
        public string LetraCriptograma { get; set; } = string.Empty;  
        
        //identifca a a posição da letra que será usada na palavra secreta
        public int Posicao { get; set; }

        //destaca a area da letra definida na posição
        public bool Destacada { get; set; }

        //corrigi automaticamente a digitação
        partial void OnLetraDigitadaChanged(string value)
        {
            Correta = value.ToUpper() == LetraOriginal.ToUpper();
        }
    }
}
