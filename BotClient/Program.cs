using Domain.Models;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotClient
{
    internal class Program
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [System.Runtime.InteropServices.DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        static async Task Main(string[] args)
        {
            ShowWindow(GetConsoleWindow(), 1);

            Console.WriteLine("Hello, World!");

            var botClient = new TelegramBotClient("6107848888:AAF5O60QZ0WbRDXW3aybcnfiROR_V6l63FU");

            using CancellationTokenSource cts = new CancellationTokenSource();

            ReceiverOptions receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
                );


            HttpClient client = new HttpClient();
            var result = await client.GetAsync("https://localhost:44356/api/Good");
            var test = await result.Content.ReadAsStringAsync();
            Console.WriteLine(test);

            Product[] products = JsonConvert.DeserializeObject<Product[]>(test);
            foreach (var p in products)
            {
                Console.WriteLine($"{p.NumberProduct} {p.Namee} {p.ProductPrice}");
            }

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            cts.Cancel();
        }

        static async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync("https://localhost:44356/api/Good");
            var test = await result.Content.ReadAsStringAsync();
            var r2 = await client.GetAsync("https://localhost:44356/api/Category");
            var t2 = await r2.Content.ReadAsStringAsync();
            Category[] categories = JsonConvert.DeserializeObject<Category[]>(t2);
            Product[] products = JsonConvert.DeserializeObject<Product[]>(test);
            if (categories.Select(c => c.CategoryName).Contains(callbackQuery.Data))
            {
                Category category = JsonConvert.DeserializeObject<Category[]>(t2).Where(q => q.CategoryName == callbackQuery.Data.ToString()).First();
                Product[] prod = JsonConvert.DeserializeObject<Product[]>(test).Where(x => x.IdCategories == category.IdCategories).ToArray();

                InlineKeyboardButton[][] keyboardButtons = new InlineKeyboardButton[prod.Length][];
                for (int i = 0; i < prod.Length; i++)
                {
                    keyboardButtons[i] = new InlineKeyboardButton[]
                    { InlineKeyboardButton.WithCallbackData(prod[i].Namee, callbackData: prod[i].Namee)};
                }

                var inlineKeyboard = new InlineKeyboardMarkup(keyboardButtons);
                await botClient.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id, "Электрички:", replyMarkup: inlineKeyboard);
            }
            if (products.Select(c => c.Namee).Contains(callbackQuery.Data))
            {
                botClient.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id, $"Вы купили {callbackQuery.Data}");
            }
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallbackQuery(botClient, update.CallbackQuery);
                return;
            }
            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}");

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"You said: \n{messageText}",
            cancellationToken: cancellationToken);

            if (message.Text == "Категории")
            {
                HttpClient client = new HttpClient();
                var result = await client.GetAsync("https://localhost:44356/api/Category");
                var test = await result.Content.ReadAsStringAsync();
                Category[] categories = JsonConvert.DeserializeObject<Category[]>(test);
                InlineKeyboardButton[][] keyboardButtons = new InlineKeyboardButton[categories.Length][];
                for (int i = 0; i < categories.Length; i++)
                {
                    keyboardButtons[i] = new InlineKeyboardButton[]
                    { InlineKeyboardButton.WithCallbackData(categories[i].CategoryName, callbackData: categories[i].CategoryName)};
                }

                var inlineKeyboard = new InlineKeyboardMarkup(keyboardButtons);
                await botClient.SendTextMessageAsync(message.Chat.Id, "Нажмите на кнопку:", replyMarkup: inlineKeyboard);
            }

            if (message.Text == "Когда сдашь лабораторные?")
            {
                await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Я всё сдам!",
                cancellationToken: cancellationToken);
            }
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
{
    new KeyboardButton[] { "Картинка", "Стикер", "Видео" },
    new KeyboardButton[] { "Категории" },
})
            {
                ResizeKeyboard = true
            };
            if (message.Text == "Привет")
            {
                await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "По заданию надо приветствовать Олега, но я все равно скажу тебе Привет!",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
            }
            if (message.Text == "Картинка")
            {
                await botClient.SendPhotoAsync(
    chatId: chatId,
    photo: "https://raw.githubusercontent.com/Accfora/Task8/main/wAS.jpg",
    caption: "<b>хочу черешню</b>",
    parseMode: ParseMode.Html,
    cancellationToken: cancellationToken);
            }
            if (message.Text == "Стикер")
            {
                await botClient.SendStickerAsync(
                    chatId: chatId,
                    sticker: "https://raw.githubusercontent.com/Accfora/Task8/main/thumb128.webp",
                    cancellationToken: cancellationToken);
            }
            if (message.Text == "Видео")
            {
                await botClient.SendVideoAsync(
    chatId: chatId,
    video: "https://raw.githubusercontent.com/Accfora/Task8/main/SelenaKilled.mp4",
    supportsStreaming: true,
    cancellationToken: cancellationToken);
            }
        }

        static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}", 
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}