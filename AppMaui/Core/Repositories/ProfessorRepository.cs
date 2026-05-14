using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Repositories
{
    // classe já recebe injeção de dependecia do contexto]
    public class ProfessorRespository(DatabaseService context)
    {
        //declaração de variavel do contexto recebe conexao
        private readonly SQLiteAsyncConnection _db = context.Conexao;

        //metodo add
        public async Task<int> Add(Professor professor) => await _db.InsertAsync(professor);

        //metodo listar
        public async Task<List<Professor>> GetAll() => await _db.Table<Professor>().ToListAsync();

        //metodo buscar por id
        public async Task<Professor?> GetById(int id) => await _db.Table<Professor>().Where(p => p.Id == id).FirstOrDefaultAsync();

        //metodo atualizar
        public async Task<int> Update(Professor professor) => await _db.UpdateAsync(professor);

        //metodo deletar
        public async Task<int> Delete(Professor professor) => await _db.DeleteAsync<Professor>(professor);

        //metodo buscar por usuarioId
        public async Task<Professor?> GetByUsuarioId(int id) => await _db.Table<Professor>().Where(p => p.UsuarioId == id).FirstOrDefaultAsync();
    }
}
