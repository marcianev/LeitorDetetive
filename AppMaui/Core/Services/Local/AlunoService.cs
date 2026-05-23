
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
    public class AlunoService(AlunoRepository _repositorio)
    {
        //metodo salvar aluno
        public async Task<bool> SalvarAluno(Aluno aluno)
        {
            try
            {
                //validações
                if (string.IsNullOrEmpty(aluno.Nome) ||
                    aluno.Nome.Length > 100)
                    return false;

                await _repositorio.Add(aluno);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar aluno por ID: {ex.Message}");

            }
        }//fecha método salvar aluno

        //metodo para listar alunos
        public async Task<List<Aluno>> ListarAlunos(int turma)
        {
            try
            {
                //validação
                if (turma <= 0)
                    throw new Exception("Turma inválida.");
                var alunos = await _repositorio.GetByTurma(turma);

                return alunos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar alunos: {ex.Message}");
            }

        }//fim listar

        //metodo para atualizar aluno
        public async Task<bool> AtualizarAluno(Aluno aluno)
        {
            try
            {
                //validações
                if (string.IsNullOrEmpty(aluno.Nome) ||
                    aluno.Nome.Length > 100 ||
                    aluno.Id <= 0)
                    return false;

                await _repositorio.Update(aluno);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar aluno por ID: {ex.Message}");
            }
        }//fecha método atualizar aluno

        //metodo deletar aluno
        public async Task<bool> DeletarAluno(int id)
        {
            try
            {
                //pesquisa se aluno existe
                var aluno = await _repositorio.GetById(id);
                if (aluno == null)
                    return false;

                await _repositorio.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar aluno: {ex.Message}");
            }
        }//fecha método deletar aluno

        //buscar aluno por usuario e retorna o codigo de acesso
        public async Task<string> BuscarAlunoPorUsuario(int usuarioId)
        {
            try
            {
                //validação
                if (usuarioId <= 0)
                    return "";
                var aluno = await _repositorio.GetByUsuarioId(usuarioId);
                if (aluno == null)
                    return "";
                return aluno.CodigoAcesso;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar aluno por ID: {ex.Message}");
            }
        }//fecha método buscar aluno por usuario

        //buscar aluno por usuario e retorna o aluno
        public async Task<Aluno> BuscarAlunoUsuario(int usuarioId)
        {
            try
            {
                //validação
                if (usuarioId <= 0)
                    return null;
                var aluno = await _repositorio.GetByUsuarioId(usuarioId);
                if (aluno == null)
                    return null;
                return aluno;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar aluno por ID: {ex.Message}");
            }
        }//fecha método buscar aluno por usuario

        //metodo verifica se nickname existe
        public async Task<bool> VerificarExistenciaNick(string nick)
        {
            try
            {
                //validação
                if (!string.IsNullOrEmpty(nick))
                    return false;
                return await _repositorio.NicknameExist(nick);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar NickName: {ex.Message}");
            }

        }//fecha verificarExistenciaNick

        //verifica a quantidade de leitura e atualiza patente se necessário
        public async Task<bool> AtualizarPatente(int usuarioId, int quantidadeLeituras)
        {
            try
            {
                //validação
                if (usuarioId <= 0)
                    return false;
                var aluno = await _repositorio.GetByUsuarioId(usuarioId);
                if (aluno == null)
                    return false;
                //verifica a quantidade de leituras e atualiza a patente
                int patente = quantidadeLeituras / 3;
                Debug.WriteLine($"Patente: {patente}");
                //pesquisa a id da possivel nova patente
                if (quantidadeLeituras % 3 == 0)
                {
                    aluno.PatenteId = await _repositorio.GetIdPatente(patente + 1, aluno.PatenteId);
                    Debug.WriteLine($"NovaPatente: {aluno.PatenteId}");
                    //await _repositorio.Update(aluno);
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar patente: {ex.Message}");
            }
        }//atualizar patente
    }
}