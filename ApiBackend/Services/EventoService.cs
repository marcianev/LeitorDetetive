using ApiBackend.Models;
using ApiBackend.Repostiories;
using ApiBackend.Services.Interface;
using Shared.DTOs.Requests;
using Shared.DTOs.Responses;
using System.Diagnostics;

namespace ApiBackend.Services
{
    public class EventoService : IEventoService
    {
        private readonly EventoRepository _repository;

        public EventoService(EventoRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperacaoResponse> SalvarEvento(EventoRequest request)
        {
            try
            {
                if(request == null)
                    return OperacaoResponse.Resposta(false, "Requisição inválida");

                var eventoExistente = await _repository.GetByIdentificador(request.Identificador);
                if(eventoExistente != null)
                    return OperacaoResponse.Resposta(false, "Evento já existe");

                var evento = new Evento
                {
                    Tabela = request.Tabela,
                    TipoEvento = request.TipoEvento,
                    Descricao = request.Descricao,
                    DataEvento = request.DataEvento,
                    ReferenciaId = request.ReferenciaId,
                    UsuarioId = request.UsuarioId,
                    Identificador = request.Identificador
                };

                var result = await _repository.Add(evento);
                if (result == null)
                    return OperacaoResponse.Resposta(false, "Erro ao salvar o evento");

                return OperacaoResponse.Resposta(true, "Evento salvo com sucesso");
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.ToString());

                return OperacaoResponse.Resposta(
                    false,
                    ex.ToString());
            }

        }       
    }          
}

