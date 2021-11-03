using System;
using System.Data;
using System.Data.SqlClient;

namespace atividade2aula5
{
    class Program
    {
        static void Main(string[] args)
        {
            string strConexao = "Server=localhost; Database=Sexta; Trusted_Connection=True;";

            Console.WriteLine("Digite um nome para busca");
            string nome = Console.ReadLine();
            int codigoAluno = ObterCodigoAluno(nome, strConexao);

            if(codigoAluno < 0)
            {
               Console.WriteLine("Não há alunos com esse nome, cadastre um novo!\n");
               CadastrarNovoALuno(strConexao);
            }
            else
            {
                ExibirTelefones(codigoAluno, strConexao);
            }
            Console.WriteLine("\nO Programa foi finalizado!");
        }
        private static int ObterCodigoAluno(string nome, string strConexao)
        {
            int codAluno = -1;

            try
            {
                using (SqlConnection conexao = new SqlConnection(strConexao))
                {
                    string consulta = "SELECT codAluno FROM Aluno WHERE nomeCompleto=@nomeCompleto";
                    SqlCommand comando =  new SqlCommand(consulta, conexao);
                    conexao.Open();
                    comando.Parameters.Add(new SqlParameter("@nomeCompleto", nome));
                    SqlDataReader leitor = comando.ExecuteReader();
                    while(leitor.Read())
                    {
                        codAluno = (int)leitor["codAluno"];
                    }
                    leitor.Close();
                } 
            }
            catch
            {
                Console.WriteLine("Erro ao obter o código do Aluno");
            }
            return codAluno;
        }
        private static void CadastrarNovoALuno(string strConexao)
        {
            int resultado = -1;

            Console.WriteLine("Digite o Nome: ");
            string novoNome = Console.ReadLine();
            Console.WriteLine("Digite o Sobrenome: ");
            string sobrenome = Console.ReadLine();
            Console.WriteLine("Digite Data de Nascimento: (dia/mes/ano)");
            DateTime data = Convert.ToDateTime(Console.ReadLine());

            try
            {
                using (SqlConnection conexao = new SqlConnection(strConexao))
                {
                    string consulta = "INSERT INTO Aluno VALUES(@novoNome, @novaData, @novoSobrenome)";
                    SqlCommand comando =  new SqlCommand(consulta, conexao);
                    conexao.Open();
                    comando.Parameters.Add(new SqlParameter("@novoNome", novoNome));
                    comando.Parameters.Add(new SqlParameter("@novaData", data));
                    comando.Parameters.Add(new SqlParameter("@novoSobrenome", sobrenome));
                    resultado = comando.ExecuteNonQuery();
                }
                Console.WriteLine("Aluno Cadastrado Com Sucesso \n");
            }
            catch
            {
                Console.WriteLine("Erro ao obter o cadastrar aluno \n");
            }
            
            if(resultado > 0)
            {
                Console.WriteLine("Cadastre os telefones para o novo Aluno\n");
                CadastrarTelefones(ObterCodigoAluno($"{novoNome} {sobrenome}", strConexao), strConexao);
            }
        }
        private static void CadastrarTelefones(int codAluno, string strConexao)
        {
            string resposta = "SIM";

            while(resposta.ToUpper() == "SIM")
            {
                Console.WriteLine("Digite o Tipo para o Telefone");
                string tipo = Console.ReadLine();
                Console.WriteLine("Digite o Telefone");
                string telefone = Console.ReadLine();

                try
                {
                    using (SqlConnection conexao = new SqlConnection(strConexao))
                    {
                        string consulta = $"INSERT INTO Telefone VALUES({codAluno}, @tipo, @telefone)";
                        SqlCommand comando =  new SqlCommand(consulta, conexao);
                        conexao.Open();
                        comando.Parameters.Add(new SqlParameter("@tipo", tipo));
                        comando.Parameters.Add(new SqlParameter("@telefone", telefone));
                        comando.ExecuteNonQuery();

                        Console.WriteLine("Deseja inserir um novo telefone? (SIM ou NAO) \n");
                        resposta = Console.ReadLine();
                    } 
                }
                catch
                {
                    Console.WriteLine("Erro ao cadastrar telefone");
                }
            }
        }
        private static void ExibirTelefones(int codAluno, string strConexao)
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection(strConexao))
                {
                    string consulta = $"SELECT * FROM Telefone WHERE codAluno={codAluno}";
                    SqlCommand comando = new SqlCommand(consulta, conexao);
                    conexao.Open();
                    SqlDataReader leitorTelefone = comando.ExecuteReader();
                    while(leitorTelefone.Read())
                    {
                        string result = (string)leitorTelefone["telefone"];
                        Console.WriteLine($"TELEFONE: {result}");
                    }
                } 
            }
            catch
            {
                Console.WriteLine("Erro ao exibir os telefones");
            }
        }
    }
}
