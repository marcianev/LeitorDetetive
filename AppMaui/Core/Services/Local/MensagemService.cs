using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local
{
    public class MensagemService(MensagemRepository _repository)
    {
        //metodo salvar mensagem
        public async Task<bool> SalvarMensagem(Mensagem mensagem)
        {
            try
            {
                //validações
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
                throw new Exception ($"Erro ao salvar mensagem: {ex.Message}");
            }
        }//fim método salvar mensagem

        //metodo listar mensagens
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
        }//fim método listar mensagens

        //metodo atualizar mensagem
        public async Task<bool> AtualizarMensagem(Mensagem mensagem)
        {
            try
            {
                //validações
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
                throw new Exception ($"Erro ao atualizar mensagem: {ex.Message}");
            }
        }//fim método atualizar mensagem

        //metodo deletar mensagem
        public async Task<bool> DeletarMensagem(int id)
        {
            try
            {
                //validação
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
                throw new Exception ($"Erro ao deletar mensagem: {ex.Message}");
            }
        }//fim método deletar mensagem
    }
}
