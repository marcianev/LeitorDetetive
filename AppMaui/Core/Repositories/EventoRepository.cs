using AppMaui.Core.Data;
using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Repositories
{
    namespace AppMaui.Core.Repositories
    {
        /// <summary>
        /// Repositório para operações CRUD de logs de auditoria.
        /// </summary>
        public class EventoRepository(DatabaseService context)
        {
            private readonly SQLiteAsyncConnection _db = context.Conexao;

            public async Task<int> Add(EventoSistema evento) => await _db.InsertAsync(evento);

            public async Task<List<EventoSistema>> GetAll() => await _db.Table<EventoSistema>().ToListAsync();

            public async Task<EventoSistema?> GetById(int id) => await _db.Table<EventoSistema>().FirstOrDefaultAsync(l => l.Id == id);

            public async Task<int> Update(EventoSistema evento) => await _db.UpdateAsync(evento);

            public async Task<int> Delete(EventoSistema evento) => await _db.DeleteAsync(evento);

            //busca e conta quantos eventos de definidos foram registrados para determinado usuario
            public async Task<int> CountEvento(int idUsuario, Eventos evento)
            {
                var sql = @"select count(*)
                            from EventoSistema es
                            where es.UsuarioId = ?
                            and TipoEvento = ?";

                return await _db.ExecuteScalarAsync<int>(sql, idUsuario, evento);
            }

            //busca o ultimo registro de um determinado evento para um determinado usuario
            public async Task<EventoSistema> GetByTipoUsuario(int idUsuario, Eventos evento)
            {
                var sql = @"select * 
                            from EventoSistema es
                            where es.UsuarioId = ? 
                            and es.TipoEvento = ?
                            order by es.DataEvento DESC
                            LIMIT 1;";

                return await _db.FindWithQueryAsync<EventoSistema>(sql, idUsuario, evento);
            }

            // Busca os nomes dos alunos de um determinado que estão há mais de uma determinada
            // quantidade de dias sem registrar o evento informado
            public async Task<List<string>> BuscarAlunosInativos(Eventos evento, int professorId, int dias)
            {
                var sql = @"SELECT 
                            a.Nickname,
                            a.Nome
                        FROM EventoSistema es

                        INNER JOIN Aluno a
                            ON a.UsuarioId = es.UsuarioId

                        INNER JOIN Turma t
                            ON t.Id = a.TurmaId

                        WHERE es.TipoEvento = ?
                        AND t.ProfessorId = ?

                        GROUP BY es.UsuarioId

                        HAVING MAX(es.DataEvento) <= DATE('now', '-' || ? || ' day')";

                return await _db.QueryScalarsAsync<string>(
                    sql,
                    evento,
                    professorId,
                    dias
                );
            }

            // Busca os alunos de determindao professor que registraram evento de subida
            // de nível nos últimos dias informados
            public async Task<List<string>> BuscarAlunosNivelUp(
               int dias, int professorId)
            {
                var sql = @"SELECT DISTINCT
                            a.Nickname,
                            a.Nome
                        FROM EventoSistema es

                        INNER JOIN Aluno a
                            ON a.UsuarioId = es.UsuarioId

                        INNER JOIN Turma t
                            ON t.Id = a.TurmaId

                        WHERE es.TipoEvento = ?
                        AND es.DataEvento >= DATE('now', '-' || ? || ' day')
                        AND t.ProfessorId = ?";

                return await _db.QueryScalarsAsync<string>(
                    sql,
                    Eventos.NivelUp,
                    dias,
                    professorId
                );
            }
        }
    }
}
