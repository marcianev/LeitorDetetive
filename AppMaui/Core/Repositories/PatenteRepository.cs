using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;
using System.Text.Json;


namespace AppMaui.Core.Repositories
{
    namespace AppMaui.Core.Repositories
    {
        /// <summary>
        /// Repositório para operações com patentes e população de dados iniciais.
        /// </summary>
        public class PatenteRepository(DatabaseService context)
        {
            private readonly SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(Patente patente) => await _db.InsertAsync(patente);

            public async Task<List<Patente>> GetAll() => await _db.Table<Patente>().ToListAsync();

            public async Task<List<Patente>> GetByTrilha(int id) =>
                await _db.Table<Patente>().Where(p => p.TrilhaId == id).ToListAsync();

            public async Task<Patente?> GetById(int id) => await _db.Table<Patente>().FirstOrDefaultAsync(p => p.Id == id);

            public async Task<int> Update(Patente patente) => await _db.UpdateAsync(patente);

            public async Task<int> Delete(Patente patente) => await _db.DeleteAsync<Patente>(patente);

            /// <summary>Popula a tabela de patentes com dados do arquivo patente.json se estiver vazia.</summary>
            public async Task PopularPatentes()
            {
                var existe = await _db.Table<Patente>().CountAsync();
                if (existe > 0) return;

                using var stream = await FileSystem.OpenAppPackageFileAsync("patente.json");
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                var patentes = JsonSerializer.Deserialize<List<Patente>>(json);
                if (patentes != null)
                    await _db.InsertAllAsync(patentes);
            }
        }
    }
}
