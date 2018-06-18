using MySql.Data.MySqlClient;
using System.Collections.Generic;
using CryptSharp;
using iSociety.UI.Web.Models;
using System.Globalization;
using System;
using iSociety.Areas.Admin.Models;

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


        public void InserirPagamento(Pagamento pgto)
        {

            var strQuery = "";
            strQuery += $"INSERT INTO pagamento(idConsumidor, idAdministrador, horarioReservado, formaPagamento) VALUES({pgto.idConsumidor}, {pgto.idAdministrador}, '{pgto.horarioReservado}', '{pgto.formaPagamento}')";

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

        public void ConfirmarAluguel(Aluguel aluguel)
        {
            int confirmado = 0;
            if (aluguel.confirmado == true) {
                confirmado = 1;
            }
            var strQuery = "";
            strQuery += $"UPDATE aluguel SET confirmado = {confirmado}, idPagamento = {aluguel.idPagamento} WHERE idAluguel = {aluguel.idAluguel}";

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void CancelarAluguel(int id)
        {
            var strQuery = "";
            strQuery += $"UPDATE aluguel SET confirmado = 0, responsavelId = NULL WHERE idAluguel = {id}";

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        
        //public void AlterarAluguel(CampoAluguel aluguel)
        //{

        //    var strQuery = $"UPDATE aluguel SET reponsavelId = {aluguel.responsavelId}, pagamento = {aluguel.id} ";

        //    using (contexto = new Contexto())
        //    {
        //        contexto.ExecutaComando(strQuery);
        //    }

        //}

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

        public List<CampoAluguel> ListarCamposAlugueis()
        {
            using (contexto = new Contexto())
            {
                var strQuery = $"SELECT C.nomeCampo, C.rua, C.cep, C.numero, C.cidade, C.bar, A.idAluguel,  A.horarioInicio, A.horarioFim, A.valor FROM aluguel as A JOIN campo as  C ON A.idCampo = C.idCampo WHERE confirmado = {0} and A.responsavelId IS NULL";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return ConvertCampoAluguelToObject(DataReader);
            }
        }

        public int ListarIdPagamento() {

            using (contexto = new Contexto())
            {
                var strQuery = "SELECT idPagamento FROM pagamento WHERE idPagamento = (SELECT MAX(idPagamento) FROM pagamento)"; 
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);

                int id = new int();
                while (DataReader.Read())
                {
                    id = int.Parse(DataReader["idPagamento"].ToString());
                }
                DataReader.Close();

                return id;
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

        public List<CampoAluguel> ListarCamposAlugueisPorId(int id) {

            using (contexto = new Contexto())
            {
                var strQuery = $"SELECT C.nomeCampo, C.rua, C.cep, C.numero, C.cidade, C.bar, A.idAluguel,  A.horarioInicio, A.horarioFim," +
                               $" A.valor FROM aluguel as A JOIN campo as  C ON A.idCampo = C.idCampo WHERE idAluguel = {id}";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return ConvertCampoAluguelToObject(DataReader);
            }
        }

        public List<CampoAluguel> ListarCamposAlugueisPorIdUsuario(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = $"SELECT C.nomeCampo, C.rua, C.cep, C.numero, C.cidade, C.bar, A.idAluguel,  A.horarioInicio, A.horarioFim," +
                               $" A.valor FROM aluguel as A JOIN campo as  C ON A.idCampo = C.idCampo WHERE confirmado = {0} and A.responsavelId = {id}";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return ConvertCampoAluguelToObject(DataReader);
            }
        }       

        public List<CampoAluguel> ListarPeladas(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = $"SELECT C.nomeCampo, C.rua, C.cep, C.numero, C.cidade, C.bar, A.idAluguel,  A.horarioInicio, A.horarioFim," +
                               $" A.valor FROM aluguel as A JOIN campo as  C ON A.idCampo = C.idCampo WHERE confirmado = {1} and A.responsavelId = {id}";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return ConvertCampoAluguelToObject(DataReader);
            }
        }

        public List<UsuarioConsumidor> ListarPorNome(string nome)
        {
            using (contexto = new Contexto())
            {
                var strQuery = string.Format(" SELECT * FROM usuarioConsumidor WHERE nomeUsuario = '{0}' ", nome);
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);

                return ConvertToObject(DataReader);
            }
        }

        public int ListarIDAdmNPorNomeCampo(string nome) {
            using (contexto = new Contexto())
            {
                var strQuery = $"SELECT idAdministrador from campo WHERE idCampo = (SELECT idCampo from campo WHERE nomeCampo = '{nome}')";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);

                int id = new int();
                while (DataReader.Read())
                {
                    id = int.Parse(DataReader["idAdministrador"].ToString());
                }
                DataReader.Close();

                return id;
            }       
        }    

        public bool ValidaUser(UsuarioConsumidor user)
        {

            using (contexto = new Contexto())
            {
                
                var strQuery = $"SELECT nomeUsuario, senha FROM usuarioConsumidor WHERE nomeUsuario = '{user.Nome}'";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                var Credenciais = ConvertToObjectLess(DataReader);

                if (Crypter.CheckPassword(user.Senha, Credenciais[0].Senha))
                    return true;
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

        private List<CampoAluguel> ConvertCampoAluguelToObject(MySqlDataReader reader)
        {
            var fields = new List<CampoAluguel>();
            while (reader.Read())
            {
                var temObjeto = new CampoAluguel()
                {
                    aluguelId = int.Parse(reader["idAluguel"].ToString()),
                    responsavelId = 0,
                    nomeCampo = reader["nomeCampo"].ToString(),
                    rua = reader["rua"].ToString(),
                    cep = reader["cep"].ToString(),
                    numero = int.Parse(reader["numero"].ToString()),
                    cidade = reader["cidade"].ToString(),
                    bar = bool.Parse(reader["bar"].ToString()),
                    horarioInicio = reader["horarioInicio"].ToString(),
                    horarioFim = reader["horarioFim"].ToString(),
                    valor = Double.Parse(reader["valor"].ToString())
                };
                fields.Add(temObjeto);
            }
            reader.Close();
            return fields;
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

