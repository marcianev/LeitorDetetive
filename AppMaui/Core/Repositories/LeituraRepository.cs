using AppMaui.Core.Data;
using AppMaui.Core.DTOs;
using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Repositories
{
    namespace AppMaui.Core.Repositories
    {
        /// <summary>
        /// Repositório para operações com leituras de livros com consultas analíticas customizadas.
        /// </summary>
        public class LeituraRepository(DatabaseService context)
        {
            private readonly SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(Leitura leitura) => await _db.InsertAsync(leitura);

            public async Task<List<Leitura>> GetAll() => await _db.Table<Leitura>().ToListAsync();

            public async Task<List<Leitura>> GetByUsuario(int usuarioId) =>
                await _db.Table<Leitura>().Where(l => l.UsuarioId == usuarioId).ToListAsync();

            public async Task<Leitura?> GetById(int id) => await _db.Table<Leitura>().Where(l => l.Id == id).FirstOrDefaultAsync();

            public async Task<int> Update(Leitura leitura) => await _db.UpdateAsync(leitura);

            public async Task<int> Delete(Leitura leitura) => await _db.DeleteAsync<Leitura>(leitura);

            /// <summary>Evita duplicidade de leitura para um livro e usuário específico.</summary>
            public async Task<Leitura?> GetByLivroUsuario(int livroId, int usuarioId) =>
                await _db.Table<Leitura>().Where(l => l.LivroId == livroId && l.UsuarioId == usuarioId).FirstOrDefaultAsync();

            public async Task<Leitura?> GetLeituraAtualPorUsuario(int usuarioId) =>
                await _db.Table<Leitura>().Where(l => l.UsuarioId == usuarioId && l.Status == Enums.StatusLeitura.Iniciada).FirstOrDefaultAsync();

            public async Task<Leitura?> GetLeituraAnteriorPorUsuario(int usuarioId) =>
                await _db.Table<Leitura>().Where(l => l.UsuarioId == usuarioId && l.Status == Enums.StatusLeitura.Concluida).OrderByDescending(l => l.DataFim).FirstOrDefaultAsync();

            public async Task<int> ContarLeiturasConcluidas(int usuarioId) =>
                await _db.Table<Leitura>().Where(l => l.UsuarioId == usuarioId && l.Status == Enums.StatusLeitura.Concluida).CountAsync();

            /// <summary>Retorna os 5 livros mais lidos pelas turmas de um professor específico.</summary>
            public async Task<List<LivrosMaisLidosDTO>> GetTopLivrosProfessor(int idProfessor)
            {
                string sql = @"
                SELECT
                    t.nome AS Turma,
                    l.titulo AS TituloLivro,
                    l.capa AS CapaLivro,
                    COUNT(le.id) AS QuantidadeLeituras
                FROM leitura le 
                INNER JOIN aluno al ON le.usuarioId = al.usuarioId
                INNER JOIN turma t ON al.turmaId = t.id
                INNER JOIN livro l ON le.livroId = l.id
                INNER JOIN professor pt ON t.professorId = pt.Id
                WHERE pt.Id = ? AND le.status = 2
                GROUP BY
                    t.id,
                    t.nome,
                    l.id,
                    l.titulo,
                    l.capa
                ORDER BY QuantidadeLeituras DESC
                LIMIT 5";

                var resultado = await _db.QueryAsync<LivrosMaisLidosDTO>(
                    sql,
                    idProfessor,
                    (int)StatusLeitura.Concluida
                );

                return resultado;
            } 
        }
    }
}

