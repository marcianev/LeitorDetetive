using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Repositories
{
    // classe já recebe injeção de dependecia do contexto]
    public class NotificacaoRepository(DatabaseService context)
    {
        //declaração de variavel do contexto recebe conexao
        private readonly SQLiteAsyncConnection _db = context.Conexao;

        //metodo add
        public async Task<int> Add(Notificacao notificacao) => await _db.InsertAsync(notificacao);

        //metodo listar
        public async Task<List<Notificacao>> GetAll() => await _db.Table<Notificacao>().ToListAsync();

        //metodo listar notificações por usuário
        public async Task<List<Notificacao>> GetByUsuario(int usuarioId) =>
            await _db.Table<Notificacao>().Where(n => n.UsuarioId == usuarioId).ToListAsync();

        //metodo buscar por id
        public async Task<Notificacao?> GetById(int id) => await _db.Table<Notificacao>().FirstOrDefaultAsync(n => n.Id == id);

        //metodo atualizar
        public async Task<int> Update(Notificacao notificacao) => await _db.UpdateAsync(notificacao);

        //metodo deletar
        public async Task<int> Delete(Notificacao notificacao) => await _db.DeleteAsync<Notificacao>(notificacao);
    }
}