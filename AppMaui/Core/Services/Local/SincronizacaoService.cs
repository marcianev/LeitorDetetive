using AppMaui.Core.Services.Api;
using AppMaui.Core.Services.Api.Interface;
using AppMaui.Core.Services.Local.Interfaces;
using AppMaui.Services;
using Shared.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local
{
    public class SincronizacaoService : ISincronizacaoService
    {
        private readonly ConectividadeService _conectividadeService;
        private readonly EventoService _eventoService;
        private readonly IEventoApiService _eventoApiService;

        public SincronizacaoService(ConectividadeService conectividadeService, 
            EventoService eventoService, IEventoApiService eventoApiService)
        {
            _conectividadeService = conectividadeService;
            _eventoService = eventoService;
            _eventoApiService = eventoApiService;
        }

        public async Task<OperacaoResponse> SincronizarTudo()
        {
            if (!_conectividadeService.TemInternet())
                return new OperacaoResponse { Sucesso = false, Mensagem = "Sem conexão com a internet." };
            return await SincronizarEventos();
        }

        public async Task<OperacaoResponse> SincronizarEventos()
        {           
            var eventosNaoSincronizados = await _eventoService.ListarEventosNaoSincronizados();
            if(eventosNaoSincronizados.Count == 0)
            {
                return new OperacaoResponse { Sucesso = true, Mensagem = "Nenhum evento para sincronizar." };
            }
            
            foreach (var sincronizar in eventosNaoSincronizados)
            {
                var eventoApi = await _eventoApiService.SalvarEvento(sincronizar);
                if (eventoApi?.Sucesso == true)
                {
                    sincronizar.Sincronizado = true;
                    sincronizar.DataSincronizado = DateTime.Now;
                    bool atualizado = await _eventoService.AtualizarEvento(sincronizar);
                    if(!atualizado) 
                        return new OperacaoResponse { Sucesso = false, Mensagem = $"Evento sincronizado, mas falha ao atualizar status local." };
                }
            }            
            return new OperacaoResponse { Sucesso = true, Mensagem = "Todos os eventos sincronizados com sucesso." };
        }
    }
}
