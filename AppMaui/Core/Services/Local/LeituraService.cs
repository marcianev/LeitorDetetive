using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local
{
    public class LeituraService(LeituraRepository _repositorio)
    {
        //recebe o modelo e salva um aluno no banco de dados
        public async Task<bool> SalvarLeitura(Leitura leitura)
        {
            try
            {
                //validações
                if (leitura.UsuarioId <= 0 ||
                    leitura.DataInicio == default ||
                    leitura.LivroId <= 0)
                    return false;

                //atribuindo a data de início da leitura como a data atual e o status como iniciada
                leitura.DataInicio = DateTime.Now;
                leitura.Status = StatusLeitura.Iniciada;

                await _repositorio.Add(leitura);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao salvar leitura: {ex.Message}");
            }
        }//fecha método salvar leitura

        //metodo para listar leituras
        public async Task<List<Leitura>> ListarLeituras(int usuarioId)
        {
            try
            {
                //validação do id do usuário
                if (usuarioId <= 0)
                    throw new Exception("ID do usuário é inválido.");

                var leituras = await _repositorio.GetByUsuario(usuarioId);
                return leituras;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar leituras: {ex.Message}");
            }
        }//fim listar

        //metodo para atualizar leitura
        public async Task<bool> AtualizarLeitura(Leitura leitura)
        {
            try
            {
                //validações
                if (leitura.Id <= 0 ||
                    leitura.UsuarioId <= 0 ||
                    leitura.DataInicio == default ||
                    leitura.LivroId <= 0)
                    return false;
                string s = leitura.Status.ToString();
                if (s.Length > 20)
                    return false;

                await _repositorio.Update(leitura);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao atualizar leitura: {ex.Message}");
            }
        }//fim atualizar

        //metodo para deletar leitura
        public async Task<bool> DeletarLeitura(int leituraId)
        {
            try
            {
                //validação do id da leitura
                if (leituraId <= 0)
                    return false;
                var leitura = await _repositorio.GetById(leituraId);
                if (leitura == null)
                    return false;

                await _repositorio.Delete(leitura);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao deletar leitura: {ex.Message}");
            }
        }//fim deletar
    }
}
