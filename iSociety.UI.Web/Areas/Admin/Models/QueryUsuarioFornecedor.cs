using MySql.Data.MySqlClient;
using System.Collections.Generic;
using CryptSharp;
using iSociety.Models;
using iSociety.UI.Web.Models;
using System;
using iSociety.UI.Web.Areas.Admin.Models;
using System.Linq;

namespace iSociety.Areas.Admin.Models
{
    public class QueryUsuarioFornecedor
    {
        private Contexto contexto;

        //Métodos para inserção

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

        public void AdicionarHorario(Horario horario)
        {
            IEnumerable<String> horarios = horario.horarios.Split(',');

            foreach (String item in horarios)
            {
                var strQuery = "";
                strQuery += "INSERT INTO horarios (idCampo, horarioDisponivel)";
                strQuery += string.Format("VALUES('{0}', '{1}')", horario.idCampo, item);

                using (contexto = new Contexto())
                {
                    contexto.ExecutaComando(strQuery);
                }
            }
        }

        public void CriarAluguel(Aluguel aluguel)
        {
            int confirmado = 0;
            if (aluguel.confirmado == true)
            {
                confirmado = 1;
            }
            var strQuery = "";
            strQuery += "INSERT INTO aluguel (idAluguel, idCampo,horarioInicio, horarioFim, confirmado, valor)";
            strQuery += string.Format("VALUES( '{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", aluguel.idAluguel, aluguel.idCampo, aluguel.horarioInicio, aluguel.horarioFim, confirmado, aluguel.valor);

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }

        }

