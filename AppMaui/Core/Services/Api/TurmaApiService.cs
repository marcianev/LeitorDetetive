using AppMaui.Core.Services.Api.Interface;
using Shared.DTOs.Requests;
using Shared.DTOs.Responses;
using System.Diagnostics;
using System.Net.Http.Json;

namespace AppMaui.Core.Services.Api
{
    public class TurmaApiService(HttpClient httpClient) : ITurmaApiService
    {    
        public async Task<OperacaoResponse<TurmaResponse?>> Add(TurmaRequest request)
        {            
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/turma/salvar-turma", request);
                var conteudo = await response.Content.ReadAsStringAsync();

                Debug.WriteLine(conteudo);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"cheguei, {response.StatusCode}");
                    return new OperacaoResponse<TurmaResponse?>
                    {
                        Sucesso = false,
                        Mensagem = $"Erro HTTP: {(int)response.StatusCode}"
                    };                    
                }
 

                 var resultado = await response.Content.ReadFromJsonAsync<OperacaoResponse<TurmaResponse?>>();
                return resultado ?? new OperacaoResponse<TurmaResponse?>();
            }
            catch (Exception ex)
            {
                return new OperacaoResponse<TurmaResponse?>
                {
                    Sucesso = false,
                    Mensagem = $"Ocorreu um erro ao adicionar a turma: {ex.Message}"
                };
            }
           
        }

        public async Task<OperacaoResponse<List<TurmaResponse>>> GetAll()
        {
            try
            {
                var response = await httpClient.GetAsync("api/turma/get-all");

                if (!response.IsSuccessStatusCode)
                {
                    return new OperacaoResponse<List<TurmaResponse>>
                    {
                        Sucesso = false,
                        Mensagem = $"Erro HTTP: {(int)response.StatusCode}"
                    };
                }

                var resultado = await response.Content.ReadFromJsonAsync<OperacaoResponse<List<TurmaResponse>>>();
                return resultado ?? new OperacaoResponse<List<TurmaResponse>>();
            }
            catch (Exception ex)
            {
                return new OperacaoResponse<List<TurmaResponse>>
                {
                    Sucesso = false,
                    Mensagem = $"Ocorreu um erro ao obter as turmas: {ex.Message}"
                };
            }
        }

        public async Task<OperacaoResponse> Update(TurmaRequest request)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"api/turma/update-turma", request);

                if (!response.IsSuccessStatusCode)
                {
                    return new OperacaoResponse
                    {
                        Sucesso = false,
                        Mensagem = $"Erro HTTP: {(int)response.StatusCode}"
                    };
                }

                var resultado = await response.Content.ReadFromJsonAsync<OperacaoResponse>();
                return resultado ?? new OperacaoResponse();
            }
            catch (Exception ex)
            {
                return new OperacaoResponse
                {
                    Sucesso = false,
                    Mensagem = $"Ocorreu um erro ao atualizar a turma: {ex.Message}"
                };
            }
        }

        public async Task<OperacaoResponse> Delete(string uuid)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/turma/{uuid}");

                if (!response.IsSuccessStatusCode)
                {
                    return new OperacaoResponse
                    {
                        Sucesso = false,
                        Mensagem = $"Erro HTTP: {(int)response.StatusCode}"
                    };
                }

                var resultado = await response.Content.ReadFromJsonAsync<OperacaoResponse>();
                return resultado ?? new OperacaoResponse();
            }
            catch (Exception ex)
            {
                return new OperacaoResponse
                {
                    Sucesso = false,
                    Mensagem = $"Ocorreu um erro ao deletar a turma: {ex.Message}"
                };
            }                
        }

        public async Task<OperacaoResponse<List<TurmaResponse>>> GetByProfessor(int professorId)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/turma/professor/{professorId}"); 
                if (!response.IsSuccessStatusCode)
                {
                    return new OperacaoResponse<List<TurmaResponse>>
                    {
                        Sucesso = false,
                        Mensagem = $"Erro HTTP: {(int)response.StatusCode}"
                    };
                }

                var resultado = await response.Content.ReadFromJsonAsync<OperacaoResponse<List<TurmaResponse>>>();
                return resultado ?? new OperacaoResponse<List<TurmaResponse>>();
            }
           catch(Exception ex)
            {
                return new OperacaoResponse<List<TurmaResponse>>
                {
                    Sucesso = false,
                    Mensagem = $"Ocorreu um erro ao obter as turmas do professor: {ex.Message}"
                };
            }
        }

        public async Task<OperacaoResponse<TurmaResponse?>> GetByUuid(string uuid)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/turma/{uuid}");

                if (!response.IsSuccessStatusCode)
                {
                    return new OperacaoResponse<TurmaResponse?>
                    {
                        Sucesso = false,
                        Mensagem = $"Erro HTTP: {(int)response.StatusCode}"
                    };
                }

                var resultado = await response.Content.ReadFromJsonAsync<OperacaoResponse<TurmaResponse?>>();
                return resultado ?? new OperacaoResponse<TurmaResponse?>();
            }
            catch (Exception ex)
            {
                return new OperacaoResponse<TurmaResponse?>
                {
                    Sucesso = false,
                    Mensagem = $"Ocorreu um erro ao obter a turma: {ex.Message}"
                };
            }
        }

        public async Task<OperacaoResponse<TurmaResponse?>> GetOneByProfessor(int professorId)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/turma/um-professor/{professorId}");
                if (!response.IsSuccessStatusCode)
                {
                    return new OperacaoResponse<TurmaResponse?>
                    {
                        Sucesso = false,
                        Mensagem = $"Erro HTTP: {(int)response.StatusCode}"
                    };
                }

                var resultado = await response.Content.ReadFromJsonAsync<OperacaoResponse<TurmaResponse?>>();
                return resultado ?? new OperacaoResponse<TurmaResponse?>();
            }
            catch (Exception ex)
            {
                return new OperacaoResponse<TurmaResponse?>
                {
                    Sucesso = false,
                    Mensagem = $"Ocorreu um erro ao obter a turma do professor: {ex.Message}"
                };

            }
        }
    }
}
