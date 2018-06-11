using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSociety.Dominio;

namespace Ui.Dos
{
    class Program
    {
        static void Main(string[] args)
        {

            QueryUsuarioConsumidor user = new QueryUsuarioConsumidor();

            Console.Write("Digite o usuario");
            string usuario = Console.ReadLine();

            Console.Write("Digite a conta de email");
            string email = Console.ReadLine();

            Console.Write("Digite a senha");
            string senha = Console.ReadLine();

            Console.Write("Digite a conta bancaria");
            string conta = Console.ReadLine();

            UsuarioConsumidor user1 = new UsuarioConsumidor
            {
                Nome = usuario,
                email = email,
                Senha = senha,
                contaBanco = conta,
            };

            user1.Id = 99;
            
            var usuarios = user.ListarTodos();

            foreach (var temp in usuarios)
            {
                Console.WriteLine(temp.nome);
            }
        }
    }
}

