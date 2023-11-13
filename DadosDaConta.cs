class ContaBancaria
{
    public string Titular { get; set; }
    public int Numero { get; set; }
    public double Saldo { get; set; }

    public ContaBancaria()
    {
        Random random = new Random();
        Numero = random.Next(1000, 2000);
    }

}