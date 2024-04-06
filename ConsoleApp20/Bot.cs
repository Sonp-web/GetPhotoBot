using Telegram.Bot;

namespace ConsoleApp20;

public class Bot
{
    private readonly TelegramBotClient _botClient;

    public Bot(string token)
    {
        _botClient = new TelegramBotClient(token);
    }
}