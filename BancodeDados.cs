using Microsoft.Data.Sqlite;

class ContaBancariaService
{
    static string conexaoString = "Data Source=db/DB_BANK_PAIVA.db";

    public static void CriarConta(HashSet<ContaBancaria> contas)
    {
        Console.Write("-- Informe o nome do titular da conta: ");
        string titular = Console.ReadLine();

        ContaBancaria novaConta = new ContaBancaria
        {
            Titular = titular,
            Saldo = 0
        };

        contas.Add(novaConta);
        dBSalvarConta(novaConta);

        Console.Write("-------------------------------------------------------------------- ");
        Console.WriteLine($"\nConta criada com sucesso!");
        Console.WriteLine($"-- Número da conta: {novaConta.Numero}");
        Console.WriteLine($"-- Titular: {novaConta.Titular}");
        Console.Write("-------------------------------------------------------------------- ");
    }

    static void dBSalvarConta(ContaBancaria conta)
    {

        using (var conexao = new SqliteConnection(conexaoString))
        {
            conexao.Open();

            using (var comando = new SqliteCommand("INSERT INTO Contas (Numero, Titular, Saldo) VALUES (@Numero, @Titular, @Saldo);", conexao))
            {
                comando.Parameters.AddWithValue("@Numero", conta.Numero);
                comando.Parameters.AddWithValue("@Titular", conta.Titular);
                comando.Parameters.AddWithValue("@Saldo", conta.Saldo);

                comando.ExecuteNonQuery();
            }
        }
    }

    public static void Depositar()
    {
        Console.Write("-- Informe o número da conta para depósito: ");
        int numeroConta = int.Parse(Console.ReadLine());

        Console.Write("-- Informe o valor a ser depositado: ");
        double valor = double.Parse(Console.ReadLine());

        using (var conexao = new SqliteConnection(conexaoString))
        {
            conexao.Open();

            var comando = conexao.CreateCommand();
            comando.CommandText = "UPDATE Contas SET Saldo = Saldo + @Valor WHERE Numero = @Numero";
            comando.Parameters.AddWithValue("@Valor", valor);
            comando.Parameters.AddWithValue("@Numero", numeroConta);

            int rowsAffected = comando.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.Write("-------------------------------------------------------------------- ");
                Console.WriteLine($"\nDepósito realizado com sucesso!");
                Console.WriteLine($"-- Número da conta: {numeroConta}");
                Console.WriteLine($"-- Valor: {valor}");
                Console.Write("-------------------------------------------------------------------- ");
            }
            else
            {
                Console.WriteLine($"Conta com número {numeroConta} não encontrada.");
            }
        }
    }

    public static void ListarContas()
    {
        using (var conexao = new SqliteConnection(conexaoString))
        {
            conexao.Open();

            var comando = conexao.CreateCommand();
            comando.CommandText = "SELECT Numero, Titular, Saldo FROM Contas";

            var contas = comando.ExecuteReader();

            Console.WriteLine("Contas existentes:");
            while (contas.Read())
            {
                int numeroConta = Convert.ToInt32(contas["Numero"]);
                string titular = Convert.ToString(contas["Titular"]);
                double saldo = Convert.ToDouble(contas["Saldo"]);

                Console.WriteLine($"Número: {numeroConta}, Titular: {titular}, Saldo: {saldo:C}");
            }
        }
    }

    public static void Sacar()
    {
        Console.Write("-- Informe o número da conta para realizar o saque: ");
        int numeroConta = int.Parse(Console.ReadLine());

        Console.Write("-- Informe o valor para saque: ");
        double valor = double.Parse(Console.ReadLine());

        using (var conexao = new SqliteConnection(conexaoString))
        {
            conexao.Open();

            var comando = conexao.CreateCommand();
            comando.CommandText =
                "UPDATE Contas SET Saldo = Saldo - @Valor WHERE Numero = @Numero AND Saldo >= @Valor";

            comando.Parameters.AddWithValue("@Valor", valor);
            comando.Parameters.AddWithValue("@Numero", numeroConta);

            int rowsAffected = comando.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.Write("-------------------------------------------------------------------- ");
                Console.WriteLine($"\nSaque realizado com sucesso!");
                Console.WriteLine($"-- Número da conta: {numeroConta}");
                Console.WriteLine($"-- Valor: {valor}");
                Console.Write("-------------------------------------------------------------------- ");
            }
            else
            {
                Console.WriteLine($"Saque não permitido.");
            }
        }
    }

    public static void VerificarSaldo()
    {
        Console.Write("-- Informe o número da conta para verificar o saldo: ");
        int numeroConta = int.Parse(Console.ReadLine());

        using (var conexao = new SqliteConnection(conexaoString))
        {
            conexao.Open();

            var comando = conexao.CreateCommand();
            comando.CommandText = "SELECT Saldo FROM Contas WHERE Numero = @Numero";
            comando.Parameters.AddWithValue("@Numero", numeroConta);

            var reader = comando.ExecuteReader();

            if (reader.Read())
            {
                double saldo = Convert.ToDouble(reader["Saldo"]);
                Console.Write("-------------------------------------------------------------------- ");
                Console.WriteLine($"\nExtrato Bancário");
                Console.WriteLine($"-- Número da conta: {numeroConta}");
                Console.WriteLine($"-- Saldo: {saldo}");
                Console.Write("-------------------------------------------------------------------- ");
            }
            else
            {
                Console.WriteLine($"Conta com número {numeroConta} não encontrada.");
            }
        }
    }
}