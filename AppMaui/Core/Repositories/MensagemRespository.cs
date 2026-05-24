using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppMaui.Core.Repositories
{
    // classe já recebe injeção de dependecia do contexto]
    public class MensagemRepository(DatabaseService context)
    {
        //declaração de variavel do contexto recebe conexao
        private readonly SQLiteAsyncConnection _db = context.Conexao;

        //metodo add
        public async Task<int> Add(Mensagem mensagem) => await _db.InsertAsync(mensagem);

        //metodo listar
        public async Task<List<Mensagem>> GetAll() => await _db.Table<Mensagem>().ToListAsync();

        //metodo buscar por id
        public async Task<Mensagem?> GetById(int id) => await _db.Table<Mensagem>().FirstOrDefaultAsync(m => m.Id == id);

        //metodo atualizar
        public async Task<int> Update(Mensagem mensagem) => await _db.UpdateAsync(mensagem);

        //metodo deletar
        public async Task<int> Delete(int id) => await _db.DeleteAsync<Mensagem>(id);

        //metodo injeta a lista de mensagens
        public async Task PopularMensagens()
        {
            var existe = await _db.Table<Mensagem>().CountAsync();
            if (existe > 0) return;

            using var stream = await FileSystem.OpenAppPackageFileAsync("mensagens.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            var mensagens = JsonSerializer.Deserialize<List<Mensagem>>(json);
            if (mensagens != null)
                await _db.InsertAllAsync(mensagens);
        }
    }
}
