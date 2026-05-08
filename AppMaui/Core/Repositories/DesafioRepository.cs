using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Repositories
{
    // classe já recebe injeção de dependecia do contexto]
    public class DesafioRepository(DatabaseService context)
    {
        //declaração de variavel do contexto recebe conexao
        private readonly SQLiteAsyncConnection _db = context.Conexao;

        //metodo add
        public async Task<int> Add(Desafio desafio) => await _db.InsertAsync(desafio);

        //metodo listar
        public async Task<List<Desafio>> GetAll() => await _db.Table<Desafio>().ToListAsync();

        //metodo listar por livro
        public async Task<List<Desafio>> GetByLivro(int livro) =>
            await _db.Table<Desafio>().Where(d => d.LivroId == livro).ToListAsync();

        //metodo buscar por id
        public async Task<Desafio?> GetById(int id) => await _db.Table<Desafio>().Where(d => d.Id == id).FirstOrDefaultAsync();

        //metodo atualizar
        public async Task<int> Update(Desafio desafio) => await _db.UpdateAsync(desafio);

        //metodo deletar DesafioRepository
        public async Task<int> Delete(Desafio desafio) => await _db.DeleteAsync<Desafio>(desafio);

    }
}
