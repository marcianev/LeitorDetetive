using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Logging
{
    public class LogService(LogRepository _repository)
    {
        //metodo salvar log
        public async Task<bool> SalvarLog(Log log)
        {
            try
            {
                //validações
                string s = log.Acao.ToString();
                if (s.Length > 10 ||
                    log.Campo.Length > 50 ||
                    (log.ValorAnterior != null && log.ValorAnterior.Length > 100) ||
                    log.ValorNovo.Length > 100)
                    return false;

                //atribui a data e hora atual ao log
                log.DataHora = DateTime.Now;

                await _repository.Add(log);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao salvar log: {ex.Message}");
            }
        }//fim método salvar log

        //metodo listar logs
        public async Task<List<Log>> ListarLogs()
        {
            try
            {
                var logs = await _repository.GetAll();
                return logs;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar logs: {ex.Message}");
            }
        }//fim metodo listar logs       
    }
}
