using AppMaui.Core.Data;
using AppMaui.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Repositories
{
    namespace AppMaui.Core.Repositories
    {
        /// <summary>
        /// Repositório para respostas de alunos aos desafios com consultas customizadas.
        /// </summary>
        public class RespostaRepository(DatabaseService context)
        {
            private readonly SQLite.SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(Resposta resposta) => await _db.InsertAsync(resposta);

            public async Task<List<Resposta>> GetAll() => await _db.Table<Resposta>().ToListAsync();

            public async Task<List<Resposta>> GetByDesafio(int id) =>
                await _db.Table<Resposta>().Where(r => r.DesafioId == id).ToListAsync();

            public async Task<Resposta?> GetById(int id) => await _db.Table<Resposta>().Where(r => r.Id == id).FirstOrDefaultAsync();

            public async Task<int> Update(Resposta resposta) => await _db.UpdateAsync(resposta);

            public async Task<int> Delete(Resposta resposta) => await _db.DeleteAsync<Resposta>(resposta);

            public async Task<List<Resposta>> GetByAlunoDesafio(int alunoId, int desafioId) =>
                await _db.Table<Resposta>().Where(r => r.AlunoId == alunoId && r.DesafioId == desafioId).ToListAsync();

            /// <summary>Retorna respostas de um aluno para todos os desafios de um livro específico.</summary>
            public async Task<List<Resposta>> BuscarRespostasLivro(int alunoId, int livroId)
            {
                string sql = @"
                SELECT r.*
                FROM resposta r
                INNER JOIN desafio d ON d.id = r.desafioId
                WHERE r.alunoId = ?
                AND d.livroId = ?";

                return await _db.QueryAsync<Resposta>(sql, alunoId, livroId);
            }
        }
    }
}
