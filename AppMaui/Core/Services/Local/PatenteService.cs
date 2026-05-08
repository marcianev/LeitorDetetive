using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local
{
    public class PatenteService(PatenteRepository _repositorio)
    {
        //metodo salvar patente
        public async Task<bool> SalvarPatente(Patente patente)
        {
            try
            {
                //validações
                if (string.IsNullOrEmpty(patente.Nome) ||
                    patente.Nome.Length > 100)
                    return false;

                await _repositorio.Add(patente);
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao salvar o aluno: {ex.Message}");
            }
        }//fim salvar

        //metodo listar patente
        public async Task<List<Patente>> ListarPatentesPorTrilha(int id)
        {
            try
            {
                if (id == 0)
                    throw new Exception("Trilha inválida.");
                var patentes = await _repositorio.GetByTrilha(id);
                return patentes;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao listar patentes: {e.Message}");
            }
        }//fim listar

        //metodo atualizar patente
        public async Task<bool> AtualizarPatente(Patente patente)
        {
            try
            {
                if (string.IsNullOrEmpty(patente.Nome) ||
                    patente.Nome.Length > 100 ||
                    patente.Nivel <= 0 ||
                    patente.Nivel < 0 ||
                    patente.TrilhaId == 0)
                    return false;                

                await _repositorio.Update(patente);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception ($"Erro ao atualizar a patente: {e.Message}");
            }
        }//fim atualizar

        //metodo deletar patente
        public async Task<bool> DeletarPatente(int id)
        {
            try
            {
                if (id <= 0)
                    return false;
                var patente = await _repositorio.GetById(id);
                if(patente == null)
                    return false;

                await _repositorio.Delete(patente);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception ($"Erro ao deletar patente: {e.Message}");
            }
        }//fim deletar

    }
}
