using AppMaui.Core.Data;
using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Repositories
{
    //classe já recebe injeção de dependência do contexto
    public class AvaliacaoRepository(DatabaseService context)
    {
        //declaração de variavel do contexto recebe conexao
        private readonly SQLiteAsyncConnection _db = context.Conexao;

        //metodo add
        public async Task<int> Add(Avaliacao avaliacao) => await _db.InsertAsync(avaliacao);

        //metodo listar
        public async Task<List<Avaliacao>> GetAll() => await _db.Table<Avaliacao>().ToListAsync();

        //metodo listar por turma
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
        
        //metodo buscar por id
        public async Task<Avaliacao?> GetById(int id) => await _db.Table<Avaliacao>().Where(a => a.Id == id).FirstOrDefaultAsync();

        //metodo atualizar
        public async Task<int> Update(Avaliacao avaliacao) => await _db.UpdateAsync(avaliacao);

        //metodo deletar
        public async Task<int> Delete(Avaliacao avaliacao) => await _db.DeleteAsync(avaliacao);

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

            WHERE a.livroID = ?
            ";

            var resultado = await _db.QueryAsync<AvaliacaoDTO>(sql,idLivro);
            return resultado;

        }
    }
}
