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
    //classe já recebe injeção de dependência do contexto
    public class AlunoRepository(DatabaseService context)
    {
        //declara a variável de conexão com o banco de dados SQLite
        private readonly SQLiteAsyncConnection _db = context.Conexao;


        //metodo add
        public async Task<int> Add(Aluno aluno) => await _db.InsertAsync(aluno);

        //metodo listar
        public async Task<List<Aluno>> GetAll() => await _db.Table<Aluno>().ToListAsync();

        //metodo buscar por id
        public async Task<Aluno?> GetById(int id) => await _db.Table<Aluno>().Where(a => a.Id == id).FirstOrDefaultAsync();

        //metodo atualizar
        public async Task<int> Update(Aluno aluno) => await _db.UpdateAsync(aluno);

        //metodo deletar
        public async Task<int> Delete(int id) => await _db.DeleteAsync<Aluno>(id);

        //metodo para listar alunos por turma
        public async Task<List<Aluno>> GetByTurma(int turmaId) =>
            await _db.Table<Aluno>().Where(a => a.TurmaId == turmaId).ToListAsync();

        //metodo pesquisar se nickname já existe
        public async Task<bool> NicknameExist(string nickname) =>
            await _db.Table<Aluno>().Where(a => a.Nickname == nickname).FirstOrDefaultAsync() != null;

        //metodo pesquisa se aluno existe por id
        public async Task<bool> AlunoExist(int id) =>
            await _db.Table<Aluno>().Where(a => a.Id == id).FirstOrDefaultAsync() != null;

        //metodo buscar aluno por usuario
        public async Task<Aluno?> GetByUsuarioId(int usuarioId) =>
            await _db.Table<Aluno>().Where(a => a.UsuarioId == usuarioId).FirstOrDefaultAsync();

    }
}
