using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;


namespace AppMaui.Core.Repositories
{
    namespace AppMaui.Core.Repositories
    {
        /// <summary>
        /// Repositório para operações com turmas, incluindo filtros por professor.
        /// </summary>
        public class TurmaRepository(DatabaseService context)
        {
            private readonly SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(Turma turma) => await _db.InsertAsync(turma);

            public async Task<List<Turma>> GetAll() => await _db.Table<Turma>().ToListAsync();

            public async Task<List<Turma>> GetByProfessor(int id) =>
                await _db.Table<Turma>().Where(t => t.ProfessorId == id && t.Status == true).ToListAsync();

            public async Task<Turma?> GetById(int id) => await _db.Table<Turma>().FirstOrDefaultAsync(t => t.Id == id);

            public async Task<int> Update(Turma turma) => await _db.UpdateAsync(turma);

            public async Task<int> Delete(Turma turma) => await _db.DeleteAsync<Turma>(turma);

            /// <summary>Retorna a turma ativa de um professor (assumindo apenas uma turma por professor).</summary>
            public async Task<Turma?> GetOneByProfessor(int id) => 
                await _db.Table<Turma>().FirstOrDefaultAsync(t => t.ProfessorId == id && t.Status == true);

            public async Task<List<Turma>> GetNaoSincronizadas() =>
                await _db.Table<Turma>().Where(t => t.Sincronizado == false).ToListAsync();
        }
    }
}
