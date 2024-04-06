using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace ConsoleApp20;

public class Bot
{
    private readonly TelegramBotClient _botClient;

    public Bot(string token)
    {
        _botClient = new TelegramBotClient(token);
    }

    public void CreateCommands()
    {
        _botClient.SetMyCommandsAsync(new List<BotCommand>()
        {
            new()
            {
                Command = CustomBotCommand.START,
                Description = "Запустить бота"
            },
            new()
            {
                Command = CustomBotCommand.ABOUT,
                Description = "Что делает бот и как им пользоваться?"
            }
        });
    }

    public void StartReceiving()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new UpdateType[] { UpdateType.Message }
        };
        
        _botClient.StartReceiving(HandleUpdateAsync,HandleError,receiverOptions,cancellationToken);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var chatId = update.Message.Chat.Id;
        if (string.IsNullOrEmpty(update.Message.Text))
        {
            await _botClient.SendTextMessageAsync(chatId, text: "Данный бот принимает только текстовые сообщения.\n" +
                                                                "Введите ваш запрос правильно",
                cancellationToken: cancellationToken);
            return;
        }

        var messageText = update.Message.Text;
        if (IsStartCommand(messageText))
        {
            await _botClient.SendTextMessageAsync(chatId: chatId,
                text: "Привет, я бот по поиску картинок. Введите ваш запрос",
                cancellationToken: cancellationToken);
            return;
        }

        if (IsAboutCommand(messageText))
        {
            await _botClient.SendTextMessageAsync(chatId:chatId,text:"Данный бот возвращает 1 картинку по запросу пользователя.\n"+
                                                                     "Чтобы получить картинку, введите текстовый запрос",
                cancellationToken:cancellationToken);
            return;
        }

        await SendPhotoAsync(chatId, messageText, cancellationToken);

    }

    private Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(exception);
        return Task.CompletedTask;
    }

    private bool IsStartCommand(string messageText)
    {
        return messageText.ToLower() == CustomBotCommand.START;
    }

    private bool IsAboutCommand(string messageText)
    {
        return messageText.ToLower() == CustomBotCommand.ABOUT;
    }

    private async Task SendPhotoAsync(long chatId, string request, CancellationToken cancellationToken)
    {
        var photoUrl = await FlickrAPI.GetPhotoUrlAsync(request);
        if (photoUrl.Count == 0)
        {
            await _botClient.SendTextMessageAsync(chatId: chatId, text: "Изображений не найдено",
                cancellationToken: cancellationToken);
            return;
        }

        foreach (var photo in photoUrl)
        {
            await _botClient.SendPhotoAsync(chatId: chatId, photo: new InputFileUrl(photo),
                cancellationToken: cancellationToken);
        }
        
    }
}