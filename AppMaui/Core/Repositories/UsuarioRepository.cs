using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Repositories
{
    // classe já recebe injeção de dependecia do contexto]
    public class UsuarioRepository(DatabaseService context)
    {
        //declaração de variavel do contexto recebe conexao
        private readonly SQLiteAsyncConnection _db =context.Conexao;        

        //metodo add
        public async Task<int> Add(Usuario usuario) => await _db.InsertAsync(usuario);

        //metodo listar
        public async Task<List<Usuario>> GetAll() => await _db.Table<Usuario>().ToListAsync();

        //metodo buscar por id
        public async Task<Usuario?> GetById(int id) => await _db.Table<Usuario>().Where(u => u.Id == id).FirstOrDefaultAsync();

        //metodo atualizar
        public async Task<int> Update(Usuario usuario) => await _db.UpdateAsync(usuario);

        //metodo deletar
        public async Task<int> Delete(Usuario usuario) => await _db.DeleteAsync<Usuario>(usuario);

        //metodo para buscar usuario por user
        public async Task<Usuario?> GetByUser(string user) =>
            await _db.Table<Usuario>().Where(u => u.User == user).FirstOrDefaultAsync();
    }
}
