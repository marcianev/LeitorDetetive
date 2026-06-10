using ApiBackend.Data;
using ApiBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiBackend.Repostiories
{
    public class TurmaRepository
    {
        private readonly AppDbContext _db;

        public TurmaRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task<Turma?> Add(Turma turma)
        {
            _db.Turmas.Add(turma);

            int linhasAfetadas = await _db.SaveChangesAsync();
            Console.WriteLine($"Linhas afetadas: {linhasAfetadas}");

            return linhasAfetadas > 0 ? turma : null;
        }

        public async Task<List<Turma>> GetAll()
        {
            return await _db.Turmas.ToListAsync();
        }

       
        public  async Task<List<Turma>> GetByProfessor(int id)
        {
            return await _db.Turmas.Where(t => t.ProfessorId == id && t.Status == true).ToListAsync();
        }


        public async Task<Turma?> GetByUuid(string uuid)
        {
            return await _db.Turmas.FirstOrDefaultAsync(t => t.Uuid.ToString() == uuid);
        }

        public async Task<bool> Update(Turma turma)
        {
            _db.Turmas.Update(turma);
            int linhasAfetadas = await _db.SaveChangesAsync();
            return linhasAfetadas > 0;
        }

        public async Task<bool> Delete(string uuid)
        {
            var turma = await GetByUuid(uuid);
            if (turma == null)
                return false;

            _db.Turmas.Remove(turma);
            int linhasAfetadas = await _db.SaveChangesAsync();
            return linhasAfetadas > 0;
        }

        public async Task<Turma?> GetOneByProfessor(int id)
        {
            return _db.Turmas.FirstOrDefault(t => t.ProfessorId == id && t.Status == true);
        }

      
    }
}

