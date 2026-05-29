using AppMaui.Core.Models;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;


namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Serviço para gerenciar mensagens template com validações e população de dados iniciais.
    /// </summary>
    public class MensagemService(MensagemRepository _repository)
    {
        public async Task<bool> SalvarMensagem(Mensagem mensagem)
        {
            try
            {
                if (string.IsNullOrEmpty(mensagem.Conteudo) ||
                    mensagem.Conteudo.Length > 100 ||
                    Enum.IsDefined(mensagem.Tipo))
                    return false;                   
                string s = mensagem.Tipo.ToString();
                if (s.Length > 20 ||
                    mensagem.Titulo != null && mensagem.Titulo.Length > 100)
                    return false;

                await _repository.Add(mensagem);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar mensagem: {ex.Message}");
            }
        }

        public async Task<List<Mensagem>> ListarMensagens()
        {
            try
            {
                var mensagens = await _repository.GetAll();
                return mensagens;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar mensagens: {ex.Message}");
            }
        }

        public async Task<bool> AtualizarMensagem(Mensagem mensagem)
        {
            try
            {
                if (mensagem.Id <= 0 ||
                    string.IsNullOrEmpty(mensagem.Conteudo) ||
                    Enum.IsDefined(mensagem.Tipo) == false ||
                    mensagem.Titulo != null && mensagem.Titulo.Length > 100)
                    return false;

                await _repository.Update(mensagem);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar mensagem: {ex.Message}");
            }
        }

        public async Task<bool> DeletarMensagem(int id)
        {
            try
            {
                if (id <= 0)
                    return false;
                var mensagem = await _repository.GetById(id);
                if (mensagem == null)
                    return false;

                await _repository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar mensagem: {ex.Message}");
            }
        }

        /// <summary> Popula a tabela de mensagens com dados iniciais do arquivo JSON. </summary>
        
        public async Task PopularMensagens()
        {
            try
            {
                await _repository.PopularMensagens();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao popular mensagens: {ex.Message}");
            }
        }
    }
}
