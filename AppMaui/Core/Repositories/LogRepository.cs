using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Repositories
{
    // classe já recebe injeção de dependecia do contexto]
    public class LogRepository(DatabaseService context)
    {
        //declaração de variavel do contexto recebe conexao
        private readonly SQLiteAsyncConnection _db = context.Conexao;

        //metodo add
        public async Task<int> Add(Log log) => await _db.InsertAsync(log);

        //metodo listar
        public async Task<List<Log>> GetAll() => await _db.Table<Log>().ToListAsync();

        //metodo buscar por id
        public async Task<Log?> GetById(int id) => await _db.Table<Log>().FirstOrDefaultAsync(l => l.Id == id);

        //metodo atualizar
        public async Task<int> Update(Log log) => await _db.UpdateAsync(log);

        //metodo deletar
        public async Task<int> Delete(Log log) => await _db.DeleteAsync(log);
    }
}
