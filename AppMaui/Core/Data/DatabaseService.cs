using AppMaui.Core.Models;
using SQLite;

namespace AppMaui.Core.Data
{
    public class DatabaseService
    {
        
        private readonly SQLiteAsyncConnection _conexao;

        /// <summary>
        /// Responsável por estabelecer a conexão com o banco de dados SQLite 
        /// e criar as tabelas necessárias para as entidades do aplicativo.
        /// </summary>      
        public DatabaseService()
        {          
            //Evitar recriar a conexão 
            if (_conexao != null)
                return;

            //inicializa componentes nativos do SQLite
            SQLitePCL.Batteries_V2.Init();

            string caminho = Path.Combine(FileSystem.AppDataDirectory, "leitorDetetive.db3");   
            _conexao = new SQLiteAsyncConnection(caminho);        
        }

        public SQLiteAsyncConnection Conexao => _conexao;

        //cria as tabelas do banco de dados SQLite para as entidades
        public async Task CriarTabelas()
        {
            
                await _conexao.CreateTableAsync<Aluno>();
                await _conexao.CreateTableAsync<Avaliacao>();
                await _conexao.CreateTableAsync<Desafio>();
                await _conexao.CreateTableAsync<Leitura>();
                await _conexao.CreateTableAsync<Livro>();
                await _conexao.CreateTableAsync<EventoSistema>();
                await _conexao.CreateTableAsync<Mensagem>();
                await _conexao.CreateTableAsync<Notificacao>();
                await _conexao.CreateTableAsync<Patente>();
                await _conexao.CreateTableAsync<Professor>();
                await _conexao.CreateTableAsync<Resposta>();
                await _conexao.CreateTableAsync<Trilha>();
                await _conexao.CreateTableAsync<Turma>();
                await _conexao.CreateTableAsync<Usuario>();
                  
        }
    }
}
