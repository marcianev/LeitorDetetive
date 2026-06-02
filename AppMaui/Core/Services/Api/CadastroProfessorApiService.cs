using AppMaui.Core.Data;
using AppMaui.Core.Services.Api.Interface;
using AppMaui.Core.Services.External;
using AppMaui.Core.Services.Local;
using AppMaui.Services.Interfaces;
using Shared.DTOs.Requests;
using Shared.DTOs.Responses;
using System.Diagnostics;
using System.Net.Http.Json;

namespace AppMaui.Core.Services.Api
{
    /// <summary>
    /// Orquestra o cadastro e atualização de professores, gerando credenciais e enviando email de boas-vindas.
    /// </summary>
    public class CadastrarProfessorApiService : ICadastroProfessorApiService
    {
        private readonly ProfessorService _professorService;
        private readonly EmailService _emailService;
        private readonly DatabaseService _databaseService;
        private readonly ICriptoService _criptoService;
        private readonly HttpClient _httpClient;

        public CadastrarProfessorApiService(ProfessorService professorService, 
            ICriptoService criptoService, DatabaseService databaseService, EmailService es,
            HttpClient httpClient)
        {
            _professorService = professorService;
            _emailService = es;
            _databaseService = databaseService;
            _criptoService = criptoService;
            _httpClient = httpClient;
        }

        /// <summary>Cadastra novo professor ou atualiza dados de um existente. Envia email com senha provisória.</summary>
        public async Task<OperacaoResponse?> CadastrarProfessor(CadastroProfessorRequest request)       
        {
            Debug.WriteLine($"Iniciando cadastro do professor: {request.Nome}, {request.Email}, {request.Cpf}, {request.User}");
            try
            {                
                var response = await _httpClient.PostAsJsonAsync(
                    "api/cadastro/professor",
                    request);

                return await response.Content.ReadFromJsonAsync<OperacaoResponse>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }        
    }
}
