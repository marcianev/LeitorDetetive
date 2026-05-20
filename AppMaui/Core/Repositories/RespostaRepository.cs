using AppMaui.Core.Data;
using AppMaui.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Repositories
{
    // classe já recebe injeção de dependência do banco de dados
    public class RespostaRepository(DatabaseService context)
    {
        //declara a variável de conexão com o banco de dados SQLite
        private readonly SQLite.SQLiteAsyncConnection _db = context.Conexao;

        //metodo add
        public async Task<int> Add(Resposta resposta) => await _db.InsertAsync(resposta);

        //metodo listar
        public async Task<List<Resposta>> GetAll() => await _db.Table<Resposta>().ToListAsync();

        //metodo resposta por desafio
        public async Task<List<Resposta>> GetByDesafio(int id) =>
            await _db.Table<Resposta>().Where(r => r.DesafioId == id).ToListAsync();

        //metodo buscar por id
        public async Task<Resposta?> GetById(int id) => await _db.Table<Resposta>().Where(r => r.Id == id).FirstOrDefaultAsync();

        //metodo atualizar
        public async Task<int> Update(Resposta resposta) => await _db.UpdateAsync(resposta);

        //metodo deletar
        public async Task<int> Delete(Resposta resposta) => await _db.DeleteAsync<Resposta>(resposta);
        
        //buscar resposta por aluno e por desafio 
        public async Task<List<Resposta>> GetByAlunoDesafio(int alunoId, int desafioId) =>
            await _db.Table<Resposta>().Where(r => r.AlunoId == alunoId && r.DesafioId == desafioId).ToListAsync();
    }
}
