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
    public class TurmaService(TurmaRepository _repositorio, 
        ProfessorService _professorService)
    {
        //metodo salvar
        public async Task<bool> SalvarTurma(Turma turma)
        {
            try
            {
                if (string.IsNullOrEmpty(turma.Nome) ||
                    turma.Nome.Length > 100 ||
                    turma.ProfessorId <= 0 ||
                    turma.TrilhaId <= 0)
                    return false;

                if (turma.TamanhoTrilha <= 0)
                    turma.TamanhoTrilha = 18;               
                turma.Status = true;
                turma.DataCriacao = DateTime.Now;
                turma.Ano = turma.DataCriacao.Year.ToString();

                await _repositorio.Add(turma);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception ($"Erro ao salvar turma: {ex.Message}");
            }
        }//fim salvar

        //metodo listar por usuario
        public async Task<List<Turma>> ListarTurmaPorUsuario(int idUsuario)
        {
            try
            {
                //validação
                if (idUsuario <= 0)
                    throw new Exception("id invalido");

                //busca o id do professor
                var professor = await _professorService.BuscarProfessorPorUsuario(idUsuario);

                var turmas = await _repositorio.GetByProfessor(professor.Id);
                return turmas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar turmas: {ex.Message}");
            }
        }

        //metodo atualizar
        public async Task<bool> AtualizarTurma(Turma turma)
        {
            try
            {
                if (turma.Id <= 0 || string.IsNullOrEmpty(turma.Nome) ||
                    turma.Nome.Length > 100 ||
                    turma.ProfessorId <= 0 ||
                    turma.TrilhaId <= 0 ||
                    turma.TamanhoTrilha <= 0 ||                 
                    turma.DataCriacao == DateTime.MinValue ||
                    turma.Status == null)
                {
                    Debug.WriteLine("Não validou");
                    return false;
                }     
                await _repositorio.Update(turma);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception ($"Erro ao atualizar turma: {ex.Message}");
            }
        }//fim atualizar

        //metodo deletar
        public async Task<bool> DeletarTurma(int id)
        {
            try
            {
                //validação
                if (id <=0)
                    return false;
                var turma = await _repositorio.GetById(id);
                if (turma == null)
                    return false;

                await _repositorio.Delete(turma);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao deletar turma: {ex.Message}");
            }
        }//fim deletar

       //METODO BUSCAR POR ID DO PROFESSOR
       public async Task<int> BuscarPorProfessor(int id)
        {
            try
            {
                //validação
                if (id <= 0)
                    return 0;

                var turma = await _repositorio.GetOneByProfessor(id);
                if (turma == null)
                    return 0;
                return turma.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar turma: {ex.Message}");
            }
        }
    }
}
