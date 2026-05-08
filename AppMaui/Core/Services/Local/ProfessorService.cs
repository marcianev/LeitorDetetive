using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local
{
    public class ProfessorService(ProfessorRespository _repositorio)
    {
        //metodo salvar professor
        public async Task<bool> SalvarProfessor(Professor professor)
        {
            try
            {
                //validações
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
                throw new Exception ($"Erro ao cadastrar professor: {ex.Message}");
            }
        }//fim salvar

        //metodo listar
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
        }//fim listar

        //metodo atualizar
        public async Task<bool> AtualizarProfessor(Professor professor)
        {
            try
            {
                //validações
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
                throw new Exception ($"Erro ao atualizar professor: {ex.Message}");
            }
        }//fim atualizar

        //metodo deletar 
        public async Task<bool> DeletarProfessor(int id)
        {           
            try
            {
                //validação
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
                throw new Exception ($"Erro ao deletar professor: {ex.Message}");
            }
        }//fim deletar


    }//fim classe
}

