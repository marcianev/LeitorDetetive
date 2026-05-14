using AppMaui.Core.Data;
using AppMaui.Core.Models;
using SQLite;


namespace AppMaui.Core.Repositories
{
    // classe já recebe injeção de dependecia do contexto]
    public class TurmaRepository(DatabaseService context)
    {
        //declaração de variavel do contexto recebe conexao
        private readonly SQLiteAsyncConnection _db = context.Conexao;

        //metodo add
        public async Task<int> Add(Turma turma) => await _db.InsertAsync(turma);

        //metodo listar
        public async Task<List<Turma>> GetAll() => await _db.Table<Turma>().ToListAsync();

        //metodo listar por usuario
        public async Task<List<Turma>> GetByProfessor(int id) =>
            await _db.Table<Turma>().Where(t => t.ProfessorId == id).ToListAsync();
        //metodo buscar por id
        public async Task<Turma?> GetById(int id) => await _db.Table<Turma>().FirstOrDefaultAsync(t => t.Id == id);

        //metodo atualizar
        public async Task<int> Update(Turma turma) => await _db.UpdateAsync(turma);

        //metodo deletar
        public async Task<int> Delete(Turma turma) => await _db.DeleteAsync<Turma>(turma);

        //metodo buscar turma unica de professor, enquanto não há multiplos cadastros
        public async Task<Turma?> GetOneByProfessor(int id) => 
            await _db.Table<Turma>().FirstOrDefaultAsync(t => t.ProfessorId == id);
    }
}
