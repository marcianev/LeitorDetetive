using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local
{
    public class DesafioService(DesafioRepository _repository)
    {
        //metodo salvar no banco
        public async Task<bool> SalvarDesafio(Desafio desafio)
        {
            try
            {
                //validações
                if (desafio.Pergunta == null || 
                    desafio.Resposta == null ||
                    desafio.Pergunta.Length > 100 ||
                    desafio.Resposta.Length > 20)
                    return false;

                await _repository.Add(desafio);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao salvar desafio: {ex.Message}");
            }

        }//final salvar

        //metodo listar desafio
        public async Task<List<Desafio>> ListarDesafiosPorLivro(int livro)
        {
            try
            {
                //validação
                if (livro <= 0)
                    throw new Exception("Livro inexistente.");

                var desafios = await _repository.GetByLivro(livro);
                return desafios;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar desafios: {ex.Message}");
            }
        }//fim listar

        //metodo atualizar
        public async Task<bool> AtualizarDesafio(Desafio desafio)
        {
            try
            {
                //validações
                if (desafio.Id <= 0 ||
                    string.IsNullOrEmpty(desafio.Resposta) || 
                    string.IsNullOrEmpty(desafio.Pergunta) ||
                    desafio.Pergunta.Length > 100 ||
                    desafio.Resposta.Length > 20)
                    return false;

                await _repository.Update(desafio);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao atualizar desafio: {ex.Message}");
            }
        }//fecha atualizar

        //metodo deletar desafio
        public async Task<bool> DeletarDesafio(int id)
        {
            try
            {
                //validação
                if (id <= 0)
                    return false;
                var desafio = await _repository.GetById(id);
                if(desafio == null)
                    return false;

                await _repository.Delete(desafio);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao deletar desafio: {ex.Message}");
            }
        }
    }
}
