using System;

namespace CourseWork7.Models
{
    // Цей клас описує те, як трек буде зберігатися у нашому власному JSON-файлі
    public class SavedTrack
    {
        // Унікальний ідентифікатор у нашій базі (генерується автоматично)
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Оригінальний ID зі Spotify, щоб ми знали, звідки цей трек
        public string SpotifyId { get; set; }

        public string Name { get; set; }

        public double DurationSeconds { get; set; }
    }
}