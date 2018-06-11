using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace iSociety.Models
{
    public class QueryUsuarioConsumidor
    {

        private Contexto contexto;

        public void Inserir(UsuarioConsumidor user)
        {

            var strQuery = "";
            strQuery += "INSERT INTO usuarioConsumidor (idUsuario, nomeUsuario, email, senha, contaBanco)";
            strQuery += string.Format("VALUES('{0}', '{1}', '{2}', '{3}', '{4}')", user.Id, user.Nome, user.email, user.Senha, user.contaBanco);

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

        public void Alterar(UsuarioConsumidor user)
        {
            var strQuery = "UPDATE usuarioConsumidor SET ";
            strQuery += string.Format("nomeUsuario = '{0}', email = '{1}', senha = '{2}', contaBanco = '{3}'", user.Nome, user.email, user.Senha, user.contaBanco);
            strQuery += string.Format(" WHERE idUsuario = '{0}'", user.Id);

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void AlterarSenha(UsuarioConsumidor user)
        {
            var strQuery = "UPDATE usuarioConsumidor SET ";
            strQuery += string.Format("email = '{0}'", user.Senha);
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

        public List<string> ListarNomes()
        {
            using (contexto = new Contexto())
            {
                var strQuery = " SELECT nomeUsuario FROM usuarioConsumidor ";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return ConvertToString(DataReader);
            }
        }

        // Executa a Query para listar por ID e armazena resultados na variavel DataReader

        public List<UsuarioConsumidor> ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" SELECT * FROM usuarioConsumidor WHERE idUsuario = '{0}' ", id);
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);

                return ConvertToObject(DataReader);
            }
        }

        public bool ValidaUser(UsuarioConsumidor user)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" SELECT nomeUsuario, senha FROM usuarioConsumidor");
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                var Credenciais = ConvertToObjectLess(DataReader);

                foreach (var usuario in Credenciais)
                {
                    if (user.Nome == usuario.Nome && user.Senha == usuario.Senha)
                    {
                        return true;
                    }
                }

                return false;
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
                    Nome = reader["nomeUsuario"].ToString(),
                    email = reader["email"].ToString(),
                    Senha = reader["senha"].ToString(),
                    contaBanco = reader["contaBanco"].ToString()
                };
                users.Add(temObjeto);
            }
            reader.Close();
            return users;
        }

        private List<UsuarioConsumidor> ConvertToObjectLess(MySqlDataReader reader)
        {
            var users = new List<UsuarioConsumidor>();
            while (reader.Read())
            {
                var temObjeto = new UsuarioConsumidor()
                {
                    Nome = reader["nomeUsuario"].ToString(),
                    Senha = reader["senha"].ToString(),
                };
                users.Add(temObjeto);
            }
            reader.Close();
            return users;
        }

        private List<string> ConvertToString(MySqlDataReader reader)
        {
            var nomeUsuarios = new List<string>();
            while (reader.Read())
            {
                var temObjeto = new UsuarioConsumidor()
                {
                    Nome = reader["nomeUsuario"].ToString(),
                };

                nomeUsuarios.Add(temObjeto.Nome);
            }
            reader.Close();
            return nomeUsuarios;
        }



    }
}

