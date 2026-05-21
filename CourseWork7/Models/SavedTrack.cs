using System;

namespace CourseWork7.Models
{
    
    public class SavedTrack
    {
        
        public string Id { get; set; } = Guid.NewGuid().ToString();

        
        public string SpotifyId { get; set; }

        public string Name { get; set; }

        public double DurationSeconds { get; set; }
    }
}