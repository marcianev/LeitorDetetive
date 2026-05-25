using AppMaui.Core.Data;
using AppMaui.Core.DTOs;
using AppMaui.Core.Enums;
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
    public class LivroRepository(DatabaseService context)
    {
        //declaração de variavel do contexto recebe conexao
        private readonly SQLiteAsyncConnection _db = context.Conexao;

        //metodo add
        public async Task<int> Add(Livro livro) => await _db.InsertAsync(livro);

        //metodo listar
        public async Task<List<Livro>> GetAll() => await _db.Table<Livro>().ToListAsync();

        //metodo buscar por id
        public async Task<Livro?> GetById(int id) => await _db.Table<Livro>().Where(l => l.Id == id).FirstOrDefaultAsync();

        //metodo atualizar
        public async Task<int> Update(Livro livro) => await _db.UpdateAsync(livro);

        //metodo deletar
        public async Task<int> Delete(Livro livro) => await _db.DeleteAsync<Livro>(livro);

        //metodo injeta a lista de livros no banco de dados, para popular a estante do aluno
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
