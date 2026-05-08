using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Repositories
{
    // classe já recebe injeção de dependecia do contexto]
    public class TrilhaRepository(DatabaseService context)
    {
        //declaração de variavel do contexto recebe conexao
        private readonly SQLiteAsyncConnection _db = context.Conexao;

        //metodo add
        public async Task<int> Add(Trilha trilha) => await _db.InsertAsync(trilha);

        //metodo listar
        public async Task<List<Trilha>> GetAll() => await _db.Table<Trilha>().ToListAsync();

        //metodo buscar por id
        public async Task<Trilha?> GetById(int id) => await _db.Table<Trilha>().FirstOrDefaultAsync(t => t.Id == id);

        //metodo atualizar
        public async Task<int> Update(Trilha trilha) => await _db.UpdateAsync(trilha);

        //metodo deletar
        public async Task<int> Delete(Trilha trilha) => await _db.DeleteAsync<Trilha>(trilha);
    }
}
