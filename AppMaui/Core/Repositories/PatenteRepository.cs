using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;
using System.Text.Json;


namespace AppMaui.Core.Repositories
{
    // classe já recebe injeção de dependecia do contexto]
    public class PatenteRepository(DatabaseService context)
    {
        //declaração de variavel do contexto recebe conexao
        private readonly SQLiteAsyncConnection _db = context.Conexao;

        //metodo add
        public async Task<int> Add(Patente patente) => await _db.InsertAsync(patente);

        //metodo listar
        public async Task<List<Patente>> GetAll() => await _db.Table<Patente>().ToListAsync();

        //metodo listar por trilha
        public async Task<List<Patente>> GetByTrilha(int id) =>
            await _db.Table<Patente>().Where(p => p.TrilhaId == id).ToListAsync();

        //metodo buscar por id
        public async Task<Patente?> GetById(int id) => await _db.Table<Patente>().FirstOrDefaultAsync(p => p.Id == id);

        //metodo atualizar
        public async Task<int> Update(Patente patente) => await _db.UpdateAsync(patente);

        //metodo deletar
        public async Task<int> Delete(Patente patente) => await _db.DeleteAsync<Patente>(patente);
        
        //metodo injeta a lista de patentes no banco de dados, 
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
