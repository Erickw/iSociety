using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSociety.Repositorio
{
    public class Contexto : IDisposable
    {

        private readonly MySqlConnection minhaConexao;

        public Contexto()
        {
            minhaConexao = new MySqlConnection("server=127.0.0.1;port=3306;User Id=root;database=iSociety; password=1234;SslMode=none");
            minhaConexao.Open();
        }
        //"server=31.170.166.180;port=3306;User Id=u599063365_root;database=u599063365_isoc; password=mSoUza5EVTR45mc;SslMode=none"
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
