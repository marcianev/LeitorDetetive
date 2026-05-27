using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;

using System.Text.Json;

namespace AppMaui.Core.Repositories
{
    namespace AppMaui.Core.Repositories
    {
        /// <summary>
        /// Repositório para operações com mensagens template e população de dados iniciais.
        /// </summary>
        public class MensagemRepository(DatabaseService context)
        {
            private readonly SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(Mensagem mensagem) => await _db.InsertAsync(mensagem);

            public async Task<List<Mensagem>> GetAll() => await _db.Table<Mensagem>().ToListAsync();

            public async Task<Mensagem?> GetById(int id) => await _db.Table<Mensagem>().FirstOrDefaultAsync(m => m.Id == id);

            public async Task<int> Update(Mensagem mensagem) => await _db.UpdateAsync(mensagem);

            public async Task<int> Delete(int id) => await _db.DeleteAsync<Mensagem>(id);

            /// <summary>Popula a tabela de mensagens com dados do arquivo mensagens.json se estiver vazia.</summary>
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
}
