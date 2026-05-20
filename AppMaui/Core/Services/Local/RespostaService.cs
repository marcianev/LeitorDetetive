using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local
{
    public class RespostaService(RespostaRepository _repostitorio)
    {
        //metodo salvar resposta
        public async Task<bool> SalvarResposta(Resposta resposta)
        {
            try
            {
                //validações
                if (resposta.AlunoId <= 0 ||
                    resposta.DesafioId <= 0)
                    return false;

                Debug.WriteLine($"{resposta.AlunoId}//{resposta.DesafioId}");
                //procurar resposta já existente para o mesmo aluno e desafio
                var respostaExistente = await _repostitorio.GetByAlunoDesafio(resposta.AlunoId, resposta.DesafioId);
                if(!respostaExistente.Any())
                {
                    resposta.UltimoRegistro = DateTime.Now;

                    await _repostitorio.Add(resposta);
                    return true;
                }
                else
                    return false;               
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao salvar resposta: {ex.Message}");
            }

        }//fim salvar

        //metodo listar
        public async Task<List<Resposta>> ListarRespostas()
        {
            try
            {
                var respostas = await _repostitorio.GetAll();
                return respostas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar respostas: {ex.Message}");
            }
        }

        //metodo atualizar
        public async Task<bool> AtualizarResposta(Resposta resposta)
        {
            try
            {
                //validações
                if (resposta.Id <= 0 ||
                    resposta.AlunoId <= 0 ||
                    resposta.DesafioId <= 0)
                    return false;

                //r.Palavra = TransformarPalavra(letras);
                resposta.UltimoRegistro = DateTime.Now;

                await _repostitorio.Update(resposta);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao atualizar resposta: {ex.Message}");
            }
        }//fim listar

        //aqui irá o listar por desafio

        //metodo deletar resposta
        public async Task<bool> DeletarResposta(int id)
        {
            try
            {
                //validação
                if (id <= 0)
                    return false;
                var resposta = await _repostitorio.GetById(id);
                if(resposta == null)
                    return false;

                await _repostitorio.Delete(resposta);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao deletar resposta: {ex.Message}");
            }
        }//fim deletar

        //listar respostas por livro e usuario, para exibir na estante
        public async Task<List<Resposta>> ListarAlunoDesafio(int livroId, int alunoId)
        {
            try
            {
                if (livroId <= 0 || alunoId <= 0)
                    throw new Exception("ID do livro ou aluno é inválido.");
                var respostas = await _repostitorio.GetByAlunoDesafio(alunoId, livroId);
                if(respostas == null)
                    return null;
                return respostas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar respostas por livro e usuário: {ex.Message}");
            }
        }
    }
}
