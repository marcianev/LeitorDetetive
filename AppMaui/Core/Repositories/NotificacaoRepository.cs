using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Repositories
{
    namespace AppMaui.Core.Repositories
    {
        /// <summary>
        /// Repositório para operações com notificações filtradas por usuário.
        /// </summary>
        public class NotificacaoRepository(DatabaseService context)
        {
            private readonly SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(Notificacao notificacao) => await _db.InsertAsync(notificacao);

            public async Task<List<Notificacao>> GetAll() => await _db.Table<Notificacao>().ToListAsync();

            public async Task<List<Notificacao>> GetByUsuario(int usuarioId) =>
                await _db.Table<Notificacao>().Where(n => n.UsuarioId == usuarioId).ToListAsync();

            public async Task<Notificacao?> GetById(int id) => await _db.Table<Notificacao>().FirstOrDefaultAsync(n => n.Id == id);

            public async Task<int> Update(Notificacao notificacao) => await _db.UpdateAsync(notificacao);

            public async Task<int> Delete(Notificacao notificacao) => await _db.DeleteAsync<Notificacao>(notificacao);
        }
    }
}