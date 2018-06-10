using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSociety.Repositorio;
using iSociety.Dominio;

namespace iSociety.Aplicacao
{
    public class QueryUsuarioConsumidor
    {

        private Contexto contexto;

        public void Inserir(UsuarioConsumidor user)
        {

            var strQuery = "";
            strQuery += "INSERT INTO usuarioConsumidor (idUsuario, nomeUsuario, email, senha, contaBanco)";
            strQuery += string.Format("VALUES('{0}', '{1}', '{2}', '{3}', '{4}')", user.Id, user.nome, user.email, user.senha, user.contaBanco);

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void AlterarEmail(UsuarioConsumidor user)
        {
            var strQuery = "UPDATE usuarioConsumidor SET ";
            strQuery += string.Format("email = '{0}'", user.email);
            strQuery += string.Format(" WHERE idUsuario = '{0}'", user.Id);

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void AlterarSenha(UsuarioConsumidor user)
        {
            var strQuery = "UPDATE usuarioConsumidor SET ";
            strQuery += string.Format("email = '{0}'", user.senha);
            strQuery += string.Format(" WHERE idUsuario = '{0}'", user.Id);

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void AlterarContaBanco(UsuarioConsumidor user)
        {
            var strQuery = "UPDATE usuarioConsumidor SET ";
            strQuery += string.Format("email = '{0}'", user.contaBanco);
            strQuery += string.Format(" WHERE idUsuario = '{0}'", user.Id);

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        //Metodo somente para classe usuario fornecedor

        public void Excluir(int id)
        {
            var strQuery = string.Format("DELETE FROM usuarioConsumidor WHERE idUsuario = '{0}'", id);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        //Executa a Query e armazena resultado na variavel DataReader

        public List<UsuarioConsumidor> ListarTodos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = " SELECT * FROM usuarioConsumidor ";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return ConvertToObject(DataReader);
            }
        }
        // Converte o Resultado da query do metodo anterior em uma lista

        private List<UsuarioConsumidor> ConvertToObject(MySqlDataReader reader)
        {
            var users = new List<UsuarioConsumidor>();
            while (reader.Read())
            {
                var temObjeto = new UsuarioConsumidor()
                {
                    Id = int.Parse(reader["idUsuario"].ToString()),
                    nome = reader["nomeUsuario"].ToString(),
                    email = reader["email"].ToString(),
                    senha = reader["senha"].ToString(),
                    contaBanco = reader["contaBanco"].ToString()
                };
                users.Add(temObjeto);
            }
            reader.Close();
            return users;
        }
    }
}

