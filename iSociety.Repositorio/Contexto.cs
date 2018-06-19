using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace iSociety.Models
{
    public class Contexto : IDisposable
    {

        private readonly MySqlConnection minhaConexao;

        public Contexto()
        {
            minhaConexao = new MySqlConnection("server=127.0.0.1;port=3306;User Id=root;database=iSociety; password=1234;SslMode=none");
            minhaConexao.Open();
        }        
        public void ExecutaComando(string strQuery)
        {
            var cmdComando = new MySqlCommand
            {
                CommandText = strQuery,
                CommandType = CommandType.Text,
                Connection = minhaConexao
            };

            cmdComando.ExecuteNonQuery();
        }

        public MySqlDataReader ExecutaComandoComRetorno(string strQuery)
        {

            var cmdComando = new MySqlCommand
            {
                CommandText = strQuery,
                CommandType = CommandType.Text,
                Connection = minhaConexao
            };
            return cmdComando.ExecuteReader();
        }

        public void Dispose()
        {
            if (minhaConexao.State == ConnectionState.Open)
                minhaConexao.Close();
        }
    }
}
