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
        /// Repositório para operações com alunos e geração de dashboards analíticos.
        /// </summary>
        public class AlunoRepository(DatabaseService context)
        {
            private readonly SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(Aluno aluno) => await _db.InsertAsync(aluno);

            public async Task<List<Aluno>> GetAll() => await _db.Table<Aluno>().ToListAsync();

            public async Task<Aluno?> GetById(int id) => await _db.Table<Aluno>().Where(a => a.Id == id).FirstOrDefaultAsync();

            public async Task<int> Update(Aluno aluno) => await _db.UpdateAsync(aluno);

            public async Task<int> Delete(int id) => await _db.DeleteAsync<Aluno>(id);

            /// <summary>Retorna alunos de uma turma que possuem usuário ativo.</summary>
            public async Task<List<Aluno>> GetByTurma(int turmaId)
            {
                var alunos = await _db.Table<Aluno>()
                                      .Where(a => a.TurmaUui == turmaId)
                                      .ToListAsync();

                var usuariosAtivos = await _db.Table<Usuario>()
                                              .Where(u => u.StatusUsuario == true)
                                              .ToListAsync();

                return alunos
                    .Where(a => usuariosAtivos.Any(u => u.Id == a.UsuarioId))
                    .ToList();
            }

            public async Task<bool> NicknameExist(string nickname) =>
                await _db.Table<Aluno>().Where(a => a.Nickname == nickname).FirstOrDefaultAsync() != null;

            public async Task<bool> AlunoExist(int id) =>
                await _db.Table<Aluno>().Where(a => a.Id == id).FirstOrDefaultAsync() != null;

            public async Task<Aluno?> GetByUsuarioId(int usuarioId) =>
                await _db.Table<Aluno>().Where(a => a.UsuarioId == usuarioId).FirstOrDefaultAsync();

            /// <summary>Retorna o ID da patente do próximo nível de uma trilha.</summary>
            public async Task<int> GetIdPatente(int nivelAtual, int patenteId)
            {
               var patente= await _db.Table<Patente>().
                    Where(p=> p.Id == patenteId)
                    .FirstOrDefaultAsync();
                if (patente == null)
                    return 0;
                int trilhaId = patente.TrilhaId;

                var patenteNivel = await _db.Table<Patente>()
                                      .Where(p => p.TrilhaId == trilhaId && p.Nivel == nivelAtual)
                                      .FirstOrDefaultAsync();     
                return patenteNivel.Id;
            }

            /// <summary>Gera dashboard do aluno com patente atual, leitura e última avaliação.</summary>
            public async Task<DashBoardADTO?> GerarDashBoard(int usuarioId)
            {
                string sql = @"
                SELECT 
                    p.Nome AS Patente,
                    a.Nickname,
                    m.Conteudo AS MensagemPatente,
                (SELECT COUNT(*) FROM Leitura lt WHERE lt.UsuarioId = a.UsuarioId
                AND lt.status = ?)
                AS QuantidadeLeitura,
                (SELECT l.Titulo FROM Leitura lu INNER JOIN Livro l ON l.Id = lu.LivroId 
                 WHERE lu.UsuarioId = a.UsuarioId AND lu.Status = '2' ORDER BY lu.DataFim DESC LIMIT 1) AS Titulo,
                (SELECT l.Capa FROM Leitura lu INNER JOIN Livro l ON l.Id = lu.LivroId 
                 WHERE lu.UsuarioId = a.UsuarioId AND lu.Status = '2' ORDER BY lu.DataFim DESC LIMIT 1) AS Capa,
                (SELECT av.Comentario FROM Avaliacao av WHERE av.UsuarioId = a.UsuarioId 
                 ORDER BY av.DataCadastro DESC LIMIT 1) AS Comentario
                FROM Aluno a
                LEFT JOIN Patente p ON p.Id = a.PatenteId
                LEFT JOIN Mensagem m ON m.Titulo = Patente
                WHERE a.UsuarioId = ?";

                var resultado = await _db.QueryAsync<DashBoardADTO>(sql, StatusLeitura.Concluida, usuarioId);
                return resultado.FirstOrDefault();
            }

            /// <summary>Gera dashboard do professor com alunos, patentes e total de leituras por turma.</summary>
            public async Task<List<DashBoardPDTO>?> GerarDashBoardP(int professorId)
            {
                string sql = @"
                Select 
                    a.Nome AS Aluno,
                    p.Nome AS Patente,
                    count(l.id) AS QuantidadeLeituras
                from Aluno A
                INNER JOIN leitura l ON l.UsuarioId = a.UsuarioId
                INNER JOIN Patente p ON p.id = a.PatenteId
                INner JOIN turma t ON t.id = a.TurmaId
                WHERE t.ProfessorId = ?
                GROUP BY a.Nome, p.nome
                ORDER BY count(l.id) DESC";

                var resultado = await _db.QueryAsync<DashBoardPDTO>(sql, professorId);
                return resultado;
            }

            ///<summary>Gerar pagina de alunos no perfil do professor com patente, nickame, nome, leitura atual, e quantidade de leituras concluidas</summary>
            public async Task<List<AlunoPDTO>?> BuscarDadosAlunoP(int idTurma)
            {
                string sql = @"
         SELECT
    a.Nome,
    a.Nickname,
    a.CodigoAcesso,
    p.Nome AS Patente,

    (
        SELECT l.Capa
        FROM Leitura lt
        INNER JOIN Livro l ON l.Id = lt.LivroId
        WHERE lt.UsuarioId = a.UsuarioId
        AND lt.Status = ?
        LIMIT 1
    ) AS CapaLivroAtual,

    (
        SELECT COUNT(*)
        FROM Leitura lt
        WHERE lt.UsuarioId = a.UsuarioId
        AND lt.Status = ?
    ) AS QuantidadeLivrosLidos

FROM Aluno a
INNER JOIN Patente p ON p.Id = a.PatenteId
WHERE a.TurmaId = ?";

                return await _db.QueryAsync<AlunoPDTO>(
                    sql,
                    StatusLeitura.Iniciada,
                    StatusLeitura.Concluida,
                    idTurma);
            }
        }
    }
}
