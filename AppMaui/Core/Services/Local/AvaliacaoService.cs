
using AppMaui.Core.DTOs;
using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using AppMaui.Core.Services.External;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AppMaui.Core.Services.Local
{
    public class AvaliacaoService(AvaliacaoRepository _repositorio, 
        OpenAIService openAIService)   
    {          
        //recebe o modelo de avaliação, salva uma avaliação no banco de dados

        public async Task<bool> SalvarAvaliacao(AvaliacaoDTO dto)
        {
            try
            {
                //validações
                if (dto.Nota < 1 
                    || dto.Nota > 5 ||
                    string.IsNullOrEmpty(dto.Comentario) ||
                    dto.Comentario.Length > 300)
                    return false;               
                   

                var status = await openAIService.ValidarComentario(dto.Comentario);
               
                var avaliacao = new Avaliacao()
                {
                    Nota = dto.Nota,
                    Comentario = dto.Comentario,
                    Status = status,
                    UsuarioId = dto.UsuarioId,
                    LivroId = dto.LivroId,
                    DataCadastro = DateTime.Now
                };
                Debug.WriteLine($"Cheguei aqui {status}");

                await Task.Delay(2000);
                var salvo = await _repositorio.Add(avaliacao);
                if (salvo <= 0)
                {

                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao salvar avaliação: {ex.Message}");
            }
        }//fecha método salvar avaliação

        //metodo para listar avaliações
        public async Task<List<Avaliacao>> ListarAvaliacoes(int turmaId)
        {
            try
            {
                //validação do id da turma
                if (turmaId <= 0)
                    throw new Exception("ID do aluno é inválido.");

                var avaliacoes = await _repositorio.GetByTurma(turmaId);
                return avaliacoes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar avaliações: {ex.Message}");
            }
        }//fim listar

        //metodo para atualizar avaliação
        public async Task<bool> AtualizarAvaliacao(Avaliacao avaliaca)
        {
            try
            {
                //validações
                if (avaliaca.Id <= 0 ||
                    avaliaca.Nota < 1 || 
                    avaliaca.Nota > 5 ||
                    string.IsNullOrEmpty(avaliaca.Comentario) ||
                    avaliaca.Comentario.Length > 100)
                    return false;
                string s = avaliaca.Status.ToString();
                if (s.Length > 20)
                    return false;

                await _repositorio.Update(avaliaca);
                return true;
                //chamar o metodo de validação automática aqui
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao atualizar avaliação: {ex.Message}");
            }
        }//fecha método atualizar avaliação

        //metodo para deletar avaliação
        public async Task<bool> DeletarAvaliacao(int id)
        {
            try
            {
                //validação do id da avaliação
                if (id <= 0)
                    return false;
                var avaliacao = await _repositorio.GetById(id);
                if (avaliacao == null)
                    return false;

                await _repositorio.Delete(avaliacao);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao deletar avaliação: {ex.Message}");
            }
        }//fecha método deletar avaliação      

        //listar avaliações por livros
        public async Task<List<AvaliacaoDTO>> ListarPorLivro(int livroId)
        {
            try
            {
                //validar id
                if (livroId <= 0)
                    return null;

                return await _repositorio.GetByLivro(livroId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro listar comentários: {ex.Message}");
            }
            
            

        }
    }
}
