using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;

namespace AppMaui.Core.Services.Logging
{
    /// <summary>
    /// Serviço para registrar eventos do sistema para auditoria e rastreamento.
    /// </summary>
    public class EventoService(EventoRepository _repository)
    {
        /// <summary>Salva um evento do sistema com validações de dados obrigatórios.</summary>
        public async Task<bool> SalvarEvento(EventoSistema evento)
        {
            try
            {
                if(evento == null)
                    return false;
                string s = evento.TipoEvento.ToString();
                if (s.Length > 10 ||
                    evento.Descricao.Length > 500 ||
                    evento.Tabela == string.Empty ||
                    evento.ReferenciaId <= 0 ||
                    evento.UsuarioId <= 0)
                    return false;

                evento.DataEvento = DateTime.Now;

                await _repository.Add(evento);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar log: {ex.Message}");
            }
        }

        /// <summary>Lista todos os eventos registrados no sistema.</summary>
        public async Task<List<EventoSistema>> ListarEvento()
        {
            try
            {
                var evento = await _repository.GetAll();
                return evento;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar logs: {ex.Message}");
            }
        }

        ///<summary>Lista todos os registros de um determinado evento em um determinado tempo</summary>
        public async Task<List<string>?> ListarEventoPorInatividade(Eventos evento,int idProfessor, int dias)
        {
            try
            {
                if (dias <= 0 || idProfessor <=0)
                    return null;
                return await _repository.BuscarAlunosInativos(evento, idProfessor, dias);
            }
            catch
            { 
                return null; 
            }
        }

        ///<summary>retorna o ultimo registro de um determinado evento para um determinado usuario</summary>
        public async Task<EventoSistema?> BuscarPorEvento(int usuarioId, Eventos evento)
        {
            try
            {
                if (usuarioId <= 0)
                    return null;

                return await _repository.GetByTipoUsuario(usuarioId, evento);
            }
            catch
            {
                return null;
            }
        }

        ///<summary>lista os alunos que subiram de nivel</summary>
        public async Task<List<string>?> BuscarPorNivelUp(int idProfessor, int dias)
        {
            if(dias <= 0)
                return null;

            return await _repository.BuscarAlunosNivelUp(idProfessor, dias);
        }
    }
}
