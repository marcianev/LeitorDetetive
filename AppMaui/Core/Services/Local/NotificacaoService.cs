using AppMaui.Core.Models;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;

namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Serviço para gerenciar notificações de usuários com validações e persistência.
    /// </summary>
    public class NotificacaoService(NotificacaoRepository _repositorio)
    {
        public async Task<bool> SalvarNotificacao(Notificacao notificacao)
        {
            try
            {
                string s = notificacao.Motivo.ToString();
                if (s.Length > 100 ||
                    notificacao.UsuarioId <= 0)
                    return false;

                notificacao.DataEmissao = DateTime.Now;
                notificacao.Status = false;

                await _repositorio.Add(notificacao);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar notificação: {ex.Message}");
            }
        }

        public async Task<List<Notificacao>> ListarNotificacoesPorUsuario(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new Exception("ID do usuário é inválido.");

                var notificacoes = await _repositorio.GetByUsuario(usuarioId);
                return notificacoes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar notificações: {ex.Message}");
            }
        }

        public async Task<bool> AtualizarNotificacao(Notificacao notificacao)
        {
            try
            {
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
                throw new Exception($"Erro ao atualizar notificação: {ex.Message}");
            }
        }

        public async Task<bool> DeletarNotificacao(int id)
        {
            try
            {
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
                throw new Exception($"Erro ao deletar notificação: {ex.Message}");
            }
        }
    }
}
