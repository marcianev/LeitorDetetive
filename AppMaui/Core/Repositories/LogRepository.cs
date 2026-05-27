using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Repositories
{
    namespace AppMaui.Core.Repositories
    {
        /// <summary>
        /// Repositório para operações CRUD de logs de auditoria.
        /// </summary>
        public class LogRepository(DatabaseService context)
        {
            private readonly SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(Log log) => await _db.InsertAsync(log);

            public async Task<List<Log>> GetAll() => await _db.Table<Log>().ToListAsync();

            public async Task<Log?> GetById(int id) => await _db.Table<Log>().FirstOrDefaultAsync(l => l.Id == id);

            public async Task<int> Update(Log log) => await _db.UpdateAsync(log);

            public async Task<int> Delete(Log log) => await _db.DeleteAsync(log);
        }
    }
}
