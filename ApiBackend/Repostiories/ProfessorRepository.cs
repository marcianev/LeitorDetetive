using ApiBackend.Data;
using ApiBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiBackend.Repostiories
{
    public class ProfessorRepository
    {
        readonly AppDbContext _context;

        public ProfessorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Professor> AddAsync(Professor professor)
        {
            _context.Add(professor);
            await _context.SaveChangesAsync();
            return professor;
        }

        public async Task<Professor?> GetByEmail(string email)
        {
            return _context.Professores.FirstOrDefault(p => p.Email == email);
        }

        public async Task<Professor?> GetByUsuarioId(int UsuarioId)
        {
            return _context.Professores.FirstOrDefault(p => p.UsuarioId == UsuarioId);
        }

        public async Task<bool> CpfExists(string cpf)
        {
            return await _context.Professores.AnyAsync(p=>p.Cpf == cpf);  
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Professores.AnyAsync(p => p.Email == email);
        }
    }
}
