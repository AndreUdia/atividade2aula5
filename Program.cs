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

                SqlConnection conexao = null;
                SqlCommand comando = null;
                try
                {
                    conexao = new SqlConnection(strConexao);
                    conexao.Open();
                    Console.WriteLine("Digite um nome para busca");
                    string nome = Console.ReadLine();
                    int codAluno = -1;

                    string consulta = "SELECT * FROM Aluno WHERE nomeCompleto=@nomeCompleto";
                    comando = new SqlCommand(consulta, conexao);
                    comando.Parameters.Add(new SqlParameter("@nomeCompleto", nome));
                    SqlDataReader leitor = comando.ExecuteReader(CommandBehavior.CloseConnection);
                    while(leitor.Read())
                    {
                        codAluno = (int)leitor["codAluno"];
                        Console.WriteLine(codAluno);
                    }
                    if(codAluno < 0)
                    {
                        string novoNome = Console.ReadLine();
                        DateTime data = Convert.ToDateTime(Console.ReadLine());
                        string sobrenome = Console.ReadLine();
                        
                        consulta = "INSERT INTO Aluno VALUES(@novoNome, @novaData, @novoSobrenome)";
                        comando.Parameters.Add(new SqlParameter("@novoNome", novoNome));
                        comando.Parameters.Add(new SqlParameter("@novaData", data));
                        comando.Parameters.Add(new SqlParameter("@novoSobrenome", sobrenome));
                        comando = new SqlCommand(consulta, conexao);
                        int resultado = comando.ExecuteNonQuery();

                        if(resultado > 0)
                        {
                            consulta = "SELECT codAluno FROM Aluno WHERE nome=@novoNome";
                            comando = new SqlCommand(consulta, conexao);
                            int codigoAlunoInserido = (int)comando.ExecuteScalar();

                            bool controle = true;
                            while(controle) 
                            {
                                Console.WriteLine("Digite os dados do telefone");
                                string tipo = Console.ReadLine();
                                string telefone = Console.ReadLine();
                        
                                consulta = $"INSERT INTO Telefone VALUES({codigoAlunoInserido}, @tipo, @telefone)";
                                comando.Parameters.Add(new SqlParameter("@tipo", tipo));
                                comando.Parameters.Add(new SqlParameter("@telefone", telefone));
                                comando = new SqlCommand(consulta, conexao);
                                comando.ExecuteNonQuery();

                                Console.WriteLine("Deseja inserir um novo telefone? 1 para sim - 0 para não");
                                controle = Convert.ToBoolean(Console.ReadLine());
                            }
                        }
                    }
                    else // O erro tá aqui
                    {
                        
                        consulta = $"SELECT * FROM Telefone WHERE codAluno={codAluno}";
                        comando = new SqlCommand(consulta, conexao);
                        SqlDataReader leitorTelefone = comando.ExecuteReader(CommandBehavior.CloseConnection);
                        while(leitorTelefone.Read())
                        {
                            string result = (string)leitorTelefone["telefone"];
                            Console.WriteLine(result);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("A conexão deu erro!");
                }
                finally
                {
                    if(conexao != null)
                    {
                        conexao.Close();
                    }
                }
                Console.WriteLine("O Programa foi finalizado!");
        }
    }
}
