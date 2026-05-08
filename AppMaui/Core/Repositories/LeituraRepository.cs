using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Repositories
{
    // classe já recebe injeção de dependecia do contexto]
    public class LeituraRepository(DatabaseService context)
    {
        //declaração de variavel do contexto recebe conexao
        private readonly SQLiteAsyncConnection _db = context.Conexao;
        //metodo add
        public async Task<int> Add(Leitura leitura) => await _db.InsertAsync(leitura);

        //metodo listar
        public async Task<List<Leitura>> GetAll() => await _db.Table<Leitura>().ToListAsync();

        //metodo listar por usuario
        public async Task<List<Leitura>> GetByUsuario(int usuarioId) =>
            await _db.Table<Leitura>().Where(l => l.UsuarioId == usuarioId).ToListAsync();

        //metodo buscar por id
        public async Task<Leitura?> GetById(int id) => await _db.Table<Leitura>().Where(l => l.Id == id).FirstOrDefaultAsync();

        //metodo atualizar
        public async Task<int> Update(Leitura leitura) => await _db.UpdateAsync(leitura);

        //metodo deletar
        public async Task<int> Delete(Leitura leitura) => await _db.DeleteAsync<Leitura>(leitura);
    }
}
