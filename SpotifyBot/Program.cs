using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SpotifyTelegramBot
{
    public class SavedTrack
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double DurationSeconds { get; set; }
        public string SpotifyId { get; set; }
    }

    internal class Program
    {
        
        private static readonly string BotToken = "Мій токен";
        private static readonly string ApiUrl = "https://localhost:7178/api/tracks";

        private static Dictionary<long, List<SavedTrack>> _lastSearches = new Dictionary<long, List<SavedTrack>>();

        private static Dictionary<string, string> _artistBios = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Eminem", "🎙 **Eminem (Маршалл Метерз)** — американський репер, продюсер та актор. Один з найбільш продаваних музичних виконавців у світі. Володар 15 премій Греммі та Оскара за трек 'Lose Yourself'." },
            { "Rammstein", "🎸 **Rammstein** — культовий німецький індастріал-метал гурт, утворений у 1994 році. Відомі своїми епатажними текстами та неймовірними піротехнічними шоу на концертах." },
            { "The Weeknd", "🌃 **The Weeknd (Ейбел Тесфає)** — канадський співак, відомий своїм унікальним фальцетом та темним R&B звучанням. Його хіт 'Blinding Lights' побив рекорди чартів." },
            { "Linkin Park", "🤘 **Linkin Park** — легендарний американський рок-гурт. Вони поєднали ню-метал, реп та електроніку, ставши голосом цілого покоління нульових." },
            { "Dua Lipa", "🪩 **Dua Lipa** — британська поп-співачка. Її стиль поєднує сучасну поп-музику з диско-мотивами 80-х і 90-х." },
            { "Queen", "👑 **Queen** — легендарний британський рок-гурт на чолі з Фредді Мерк'юрі. Автори таких безсмертних гімнів як 'Bohemian Rhapsody' та 'We Will Rock You'." },
            { "Metallica", "⚡️ **Metallica** — піонери треш-металу, один з найуспішніших гуртів в історії рок-музики. Засновані у 1981 році в Лос-Анджелесі." },
            { "Nirvana", "🎸 **Nirvana** — культовий американський гранж-гурт на чолі з Куртом Кобейном. Їх пісня 'Smells Like Teen Spirit' змінила хід музичної історії." }
        };

        static async Task Main(string[] args)
        {
            var botClient = new TelegramBotClient(BotToken);
            var cts = new CancellationTokenSource();

            var me = await botClient.GetMe();
            Console.WriteLine($"Бот @{me.Username} успішно запущений! ");

            botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, null, cts.Token);
            Console.ReadLine();
            cts.Cancel();
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message != null && update.Message.Text != null)
            {
                await HandleMessageAsync(botClient, update.Message, cancellationToken);
            }
            else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
            {
                await HandleCallbackAsync(botClient, update.CallbackQuery, cancellationToken);
            }
        }

        static async Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            long chatId = message.Chat.Id;
            string text = message.Text;

            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "🔍 Пошук пісні", "🔥 Популярні артисти" },
                new KeyboardButton[] { "📚 Моя медіатека", "📖 Біографії" }
            })
            {
                ResizeKeyboard = true
            };

            if (text == "/start")
            {
                string welcome = "Привіт! Я твій музичний клієнт.\nОбери дію в меню нижче 👇";
                await botClient.SendMessage(chatId, welcome, replyMarkup: replyKeyboard, cancellationToken: cancellationToken);
                return;
            }

            if (text == "/mylibrary" || text == "📚 Моя медіатека")
            {
                await ShowLibraryAsync(botClient, chatId, cancellationToken);
                return;
            }

            if (text == "🔥 Популярні артисти")
            {
                var popularArtists = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton> {
                        InlineKeyboardButton.WithCallbackData("Eminem", "search|Eminem"),
                        InlineKeyboardButton.WithCallbackData("Rammstein", "search|Rammstein")
                    },
                    new List<InlineKeyboardButton> {
                        InlineKeyboardButton.WithCallbackData("The Weeknd", "search|The Weeknd"),
                        InlineKeyboardButton.WithCallbackData("Linkin Park", "search|Linkin Park")
                    },
                    new List<InlineKeyboardButton> {
                        InlineKeyboardButton.WithCallbackData("Queen", "search|Queen"),
                        InlineKeyboardButton.WithCallbackData("Metallica", "search|Metallica")
                    },
                    new List<InlineKeyboardButton> {
                        InlineKeyboardButton.WithCallbackData("Nirvana", "search|Nirvana"),
                        InlineKeyboardButton.WithCallbackData("Dua Lipa", "search|Dua Lipa")
                    }
                };

                var markup = new InlineKeyboardMarkup(popularArtists);
                await botClient.SendMessage(chatId, "Ось мій топ виконавців. Натисни на будь-кого для пошуку музики:", replyMarkup: markup, cancellationToken: cancellationToken);
                return;
            }

            if (text == "📖 Біографії")
            {
                var bioButtons = new List<List<InlineKeyboardButton>>();
                var keys = new List<string>(_artistBios.Keys);

                for (int i = 0; i < keys.Count; i += 2)
                {
                    var row = new List<InlineKeyboardButton>();
                    row.Add(InlineKeyboardButton.WithCallbackData(keys[i], $"bio|{keys[i]}"));
                    if (i + 1 < keys.Count)
                        row.Add(InlineKeyboardButton.WithCallbackData(keys[i + 1], $"bio|{keys[i + 1]}"));
                    bioButtons.Add(row);
                }

                var markup = new InlineKeyboardMarkup(bioButtons);
                await botClient.SendMessage(chatId, "Про кого хочеш дізнатися більше?", replyMarkup: markup, cancellationToken: cancellationToken);
                return;
            }

            if (text == "🔍 Пошук пісні")
            {
                await botClient.SendMessage(chatId, "Напиши мені ім'я виконавця або назву треку:", cancellationToken: cancellationToken);
                return;
            }

            await botClient.SendMessage(chatId, $"Шукаю треки для '{text}'... 🔍", cancellationToken: cancellationToken);
            await SearchTracksAsync(botClient, chatId, text, cancellationToken);
        }

        static async Task SearchTracksAsync(ITelegramBotClient botClient, long chatId, string query, CancellationToken cancellationToken)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"{ApiUrl}/search/{query}", cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var tracks = JsonConvert.DeserializeObject<List<SavedTrack>>(json);

                        if (tracks.Count == 0)
                        {
                            await botClient.SendMessage(chatId, "На жаль, нічого не знайдено 😔", cancellationToken: cancellationToken);
                            return;
                        }

                        _lastSearches[chatId] = tracks;

                        string replyText = $"🎶 Знайдено {tracks.Count} треків. Натисни кнопку під повідомленням, щоб зберегти:";
                        var inlineKeyboard = new List<List<InlineKeyboardButton>>();
                        for (int i = 0; i < tracks.Count; i++)
                        {
                            var button = InlineKeyboardButton.WithCallbackData($"💾 Зберегти: {tracks[i].Name}", $"save|{i}");
                            inlineKeyboard.Add(new List<InlineKeyboardButton> { button });
                        }

                        var markup = new InlineKeyboardMarkup(inlineKeyboard);
                        await botClient.SendMessage(chatId, replyText, replyMarkup: markup, cancellationToken: cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"Помилка API: {ex.Message}", cancellationToken: cancellationToken);
            }
        }

        static async Task HandleCallbackAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            long chatId = callbackQuery.Message.Chat.Id;
            string data = callbackQuery.Data;

            try
            {
                if (data != null && data.StartsWith("bio|"))
                {
                    string artist = data.Split('|')[1];
                    if (_artistBios.ContainsKey(artist))
                    {
                        await botClient.AnswerCallbackQuery(callbackQuery.Id, cancellationToken: cancellationToken);
                        await botClient.SendMessage(chatId, _artistBios[artist], parseMode: ParseMode.Markdown, cancellationToken: cancellationToken);
                    }
                }
                else if (data != null && data.StartsWith("search|"))
                {
                    string query = data.Split('|')[1];
                    await botClient.AnswerCallbackQuery(callbackQuery.Id, cancellationToken: cancellationToken);
                    await botClient.SendMessage(chatId, $"Шукаю треки для '{query}'... 🔍", cancellationToken: cancellationToken);
                    await SearchTracksAsync(botClient, chatId, query, cancellationToken);
                }
                else if (data != null && data.StartsWith("save|"))
                {
                    int index = int.Parse(data.Split('|')[1]);

                    if (_lastSearches.ContainsKey(chatId) && index < _lastSearches[chatId].Count)
                    {
                        var trackToSave = _lastSearches[chatId][index];

                        using (var httpClient = new HttpClient())
                        {
                            var jsonContent = JsonConvert.SerializeObject(trackToSave);
                            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                            var response = await httpClient.PostAsync(ApiUrl, content, cancellationToken);

                            if (response.IsSuccessStatusCode)
                                await botClient.AnswerCallbackQuery(callbackQuery.Id, $"✅ Успішно збережено!", cancellationToken: cancellationToken);
                            else
                                await botClient.AnswerCallbackQuery(callbackQuery.Id, $"❌ Помилка збереження", cancellationToken: cancellationToken);
                        }
                    }
                }
                else if (data != null && data.StartsWith("delete|"))
                {
                    string trackId = data.Split('|')[1];

                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.DeleteAsync($"{ApiUrl}/{trackId}", cancellationToken);

                        if (response.IsSuccessStatusCode)
                        {
                            await botClient.AnswerCallbackQuery(callbackQuery.Id, "✅ Видалено з бази!", cancellationToken: cancellationToken);
                            await botClient.DeleteMessage(chatId, callbackQuery.Message.MessageId, cancellationToken: cancellationToken);
                        }
                        else
                        {
                            
                            await botClient.AnswerCallbackQuery(callbackQuery.Id, "❌ Не вдалося видалити", cancellationToken: cancellationToken);
                        }
                    }
                }
            }
            catch
            {
            
                try { await botClient.AnswerCallbackQuery(callbackQuery.Id, "❌ Сталася помилка", cancellationToken: cancellationToken); } catch { }
            }
        }

        static async Task ShowLibraryAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(ApiUrl, cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var tracks = JsonConvert.DeserializeObject<List<SavedTrack>>(json);

                        if (tracks.Count == 0)
                        {
                            await botClient.SendMessage(chatId, "Твоя медіатека поки порожня.", cancellationToken: cancellationToken);
                            return;
                        }

                        double totalSeconds = 0;
                        foreach (var t in tracks) totalSeconds += t.DurationSeconds;
                        TimeSpan totalTime = TimeSpan.FromSeconds(totalSeconds);

                        string stats = $"📚 **Твої збережені треки:**\n" +
                                       $"📊 Всього пісень: {tracks.Count}\n" +
                                       $"⏱ Загальний час: {totalTime.Minutes} хв {totalTime.Seconds} сек";

                        await botClient.SendMessage(chatId, stats, cancellationToken: cancellationToken);

                        Random rnd = new Random();
                        int displayId = 1;

                        foreach (var track in tracks)
                        {
                            double fakeRating = Math.Round(rnd.NextDouble() * 1.2 + 3.8, 1);

                            string spotifyLink = $"https://open.spotify.com/track/{track.SpotifyId}";

                            string text = $"🎧 Пісня: {track.Name}\n" +
                                          $"🆔 Номер: {displayId}\n" +
                                          $"⭐️ Рейтинг: {fakeRating}/5.0\n" +
                                          $"{spotifyLink}";

                            
                            string safeIdForDelete = !string.IsNullOrEmpty(track.Id) ? track.Id : track.SpotifyId;

                            var button = InlineKeyboardButton.WithCallbackData("❌ Видалити", $"delete|{safeIdForDelete}");
                            var markup = new InlineKeyboardMarkup(button);

                            await botClient.SendMessage(chatId, text, replyMarkup: markup, parseMode: ParseMode.Markdown, cancellationToken: cancellationToken);

                            displayId++;
                        }
                    }
                }
            }
            catch
            {
                await botClient.SendMessage(chatId, "Помилка доступу до БД.", cancellationToken: cancellationToken);
            }
        }

        static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Помилка Telegram API: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}