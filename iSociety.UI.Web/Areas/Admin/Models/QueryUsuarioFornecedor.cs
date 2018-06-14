using MySql.Data.MySqlClient;
using System.Collections.Generic;
using CryptSharp;
using iSociety.Models;
using iSociety.UI.Web.Models;
using System;

namespace iSociety.Areas.Admin.Models
{
    public class QueryUsuarioFornecedor
    {

        private Contexto contexto;

        public void Inserir(UsuarioFornecedor user)
        {

            var strQuery = "";
            strQuery += "INSERT INTO UsuarioFornecedor (idUsuario, nomeUsuario, email, senha, contaBanco)";
            strQuery += string.Format("VALUES('{0}', '{1}', '{2}', '{3}', '{4}')", user.Id, user.Nome, user.email, user.Senha, user.contaBanco);

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }
        
        public void InserirCampo(Campo campo) { 
        
            int bar = 0;
            if (campo.bar == true) {
                bar = 1;
            }
            var strQuery = "";
            strQuery += "INSERT INTO Campo (idAdministrador, nomeCampo, rua, cep, numero, cidade, bar)";
            strQuery += string.Format("VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", campo.idAdministrador, campo.Nome, campo.rua, campo.cep, campo.numero, campo.cidade, bar);

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void AlterarEmail(UsuarioFornecedor user)
        {
            var strQuery = "UPDATE UsuarioFornecedor SET ";
            strQuery += string.Format("email = '{0}'", user.email);
            strQuery += string.Format(" WHERE idUsuario = '{0}'", user.Id);

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void Alterar(UsuarioFornecedor user)
        {
            var strQuery = "UPDATE UsuarioFornecedor SET ";
            strQuery += string.Format("nomeUsuario = '{0}', email = '{1}', senha = '{2}', contaBanco = '{3}'", user.Nome, user.email, user.Senha, user.contaBanco);
            strQuery += string.Format(" WHERE idUsuario = '{0}'", user.Id);

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void AlterarCampo(Campo campo)
        {
            int bar = Convert.ToInt32(campo.bar);
            var strQuery = "UPDATE campo SET ";
            strQuery += string.Format("nomeCampo = '{0}', rua = '{1}', cep = '{2}', numero = '{3}', cidade = '{4}', bar = '{5}'", campo.Nome, campo.rua, campo.cep, campo.numero, campo.cidade, bar);
            strQuery += string.Format(" WHERE idCampo = '{0}'", campo.id);

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void AlterarSenha(UsuarioFornecedor user)
        {
            var strQuery = "UPDATE UsuarioFornecedor SET ";
            strQuery += string.Format("email = '{0}'", user.Senha);
            strQuery += string.Format(" WHERE idUsuario = '{0}'", user.Id);

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void AlterarContaBanco(UsuarioFornecedor user)
        {
            var strQuery = "UPDATE UsuarioFornecedor SET ";
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
            var strQuery = string.Format("DELETE FROM UsuarioFornecedor WHERE idUsuario = '{0}'", id);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }
        
        public void ExcluirCampo(int id)
        {
            var strQuery = string.Format("DELETE FROM campo WHERE idCampo = '{0}'", id);
            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        //Executa a Query e armazena resultado na variavel DataReader

        public List<UsuarioFornecedor> ListarTodos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = " SELECT * FROM UsuarioFornecedor ";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return ConvertToObject(DataReader);
            }
        }


        public List<Campo> ListarCamposPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" SELECT * FROM Campo WHERE idAdministrador = '{0}' ", id);
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return ConvertCampoToObject(DataReader);
            }
        }
                

        public Campo SelecionaCampo(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = $"SELECT * FROM campo WHERE idCampo = '{id}'";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                var campoSelecionado = ConvertCampoToObject(DataReader)[0];
                return campoSelecionado;
            }
        }

        public List<string> ListarNomes()
        {
            using (contexto = new Contexto())
            {
                var strQuery = " SELECT nomeUsuario FROM UsuarioFornecedor ";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return ConvertToString(DataReader);
            }
        }

        // Executa a Query para listar por ID e armazena resultados na variavel DataReader

        public List<UsuarioFornecedor> ListarPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" SELECT * FROM UsuarioFornecedor WHERE idUsuario = '{0}' ", id);
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);

                return ConvertToObject(DataReader);
            }
        }

        public List<UsuarioFornecedor> ListarPorNome(string nome)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" SELECT * FROM UsuarioFornecedor WHERE nomeUsuario = '{0}' ", nome);
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);

                return ConvertToObject(DataReader);
            }
        }


        public bool ValidaUser(UsuarioFornecedor user)
        {

            using (contexto = new Contexto())
            {
                
                var strQuery = $"SELECT nomeUsuario, senha FROM UsuarioFornecedor WHERE nomeUsuario = '{user.Nome}'";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                var Credenciais = ConvertToObjectLess(DataReader);

                if (Crypter.CheckPassword(user.Senha, Credenciais[0].Senha))
                    return true;
                return false;
            }
        } 
        
        // Converte o Resultado da query do metodo anterior em uma lista

        private List<UsuarioFornecedor> ConvertToObject(MySqlDataReader reader)
        {
            var users = new List<UsuarioFornecedor>();
            while (reader.Read())
            {
                var temObjeto = new UsuarioFornecedor()
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

        private List<Campo> ConvertCampoToObject(MySqlDataReader reader)
        {
            var fields = new List<Campo>();
            while (reader.Read())
            {
                var temObjeto = new Campo()
                {
                    id = int.Parse(reader["idCampo"].ToString()),
                    idAdministrador = int.Parse(reader["idAdministrador"].ToString()),
                    Nome = reader["nomeCampo"].ToString(),
                    rua = reader["rua"].ToString(),
                    cep = reader["cep"].ToString(),
                    numero = int.Parse(reader["numero"].ToString()),
                    cidade = reader["cidade"].ToString(),
                    bar = bool.Parse(reader["bar"].ToString())
                };
                fields.Add(temObjeto);
            }
            reader.Close();
            return fields;
        }

        private List<UsuarioFornecedor> ConvertToObjectLess(MySqlDataReader reader)
        {
            var users = new List<UsuarioFornecedor>();
            while (reader.Read())
            {
                var temObjeto = new UsuarioFornecedor()
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
                var temObjeto = new UsuarioFornecedor()
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