        public void CriarPlanoMensal(PlanoMensal planoMensal)
        {
            var strQuery = "";
            strQuery += $"INSERT INTO planoMensal (idPlanoMensal, campoId , horarioInicio, horarioFim, diaSemana) " +
                        $"VALUES ({planoMensal.idPlanoMensal}, {planoMensal.campoId}, '{planoMensal.horarioInicio}', " +
                        $"'{planoMensal.horarioFim}', '{planoMensal.diaSemana}')";

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        //Métodos de verificação

        public bool VerificaUserName(UsuarioFornecedor user)
        {
            using (contexto = new Contexto())
            {
                var strQuery = $"SELECT nomeUsuario FROM usuarioFornecedor WHERE nomeUsuario LIKE '{user.Nome}%'";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                var Credenciais = ConvertToString(DataReader);
                return Credenciais.Count < 0;
            }
        }
                
        public bool VerificaHorario(Horario horario)
        {//Verifica se o horario adicionado já é existente, caso seja a listaHorarios é maior que 0

            IEnumerable<String> horarios = horario.horarios.Split(',');
            var listaHorarios = new List<Horario>();
            var horariosRetornados = new List<MySqlDataReader>();

            foreach (String item in horarios)
            {
                using (contexto = new Contexto())
                {
                    var strQuery = $"SELECT idCampo ,horarioDisponivel FROM horarios WHERE horarioDisponivel LIKE '{item}%' AND idCampo = '{horario.idCampo}'";
                    var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    while (DataReader.Read())
                    {
                        var temObjeto = new Horario()
                        {
                            idCampo = int.Parse(DataReader["idCampo"].ToString()),
                            horarios = DataReader["horarioDisponivel"].ToString(),
                        };
                        listaHorarios.Add(temObjeto);
                    }
                }
            }
            return listaHorarios.Count == 0;
        }
        
        public bool VerificaHorarioAluguel(Horario horario)
        { //Verifica se o horário para aquele aluguel esta na faixa de horário de um horario de aluguel existente, caso esteja, a listaHorarios é maior que 0.

            String[] horariosDivididos = horario.horarios.Split(new Char[] { ',', ':' });
            List<String> horariosConvertidos = new List<string>();
            // Converte a matriz horariosDivididos em Array
            foreach (var item in horariosDivididos)
            {
                horariosConvertidos.Add(item);
            }
            // Criar um array somente com as horas da lista horariosConvertidos
            var somenteHoras = horariosConvertidos.Where(val => val != "00").ToArray();
            var listaHorarios = new List<Horario>();
            var horariosRetornados = new List<MySqlDataReader>();

            foreach (var item in somenteHoras)
            {

                using (contexto = new Contexto())
                {
                    var strQuery = $"SELECT idCampo , horarioInicio FROM aluguel WHERE horarioInicio LIKE '{item}%' AND idCampo = {horario.idCampo}";
                    var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    while (DataReader.Read())
                    {
                        var temObjeto = new Horario()
                        {
                            idCampo = int.Parse(DataReader["idCampo"].ToString()),
                            horarios = DataReader["horarioInicio"].ToString(),
                        };
                        listaHorarios.Add(temObjeto);
                    }
                }
            }
            return listaHorarios.Count == 0;
        }

        public bool ValidaUser(UsuarioFornecedor user)
        {

            using (contexto = new Contexto())
            {

                var strQuery = $"SELECT nomeUsuario, senha FROM UsuarioFornecedor WHERE nomeUsuario = '{user.Nome}'";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                var Credenciais = ConvertToObjectLess(DataReader);

                if (Credenciais.Count == 0)
                    return false;
                if (Crypter.CheckPassword(user.Senha, Credenciais[0].Senha))
                    return true;
                return false;
            }
        }

        public bool ValidaUserMaster(UsuarioFornecedor user)
        {

            using (contexto = new Contexto())
            {

                var strQuery = $"SELECT nomeUsuario, senha FROM usuarioFornecedor WHERE (SELECT nomeUsuario FROM usuarioFornecedor WHERE nomeUsuario = '{user.Nome}') LIKE 'master'";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                var Credenciais = ConvertToObjectLess(DataReader);
                if (Credenciais.Count == 0)
                    return false;
                if (Crypter.CheckPassword(user.Senha, Credenciais[0].Senha))
                    return true;
                return false;
            }
        }

        //Métodos de atualização

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

        public void AlterarAluguel(Aluguel aluguel)
        {
            var strQuery = "";
            strQuery += $"UPDATE aluguel SET idAluguel = {aluguel.idAluguel}, idCampo = {aluguel.idCampo}, responsavelId = {aluguel.reponsavelId}," +
                        $" horarioInicio = '{aluguel.horarioInicio}', horarioFim = '{aluguel.horarioFim}', confirmado = {aluguel.confirmado}, valor = {aluguel.valor} WHERE idAluguel = {aluguel.idAluguel}";

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void ConfirmarAluguel(CampoAluguel campAluguel)
        {

            var strQuery = "";
            strQuery += $"UPDATE aluguel SET (idCampo,horarioInicio, horarioFim, confirmado, valor)";

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }
        
        //Métodos de remoção

        public void RemoverAluguel(Aluguel aluguel)
        {

            var strQuery = "";
            strQuery += $"DELETE FROM aluguel WHERE idAluguel = {aluguel.idAluguel} and horarioInicio = '{aluguel.horarioInicio}'";

            using (contexto = new Contexto())
            {
                contexto.ExecutaComando(strQuery);
            }
        }

        public void ExcluirHorario(Horario horario)
        {
            IEnumerable<String> horarios = horario.horarios.Split(',');

            foreach (String item in horarios)
            {
                var strQuery = "";
                strQuery += $"DELETE FROM horarios WHERE idCampo = '{horario.idCampo}' and horarioDisponivel = '{item}'";
                using (contexto = new Contexto())
                {
                    contexto.ExecutaComando(strQuery);
                }
            }
        }

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
                
        //Metódos de consulta

        public List<UsuarioFornecedor> ListarTodos()
        {//Executa a Query e armazena resultado na variavel DataReader
            using (contexto = new Contexto())
            {
                var strQuery = " SELECT * FROM UsuarioFornecedor ";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return ConvertToObject(DataReader);
            }
        }

        public List<Campo> ListarTodosCampos()
        {
            using (contexto = new Contexto())
            {
                var strQuery = " SELECT * FROM campo ";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return ConvertCampoToObject(DataReader);
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
        
        public List<Aluguel> ListarAluguelPorId(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = $" SELECT * FROM aluguel WHERE idAluguel = {id}";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return ConvertAluguelToObject(DataReader);
            }
        }

        public List<CampoAluguel> ListarCamposAlugueisPorId(int id)
        {

            using (contexto = new Contexto())
            {
                var strQuery = $"SELECT C.idAdministrador, C.nomeCampo, C.rua, C.cep, C.numero, C.cidade, C.bar, A.idAluguel, " +
                               $" A.horarioInicio, A.horarioFim, A.valor, A.confirmado FROM aluguel as A JOIN campo as C ON A.idCampo = C.idCampo WHERE idAdministrador ={id} AND confirmado = 0";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return ConvertCampoAluguelToObject(DataReader);
            }
        }

        public List<CampoAluguel> ListarPagamentos(int id)
        {
            using (contexto = new Contexto())
            {
                var strQuery = $"SELECT C.nomeCampo, C.rua, C.cep, C.numero, C.cidade, C.bar, A.idAluguel,  A.horarioInicio, A.horarioFim," +
                               $" A.valor FROM aluguel as A JOIN campo as  C ON A.idCampo = C.idCampo WHERE confirmado = {1} and C.idAdministrador = {id}";
                var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                return ConvertCampoAluguelToObject(DataReader);
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

        public List<UsuarioFornecedor> ListarPorId(int id)
        {// Executa a Query para listar por ID e armazena resultados na variavel DataReader
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

        public List<CampoHorario> ListarHorarios(int id)
        {
                using (contexto = new Contexto())
                {
                    var strQuery = $"SELECT nomeCampo, horarioDisponivel FROM campo as C JOIN horarios as H ON C.idCampo = H.idCampo WHERE idAdministrador = {id}";
                    var DataReader = contexto.ExecutaComandoComRetorno(strQuery);
                    return ConvertCampoHorarioToObject(DataReader);
                }
        }

        //Métodos de conversão em Objeto

        private List<UsuarioFornecedor> ConvertToObject(MySqlDataReader reader)
        {// Converte o Resultado da query do metodo anterior em uma lista
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
        
        private List<Aluguel> ConvertAluguelToObject(MySqlDataReader reader)
        {
            var rents = new List<Aluguel>();
            while (reader.Read())
            {
                var temObjeto = new Aluguel()
                {
                    idAluguel = int.Parse(reader["idAluguel"].ToString()),
                    idCampo = int.Parse(reader["idCampo"].ToString()),
                   // reponsavelId = (reader["responsavelId"].DB),
                    //idPagamento = (reader["responsavelId"].ToString()),
                    horarioInicio = reader["horarioInicio"].ToString(),
                    horarioFim = reader["horarioFim"].ToString(),
                    confirmado = bool.Parse(reader["confirmado"].ToString())
                };
                rents.Add(temObjeto);
            }
            reader.Close();
            return rents;
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

        private List<CampoHorario> ConvertCampoHorarioToObject(MySqlDataReader reader)
        {
            var horarios = new List<CampoHorario>();
            while (reader.Read())
            {
                var temObjeto = new CampoHorario()
                {
                    nomeCampo = reader["nomeCampo"].ToString(),
                    horarioDisponivel = reader["horarioDisponivel"].ToString(),
                };
                horarios.Add(temObjeto);
            }
            reader.Close();
            return horarios;
        }
    }
}

