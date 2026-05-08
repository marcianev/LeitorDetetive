
using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local
{
    public class AlunoService(AlunoRepository _repositorio)
    {        
        //metodo salvar aluno
        public async Task<bool> SalvarAluno(Aluno aluno, String professor)
        {
            try
            {
                //validações
                if (string.IsNullOrEmpty(aluno.Nome) || 
                    aluno.Nome.Length > 100)
                    return false;

                aluno.Nickname = await GerarNickname(aluno, professor);

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

                await _repositorio.Delete(aluno);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar aluno: {ex.Message}");
            }
        }//fecha método deletar aluno

        //buscar aluno por usuario
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

        //metodo para gerar nickname do aluno
        public async Task<string> GerarNickname(Aluno aluno, string professor)
        {
            //validação
            if (string.IsNullOrWhiteSpace(aluno.Nome))
                return string.Empty;

            //transforma o nome do aluno e do professor em arrays de caracteres para pegar as iniciais
            char[] n = aluno.Nome.ToUpper().ToCharArray();
            char[] p = professor.ToUpper().ToCharArray();

            //variaveis
            Random rand = new();
            int numm;
            int i = 0;
            string nickname;
            bool nicknameExists;

            //por enquanto funciona, para até 90 repetições
            //preciso pensar em algoritmo para ampliar
            //gerar nickname com as iniciais do nome do aluno, do professor e um número aleatório de 2 dígitos
            do
            {
                numm = rand.Next(10, 99);
                nickname = $"{n[0]}{p[0]}{numm}";
                nicknameExists = await _repositorio.NicknameExist(nickname);
                i++;
            } while (nicknameExists && i < 90);
            return nickname;
        }//fecha método gerar nickname       

    }//fecha classe AlunoServico
}
