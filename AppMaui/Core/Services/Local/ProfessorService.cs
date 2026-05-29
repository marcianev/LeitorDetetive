using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Serviço para gerenciar operações de professores com validações e persistência.
    /// </summary>
    public class ProfessorService(ProfessorRespository _repositorio)
    {
        public async Task<bool> SalvarProfessor(Professor professor)
        {
            try
            {
                if (string.IsNullOrEmpty(professor.Nome) ||
                    professor.Nome.Length > 100 ||
                    string.IsNullOrEmpty(professor.Email) ||
                    professor.Email.Length > 50 ||
                    string.IsNullOrEmpty(professor.Cpf) ||
                    professor.Cpf.Length < 11 && professor.Cpf.Length > 15 ||
                    professor.UsuarioId <= 0)
                    return false;

                await _repositorio.Add(professor);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cadastrar professor: {ex.Message}");
            }
        }

        public async Task<List<Professor>> ListarProfessor()
        {
            try
            {
                var professores = await _repositorio.GetAll();
                return professores;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar professor: {ex.Message}");
            }
        }

        public async Task<bool> AtualizarProfessor(Professor professor)
        {
            try
            {
                if (string.IsNullOrEmpty(professor.Nome) ||
                    professor.Nome.Length > 100 ||
                    string.IsNullOrEmpty(professor.Email) ||
                    professor.Email.Length > 50 ||
                    string.IsNullOrEmpty(professor.Cpf) ||
                    professor.Cpf.Length < 11 && professor.Cpf.Length > 15 ||
                    professor.UsuarioId <= 0 ||
                    professor.Id <= 0)
                    return false;

                await _repositorio.Update(professor);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar professor: {ex.Message}");
            }
        }

        public async Task<bool> DeletarProfessor(int id)
        {           
            try
            {
                if (id <= 0)
                    return false;
                var professor = await _repositorio.GetById(id);
                if(professor == null)
                    return false;

                await _repositorio.Delete(professor);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar professor: {ex.Message}");
            }
        }

        public async Task<Professor?> BuscarProfessorPorUsuario(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    return null;
                var professor = await _repositorio.GetByUsuarioId(usuarioId);
                if (professor == null)
                    return null;
                return professor;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar professor por ID: {ex.Message}");
            }
        }

        ///<summary>busca professor através do iddaTurma</summary>
        public async Task<Professor?> BuscarPorTurma(int turmaId)
        {
            try
            {
                if (turmaId <= 0)
                    return null;

                return await _repositorio.GetByTurma(turmaId);
            }
            catch
            {
                return null;
            }
        }

        ///<sumary>verifica a existência do email no banco.</sumary>
        public async Task<bool> EmailExiste(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                return await _repositorio.EmailExiste(email);
            }
            catch(Exception ex)
            {
                throw new Exception($"Erro ao buscar email: {ex.Message}");
            }
        }

        ///<sumary>verifica a existência do cpf no banco.</sumary>
        public async Task<bool> CPFExiste(string cpf)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cpf))
                    return false;

                return await _repositorio.CPFExiste(cpf);
            }
            catch(Exception ex)
            {
                throw new Exception($"Erro ao buscar email: {ex.Message}");
            }
        }
    }
}

