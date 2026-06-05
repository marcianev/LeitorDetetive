using ApiBackend.Data;
using ApiBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ApiBackend.Repostiories
{
    public class EventoRepository
    {
        private readonly AppDbContext _context;

        public EventoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Evento> Add(Evento evento)
        {
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
           
            return evento;
        }

        public async Task<Evento?> GetByIdentificador(Guid guid)
        {
            return await _context.Eventos.FirstOrDefaultAsync(e => e.Identificador == guid);
        
        }
    }
}
