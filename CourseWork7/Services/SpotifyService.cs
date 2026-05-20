using CourseWork7.Models;
using Newtonsoft.Json;

namespace CourseWork7.Services
{
    // Цей клас відповідає виключно за похід в інтернет до Spotify API
    public class SpotifyService
    {
        private readonly HttpClient _httpClient;

        public SpotifyService()
        {
            _httpClient = new HttpClient();
        }

        // Метод, який приймає запит (наприклад, "Eminem") і повертає готовий список наших треків
        public async Task<List<SavedTrack>> SearchAndFilterTracksAsync(string query)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://spotify23.p.rapidapi.com/search/?q={query}&type=tracks&offset=0&limit=15"),
                Headers =
                {
                    { "x-rapidapi-key", "73973b8ce1msh0eae9a7f2527eb5p1b3277jsnfad9c785be32" },
                    { "x-rapidapi-host", "spotify23.p.rapidapi.com" },
                },
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                // Якщо помилка (наприклад 429) - викидаємо виключення
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                // Десеріалізуємо велику і складну відповідь від Spotify
                var spotifyData = JsonConvert.DeserializeObject<SpotifySearchResponse.Rootobject>(body);

                if (spotifyData?.tracks?.items != null)
                {
                    var allTracks = spotifyData.tracks.items;

                    // ТВОЯ ЛОГІКА З 6-Ї ЛАБИ: Фільтруємо (> 3 хв) та сортуємо
                    var filteredAndSorted = allTracks
                        .Where(track => track.data.duration.totalMilliseconds > 180000)
                        .OrderBy(track => track.data.name)
                        .ToList();

                    // МАПІНГ: Перетворюємо складну модель Spotify у нашу просту і зручну модель SavedTrack
                    var resultList = new List<SavedTrack>();

                    foreach (var item in filteredAndSorted)
                    {
                        resultList.Add(new SavedTrack
                        {
                            // Id згенерується автоматично, його не чіпаємо
                            SpotifyId = item.data.id,
                            Name = item.data.name,
                            DurationSeconds = Math.Round(item.data.duration.totalMilliseconds / 1000.0, 1)
                        });
                    }

                    return resultList;
                }

                // Якщо нічого не знайшли, повертаємо порожній список
                return new List<SavedTrack>();
            }
        }
    }
}