using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local
{
    public class NotificacaoService(NotificacaoRepository _repositorio)
    {
        //metodo salvar notificação
        public async Task<bool> SalvarNotificacao(Notificacao notificacao)
        {
            try
            {
                //validações
                string s = notificacao.Motivo.ToString();
                if (s.Length > 100 ||
                    notificacao.UsuarioId <= 0)
                    return false;

                //chamar metodo que buscará mensagem cadastrada no banco para o texto 
                notificacao.DataEmissao = DateTime.Now;
                notificacao.Status = false;

                await _repositorio.Add(notificacao);
                return true;
                //chamar o metodo para enviar a notificação para o usuario professor por email
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao salvar notificação: {ex.Message}");
            }
        }//fecha método salvar notificacao

        //metodo para listar notificações de um usuário
        public async Task<List<Notificacao>> ListarNotificacoesPorUsuario(int usuarioId)
        {
            try
            {
                //validação
                if (usuarioId <= 0)
                    throw new Exception("ID do usuário é inválido.");

                var notificacoes = await _repositorio.GetByUsuario(usuarioId);
                return notificacoes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar notificações: {ex.Message}");
            }
        }//fim listar

        //metodo para atualizar notificacao
        public async Task<bool> AtualizarNotificacao(Notificacao notificacao)
        {
            try
            {
                //validações
                if (notificacao.Id <= 0 ||
                    notificacao.Motivo.ToString().Length > 100 ||
                    string.IsNullOrEmpty(notificacao.Texto) ||
                    notificacao.Texto.Length > 255 ||
                    notificacao.UsuarioId <= 0 ||
                    notificacao.DataEmissao == DateTime.MinValue)
                    return false;
                bool? status = notificacao.Status;
                if (status == null)
                    return false;

                await _repositorio.Update(notificacao);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao atualizar notificação: {ex.Message}");
            }
        }//fim atualizar

        //metodo para deletar notificacao
        public async Task<bool> DeletarNotificacao(int id)
        {
            try
            {
                //validação
                if (id <= 0)
                    return false;
                var notificacao = await _repositorio.GetById(id);
                if(notificacao == null)
                    return false;

                await _repositorio.Delete(notificacao);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao deletar notificação: {ex.Message}");
            }
        }//fim deletar

        //metodo listar notificações para o tipo definido
        //sorteará um mensagem na lista e retornará o texto para ser enviado na notificação
    }
}
