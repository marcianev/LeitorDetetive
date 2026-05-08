using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local
{
    public class TrilhaService(TrilhaRepository _repositorio)
    {
        //metodo salva
        public async Task<bool> SalvarTrilha(Trilha turma)
        {
            try
            {
                //validações
                if (string.IsNullOrEmpty(turma.Nome) ||
                    turma.Nome.Length > 100)
                    return false;

                await _repositorio.Add(turma);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao salvar trilha: {ex.Message}");
            }
        }//fim salvar

        //metodo listar
        public async Task<List<Trilha>> ListarTrilha()
        {
            try
            {
                var trilhas = await _repositorio.GetAll();
                return trilhas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar trilhas: {ex.Message}");
            }
        }//fim listar

        //metodo atualizar
        public async Task<bool> AtualizarTrilha(Trilha turma)
        {
            try
            {
                //validações
                if (turma.Id <= 0 ||
                    string.IsNullOrEmpty(turma.Nome) ||
                    turma.Nome.Length > 100)
                    return false;

                await _repositorio.Update(turma);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao atualizar trilha: {ex.Message}");
            }
        }//fim atualizar

        //metodo deletar
        public async Task<bool> DeletarTrilha(int id)
        {
            try
            {
                if (id <= 0)
                    return false;
                var trilha = await _repositorio.GetById(id);
                if(trilha == null)
                    return false;

                await _repositorio.Delete(trilha);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao deletar trilha: {ex.Message}");
            }
        }

    }
}
