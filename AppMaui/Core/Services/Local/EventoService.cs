using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;
using AppMaui.Core.Services.Api.Interface;
using AppMaui.Services;
using Shared.DTOs.Responses;
using System.Diagnostics;

namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Serviço para registrar eventos do sistema para auditoria e rastreamento.
    /// </summary>
    public class EventoService(EventoRepository _repository, ConectividadeService _conectaddo, 
        IEventoApiService eventoApiService)
    {
        /// <summary>Salva um evento do sistema com validações de dados obrigatórios.</summary>
        public async Task<OperacaoResponse> SalvarEvento(EventoSistema evento)
        {
            try
            {
                if (evento == null)
                    return new OperacaoResponse { Sucesso = false, Mensagem = "Evento não pode ser nulo." };

                string s = evento.TipoEvento.ToString();
                if (string.IsNullOrWhiteSpace(s) || 
                    s.Length > 10 ||
                    evento.Descricao.Length > 500 ||
                     string.IsNullOrWhiteSpace(evento.Tabela) ||                    
                    evento.UsuarioId <= 0)
                    return new OperacaoResponse { Sucesso = false, Mensagem = "Dados do evento inválidos." };

                evento.DataEvento = DateTime.Now;

               

                if (_conectaddo.TemInternet())
                {
                    Debug.WriteLine("Conexão disponível. Sincronizando evento com a API...");
                    evento.Sincronizado = true;
                    evento.DataSincronizado = DateTime.Now;

                    var eventoApi = await eventoApiService.SalvarEvento(evento);
                   
                    if (eventoApi == null)
                    {                        
                        evento.Sincronizado = false;
                        evento.DataSincronizado = null;
                    }
                }
                
                await _repository.Add(evento);
                return new OperacaoResponse { Sucesso = true, Mensagem = "Evento salvo com sucesso." };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
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
