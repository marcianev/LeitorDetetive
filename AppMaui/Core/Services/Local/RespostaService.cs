using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using System;
using System.Collections.Generic;
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
                //não é possível a palavra vir como lista de char pois é passado via modelo e já deve vir como 
                //string então terei de tratar pelo via dto
                //r.Palavra = TransformarPalavra(letras);
                resposta.UltimoRegistro = DateTime.Now;

                await _repostitorio.Add(resposta);
                return true;
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
