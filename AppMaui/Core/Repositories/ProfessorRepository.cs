using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Repositories
{
    namespace AppMaui.Core.Repositories
    {
        /// <summary>
        /// Repositório para operações com professores.
        /// </summary>
        public class ProfessorRespository(DatabaseService context)
        {
            private readonly SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(Professor professor) => await _db.InsertAsync(professor);

            public async Task<List<Professor>> GetAll() => await _db.Table<Professor>().ToListAsync();

            public async Task<Professor?> GetById(int id) => await _db.Table<Professor>().Where(p => p.Id == id).FirstOrDefaultAsync();

            public async Task<int> Update(Professor professor) => await _db.UpdateAsync(professor);

            public async Task<int> Delete(Professor professor) => await _db.DeleteAsync<Professor>(professor);

            public async Task<Professor?> GetByUsuarioId(int id) => await _db.Table<Professor>().Where(p => p.UsuarioId == id).FirstOrDefaultAsync();
            
            ///<summary>Verificar se email já está cadastrado</summary>
            public async Task<bool> EmailExiste(string email)
            {
                var usuario = await _db.Table<Professor>().FirstOrDefaultAsync(
                    p => p.Email.ToLower() == email.ToLower());

                return usuario != null;
            }

            ///<summary>Verificar se email já está cadastrado</summary>
            public async Task<bool> CPFExiste(string cpf)
            {
                var usuario = await _db.Table<Professor>().FirstOrDefaultAsync(
                    p => p.Cpf == cpf);

                return usuario != null;
            }

            ///<summary>buscar professor vinculado a turma de um aluno</summary>
            public async Task<Professor?> GetByTurma(int professorId)
            {
                var sql = @"Select *
                            from professor p
                            join turma t 
                            where t.professorId = ?";
                var resultado = await _db.QueryAsync<Professor>(sql, professorId);

                return resultado.FirstOrDefault();
            }
        }
    }
}
