using AppMaui.Core.Enums;
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
    public class LeituraService(LeituraRepository _repositorio, AlunoService _aluno)
    {      
        //recebe o modelo e salva um aluno no banco de dados
        public async Task<bool> SalvarLeitura(Leitura leitura)
        {
            try
            {
                //validações
                if (leitura.UsuarioId <= 0 ||
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
                throw new Exception($"Erro ao salvar leitura: {ex.Message}");
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
                throw new Exception($"Erro ao atualizar leitura: {ex.Message}");
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
                throw new Exception($"Erro ao deletar leitura: {ex.Message}");
            }
        }//fim deletar

        //buscar leitura por idlivro e idusuario, para verificar se o usuário já iniciou a leitura de um livro
        public async Task<Leitura?> GetLeituraPorLivroUsuario(int livroId, int usuarioId)
        {
            try
            {
                //validações dos ids
                if (livroId <= 0 || usuarioId <= 0)
                    throw new Exception("IDs de livro e usuário são inválidos.");
                var leitura = await _repositorio.GetByLivroUsuario(livroId, usuarioId);
                return leitura;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar leitura: {ex.Message}");
            }
        }//fim método buscar leitura por livro e usuário

        //buscar leitura atual por usuário, para exibir na estante o livro que o usuário está lendo no momento
        public async Task<Leitura?> GetLeituraAtualPorUsuario(int usuarioId)
        {
            try
            {
                //validação do id do usuário
                if (usuarioId <= 0)
                    throw new Exception("ID do usuário é inválido.");
                var leitura = await _repositorio.GetLeituraAtualPorUsuario(usuarioId);
                return leitura;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar leitura atual: {ex.Message}");
            }
        }//fim método buscar leitura atual por usuário

        //buscar leitura anterior por usuário, para exibir na estante o livro que o usuário leu por último
        public async Task<Leitura?> GetLeituraAnteriorPorUsuario(int usuarioId)
        {
            try
            {
                //validação do id do usuário
                if (usuarioId <= 0)
                    throw new Exception("ID do usuário é inválido.");
                var leitura = await _repositorio.GetLeituraAnteriorPorUsuario(usuarioId);
                return leitura;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar leitura anterior: {ex.Message}");
            }
        }//fim método buscar leitura anterior por usuário

        //concluir leitura, para marcar a leitura como concluída e atribuir a data de término
        public async Task<bool> ConcluirLeitura(int usuarioId)
        {
            try
            {
                //validação do id do usuário
                if (usuarioId <= 0)
                    return false;
                var leitura = await _repositorio.GetLeituraAtualPorUsuario(usuarioId);
                if (leitura == null)
                    return false;
                leitura.DataFim = DateTime.Now;
                leitura.Status = StatusLeitura.Concluida;
                await _repositorio.Update(leitura);
                int leituras = await _repositorio.ContarLeiturasConcluidas(usuarioId);
                Debug.WriteLine($"Leituras concluídas: {leituras}");
                bool atualizarNivel = await _aluno.AtualizarPatente(usuarioId, leituras);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao concluir leitura: {ex.Message}");
            }
        }
    }
}
