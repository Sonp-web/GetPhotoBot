using ConsoleApp20;

class Program
{
    static void Main(string[] args)
    {
        var bot = new Bot("7133943475:AAHL0QrGiHp6ztc-ZEAIYZ5cMjOqAV_XlOU");
        bot.CreateCommands();
        bot.StartReceiving();
        Console.ReadKey();
    }
}