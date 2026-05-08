using AppMaui.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Data
{
    public class DatabaseService
    {
        //declara a variável de conexão com o banco de dados SQLite
        private SQLiteAsyncConnection _conexao;

        //estabelece a conexão com o banco de dados SQLite chamando o método GetDatabasePath()
        //da classe DbContextcs para obter o caminho do banco de dados
        //e criar uma nova instância de SQLiteAsyncConnection
        public DatabaseService()
        {           
            if (_conexao != null)
                return;

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
            await _conexao.CreateTableAsync<Log>();
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
