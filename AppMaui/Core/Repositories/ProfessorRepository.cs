using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Repositories
{
    namespace AppMaui.Core.Repositories
    {
        /// <summary>
        /// Repositório para operações com professores.
        /// </summary>
        public class ProfessorRespository(DatabaseService context)
        {
            private readonly SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(Professor professor) => await _db.InsertAsync(professor);

            public async Task<List<Professor>> GetAll() => await _db.Table<Professor>().ToListAsync();

            public async Task<Professor?> GetById(int id) => await _db.Table<Professor>().Where(p => p.Id == id).FirstOrDefaultAsync();

            public async Task<int> Update(Professor professor) => await _db.UpdateAsync(professor);

            public async Task<int> Delete(Professor professor) => await _db.DeleteAsync<Professor>(professor);

            public async Task<Professor?> GetByUsuarioId(int id) => await _db.Table<Professor>().Where(p => p.UsuarioId == id).FirstOrDefaultAsync();
        }
    }
}
