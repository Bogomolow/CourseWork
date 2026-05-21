using CourseWork7.Models;
using Newtonsoft.Json;

namespace CourseWork7.Services
{
    
    public class SpotifyService
    {
        private readonly HttpClient _httpClient;

        public SpotifyService()
        {
            _httpClient = new HttpClient();
        }

       
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
                
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

               
                var spotifyData = JsonConvert.DeserializeObject<SpotifySearchResponse.Rootobject>(body);

                if (spotifyData?.tracks?.items != null)
                {
                    var allTracks = spotifyData.tracks.items;

                    
                    var filteredAndSorted = allTracks
                        .Where(track => track.data.duration.totalMilliseconds > 180000)
                        .OrderBy(track => track.data.name)
                        .ToList();

                    
                    var resultList = new List<SavedTrack>();

                    foreach (var item in filteredAndSorted)
                    {
                        resultList.Add(new SavedTrack
                        {
                            
                            SpotifyId = item.data.id,
                            Name = item.data.name,
                            DurationSeconds = Math.Round(item.data.duration.totalMilliseconds / 1000.0, 1)
                        });
                    }

                    return resultList;
                }

                
                return new List<SavedTrack>();
            }
        }
    }
}