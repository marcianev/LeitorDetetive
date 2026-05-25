
namespace AppMaui.Core.DTOs
{
    /// <summary>
    /// DTO utilizado para consolidar e transportar os dados
    /// exibidos no dashboard do perfil Professor
    /// </summary>
    public class DashBoardPDTO
    {
        //dados vinculados a entidade aluno
        public string Aluno { get; set; } = string.Empty;
        public string Patente { get; set; } = string.Empty;

        //dados vinculados a entidade leitura
        public int QuantidadeLeituras { get; set; }

        //TODO: dados vinculados a entidade log
        public int NivelUp { get; set; }
        public int Leituras015 { get; set; }
    }
}
