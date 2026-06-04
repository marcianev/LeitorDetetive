using ApiBackend.Data;
using ApiBackend.Models;
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
    }
}
