using AppMaui.Core.Data;
using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Repositories
{
    //classe já recebe injeção de dependência do contexto
    public class AlunoRepository(DatabaseService context)
    {
        //declara a variável de conexão com o banco de dados SQLite
        private readonly SQLiteAsyncConnection _db = context.Conexao;


        //metodo add
        public async Task<int> Add(Aluno aluno) => await _db.InsertAsync(aluno);

        //metodo listar
        public async Task<List<Aluno>> GetAll() => await _db.Table<Aluno>().ToListAsync();

        //metodo buscar por id
        public async Task<Aluno?> GetById(int id) => await _db.Table<Aluno>().Where(a => a.Id == id).FirstOrDefaultAsync();

        //metodo atualizar
        public async Task<int> Update(Aluno aluno) => await _db.UpdateAsync(aluno);

        //metodo deletar
        public async Task<int> Delete(int id) => await _db.DeleteAsync<Aluno>(id);

        //metodo para listar alunos por turma
        public async Task<List<Aluno>> GetByTurma(int turmaId)
        {
            var alunos = await _db.Table<Aluno>()
                                  .Where(a => a.TurmaId == turmaId)
                                  .ToListAsync();

            var usuariosAtivos = await _db.Table<Usuario>()
                                          .Where(u => u.StatusUsuario == true)
                                          .ToListAsync();

            return alunos
                .Where(a => usuariosAtivos.Any(u => u.Id == a.UsuarioId))
                .ToList();
        }

        //metodo pesquisar se nickname já existe
        public async Task<bool> NicknameExist(string nickname) =>
            await _db.Table<Aluno>().Where(a => a.Nickname == nickname).FirstOrDefaultAsync() != null;

        //metodo pesquisa se aluno existe por id
        public async Task<bool> AlunoExist(int id) =>
            await _db.Table<Aluno>().Where(a => a.Id == id).FirstOrDefaultAsync() != null;

        //metodo buscar aluno por usuario
        public async Task<Aluno?> GetByUsuarioId(int usuarioId) =>
            await _db.Table<Aluno>().Where(a => a.UsuarioId == usuarioId).FirstOrDefaultAsync();

        //metodo para listar alunos por turma
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
        //construir dto da dashboradAluno 
        public async Task<DashBoardADTO> GerarDashBoard(int usuarioId)
        {
            string sql = @"
        SELECT 
    p.Nome AS Patente,

    a.Nickname,

    m.Conteudo AS MensagemPatente,

    (
        SELECT COUNT(*)
        FROM Leitura lt
        WHERE lt.UsuarioId = a.UsuarioId
    ) AS QuantidadeLeitura,

    (
        SELECT l.Titulo
        FROM Leitura lu
        INNER JOIN Livro l
            ON l.Id = lu.LivroId
        WHERE lu.UsuarioId = a.UsuarioId
          AND lu.Status = '2'
        ORDER BY lu.DataFim DESC
        LIMIT 1
    ) AS Titulo,

    (
        SELECT l.Capa
        FROM Leitura lu
        INNER JOIN Livro l
            ON l.Id = lu.LivroId
        WHERE lu.UsuarioId = a.UsuarioId
          AND lu.Status = '2'
        ORDER BY lu.DataFim DESC
        LIMIT 1
    ) AS Capa,

    (
        SELECT av.Comentario
        FROM Avaliacao av
        WHERE av.UsuarioId = a.UsuarioId
        ORDER BY av.DataCadastro DESC
        LIMIT 1
    ) AS Comentario

FROM Aluno a

LEFT JOIN Patente p
    ON p.Id = a.PatenteId

LEFT JOIN Mensagem m
    ON m.Titulo = Patente

WHERE a.UsuarioId = ?";

            var resultado = await _db.QueryAsync<DashBoardADTO>(sql, usuarioId);
            return resultado.FirstOrDefault();
        }

        //listar alunos, patentes e total de leitura
        public async Task<List<DashBoardPDTO>> GerarDashBoardP(int professorId)
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
WHERE t.ProfessorId = 9

GROUP BY
a.Nome,
p.nome

ORDER BY
count(l.id)DESC
";
            var resultado = await _db.QueryAsync<DashBoardPDTO>(sql, professorId);
            return resultado;
        }

    }
}
