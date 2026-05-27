using AppMaui.Core.Data;
using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Repositories
{
    namespace AppMaui.Core.Repositories
    {
        /// <summary>
        /// Repositório para operações com avaliações e consultas analíticas de livros.
        /// </summary>
        public class AvaliacaoRepository(DatabaseService context)
        {
            private readonly SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(Avaliacao avaliacao) => await _db.InsertAsync(avaliacao);

            public async Task<List<Avaliacao>> GetAll() => await _db.Table<Avaliacao>().ToListAsync();

            /// <summary>Retorna todas as avaliações dos alunos de uma turma específica.</summary>
            public async Task<List<Avaliacao>> GetByTurma(int turmaId)
            {
                string query = @"
                                SELECT a.*
                                FROM Avaliacao a
                                INNER JOIN Aluno al ON a.AlunoId = al.Id
                                WHERE al.TurmaId = ?
                                ";

                return await _db.QueryAsync<Avaliacao>(query, turmaId);
            }

            public async Task<Avaliacao?> GetById(int id) => await _db.Table<Avaliacao>().Where(a => a.Id == id).FirstOrDefaultAsync();

            public async Task<int> Update(Avaliacao avaliacao) => await _db.UpdateAsync(avaliacao);

            public async Task<int> Delete(Avaliacao avaliacao) => await _db.DeleteAsync(avaliacao);

            /// <summary>Retorna as avaliações de um livro com dados do aluno, turma e metadados do livro.</summary>
            public async Task<List<AvaliacaoDTO>> GetByLivro(int idLivro)
            {
                string sql = @"
                SELECT
                    a.Id as IdAvaliacao,
                    a.nota,
                    a.comentario,
                    a.status,
                    a.dataCadastro,
                    al.nickname AS Nickname,
                    t.nome AS Turma,
                    l.titulo AS NomeLivro,
                    l.autor AS Autor,
                    l.ilustrador AS Ilustrados,
                    l.capa AS Capa
                FROM avaliacao a
                INNER JOIN aluno al
                    ON a.usuarioId = al.usuarioId
                INNER JOIN turma t
                    ON al.turmaId = t.id
                INNER JOIN livro l
                    ON a.livroId = l.id
                WHERE a.livroID = ?";

                var resultado = await _db.QueryAsync<AvaliacaoDTO>(sql, idLivro);
                return resultado;
            }

            /// <summary>Retorna avaliações de um livro filtradas por turma do professor para moderação.</summary>
            public async Task<List<AvaliacaoDTO>> GetByLivroProfessor(int idLivro, int idProfessor)
            {
                string sql = @"
                SELECT
                    a.Id as IdAvaliacao,
                    a.nota,
                    a.comentario,
                    a.status,
                    a.dataCadastro,
                    al.nickname AS Nickname,
                    t.nome AS Turma,
                    l.titulo AS NomeLivro,
                    l.autor AS Autor,
                    l.ilustrador AS Ilustrados,
                    l.capa AS Capa
                FROM avaliacao a
                INNER JOIN aluno al
                    ON a.usuarioId = al.usuarioId
                INNER JOIN turma t
                    ON al.turmaId = t.id
                INNER JOIN livro l
                    ON a.livroId = l.id
                INNER JOIN professor pt
                    ON t.professorId = pt.Id
                WHERE a.livroId = ?
                  AND pt.Id = ?";

                var resultado = await _db.QueryAsync<AvaliacaoDTO>(
                    sql,
                    idLivro,
                    idProfessor
                );

                return resultado;
            }

            /// <summary>Retorna os 5 livros mais bem avaliados pelas turmas de um professor.</summary>
            public async Task<List<LivrosBemAvaliadosDTO>> GetBemAvalidado(int idProfessor)
            {
                var sql = @"
                        Select
                            t.nome AS Turma,
                            l.Titulo AS Titulo,
                            l.Capa AS Capa,
                            AVG(a.nota) AS Nota
                        FROM  avaliacao a
                        INNER JOIN aluno al ON al.usuarioId = a.usuarioId
                        INNER JOIN turma t ON t.Id = al.turmaId
                        INNER JOIN livro l ON l.Id = a.livroId
                        Where t.professorId = ?
                        GROUP BY
                           t.nome,
                           l.titulo,
                           l.capa
                        ORDER BY Nota DESC
                        limit 5";

                var resultado = await _db.QueryAsync<LivrosBemAvaliadosDTO>(
                    sql,
                    idProfessor);
                return resultado;
            }
        }
    }
}
