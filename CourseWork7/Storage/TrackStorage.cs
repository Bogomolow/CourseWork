using CourseWork7.Models;
using Newtonsoft.Json;

namespace CourseWork7.Storage
{
    // Цей клас відповідає виключно за роботу з файлом "бази даних"
    public class TrackStorage
    {
        // Назва файлу, який створиться прямо в папці з проєктом
        private readonly string _filePath = "saved_tracks.json";

        // READ: Отримати всі збережені треки з файлу
        public List<SavedTrack> GetAll()
        {
            // Якщо файлу ще немає (ми ще нічого не зберігали), просто повертаємо порожній список
            if (!File.Exists(_filePath))
            {
                return new List<SavedTrack>();
            }

            // Читаємо весь текст з файлу
            string json = File.ReadAllText(_filePath);

            // Перетворюємо JSON-текст назад у список об'єктів SavedTrack
            return JsonConvert.DeserializeObject<List<SavedTrack>>(json) ?? new List<SavedTrack>();
        }

        // CREATE: Додати новий трек у файл
        public void Add(SavedTrack track)
        {
            // 1. Витягуємо всі існуючі треки
            var tracks = GetAll();

            // 2. Додаємо до них новий трек
            tracks.Add(track);

            // 3. Перетворюємо оновлений список у красивий JSON-текст
            string json = JsonConvert.SerializeObject(tracks, Formatting.Indented);

            // 4. Перезаписуємо файл
            File.WriteAllText(_filePath, json);
        }

        // DELETE: Видалити трек за його ID
        public void Delete(string id)
        {
            var tracks = GetAll();

            // Шукаємо трек, у якого Id збігається з тим, що ми передали
            var trackToRemove = tracks.FirstOrDefault(t => t.Id == id);

            if (trackToRemove != null)
            {
                tracks.Remove(trackToRemove); // Видаляємо зі списку

                // Перезаписуємо файл без цього треку
                string json = JsonConvert.SerializeObject(tracks, Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
        }
    }
}