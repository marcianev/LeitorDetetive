using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Repositories
{
    namespace AppMaui.Core.Repositories
    {
        /// <summary>
        /// Repositório para operações CRUD de trilhas de aprendizado.
        /// </summary>
        public class TrilhaRepository(DatabaseService context)
        {
            private readonly SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(Trilha trilha) => await _db.InsertAsync(trilha);

            public async Task<List<Trilha>> GetAll() => await _db.Table<Trilha>().ToListAsync();

            public async Task<Trilha?> GetById(int id) => await _db.Table<Trilha>().FirstOrDefaultAsync(t => t.Id == id);

            public async Task<int> Update(Trilha trilha) => await _db.UpdateAsync(trilha);

            public async Task<int> Delete(Trilha trilha) => await _db.DeleteAsync<Trilha>(trilha);
        }
    }
}
