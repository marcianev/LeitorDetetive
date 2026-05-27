using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;

using System.Text.Json;


namespace AppMaui.Core.Repositories
{
    namespace AppMaui.Core.Repositories
    {
        /// <summary>
        /// Repositório para operações com livros e população de dados iniciais.
        /// </summary>
        public class LivroRepository(DatabaseService context)
        {
            private readonly SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(Livro livro) => await _db.InsertAsync(livro);

            public async Task<List<Livro>> GetAll() => await _db.Table<Livro>().ToListAsync();

            public async Task<Livro?> GetById(int id) => await _db.Table<Livro>().Where(l => l.Id == id).FirstOrDefaultAsync();

            public async Task<int> Update(Livro livro) => await _db.UpdateAsync(livro);

            public async Task<int> Delete(Livro livro) => await _db.DeleteAsync<Livro>(livro);

            /// <summary>Popula a tabela de livros com dados do arquivo livros.json se estiver vazia.</summary>
            public async Task PopularLivros()
            {
                var existe = await _db.Table<Livro>().CountAsync();
                if (existe > 0) return;

                using var stream = await FileSystem.OpenAppPackageFileAsync("livros.json");
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                var livros = JsonSerializer.Deserialize<List<Livro>>(json);
                if (livros != null)
                    await _db.InsertAllAsync(livros);
            }
        }
    }
}
