using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Repositories
{
    namespace AppMaui.Core.Repositories
    {
        /// <summary>
        /// Repositório para operações CRUD e consultas customizadas de usuários.
        /// </summary>
        public class UsuarioRepository(DatabaseService context)
        {
            private readonly SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(Usuario usuario) => await _db.InsertAsync(usuario);

            public async Task<List<Usuario>> GetAll() => await _db.Table<Usuario>().ToListAsync();

            public async Task<Usuario?> GetById(int id) => await _db.Table<Usuario>().Where(u => u.Id == id).FirstOrDefaultAsync();

            public async Task<int> Update(Usuario usuario) => await _db.UpdateAsync(usuario);

            public async Task<int> Delete(Usuario usuario) => await _db.DeleteAsync<Usuario>(usuario);

            public async Task<Usuario?> GetByUser(string user) =>
                await _db.Table<Usuario>().Where(u => u.User == user).FirstOrDefaultAsync();
        }
    }
}
