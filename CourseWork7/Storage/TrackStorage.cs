using CourseWork7.Models;
using Newtonsoft.Json;

namespace CourseWork7.Storage
{
    
    public class TrackStorage
    {
        
        private readonly string _filePath = "saved_tracks.json";

       
        public List<SavedTrack> GetAll()
        {
            
            if (!File.Exists(_filePath))
            {
                return new List<SavedTrack>();
            }

            
            string json = File.ReadAllText(_filePath);

            
            return JsonConvert.DeserializeObject<List<SavedTrack>>(json) ?? new List<SavedTrack>();
        }

        
        public void Add(SavedTrack track)
        {
            
            var tracks = GetAll();

          
            tracks.Add(track);

            
            string json = JsonConvert.SerializeObject(tracks, Formatting.Indented);

           
            File.WriteAllText(_filePath, json);
        }

        
        public void Delete(string id)
        {
            var tracks = GetAll();

           
            var trackToRemove = tracks.FirstOrDefault(t => t.Id == id);

            if (trackToRemove != null)
            {
                tracks.Remove(trackToRemove); 

                
                string json = JsonConvert.SerializeObject(tracks, Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
        }
    }
}