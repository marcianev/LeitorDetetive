using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;
using System.Text.Json;

namespace AppMaui.Core.Repositories
{
    namespace AppMaui.Core.Repositories
    {
        /// <summary>
        /// Repositório para operações com desafios e população de dados iniciais.
        /// </summary>
        public class DesafioRepository(DatabaseService context)
        {
            private readonly SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(Desafio desafio) => await _db.InsertAsync(desafio);

            public async Task<List<Desafio>> GetAll() => await _db.Table<Desafio>().ToListAsync();

            public async Task<List<Desafio>> GetByLivro(int livro) =>
                await _db.Table<Desafio>().Where(d => d.LivroId == livro).ToListAsync();

            public async Task<Desafio?> GetById(int id) => await _db.Table<Desafio>().Where(d => d.Id == id).FirstOrDefaultAsync();

            public async Task<int> Update(Desafio desafio) => await _db.UpdateAsync(desafio);

            public async Task<int> Delete(Desafio desafio) => await _db.DeleteAsync<Desafio>(desafio);

            /// <summary>Popula a tabela de desafios com dados do arquivo desafios.json se estiver vazia.</summary>
            public async Task PopularDesafios()
            {
                var existe = await _db.Table<Desafio>().CountAsync();
                if (existe > 0) return;

                using var stream = await FileSystem.OpenAppPackageFileAsync("desafios.json");
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                var desafios = JsonSerializer.Deserialize<List<Desafio>>(json);
                if (desafios != null)
                    await _db.InsertAllAsync(desafios);
            }
        }
    }
}
