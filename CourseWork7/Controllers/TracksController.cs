using Microsoft.AspNetCore.Mvc;
using CourseWork7.Models;
using CourseWork7.Services;
using CourseWork7.Storage;

namespace CourseWork7.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class TracksController : ControllerBase
    {
        private readonly SpotifyService _spotifyService;
        private readonly TrackStorage _storage;

        public TracksController()
        {
            
            _spotifyService = new SpotifyService();
            _storage = new TrackStorage();
        }

        

        
        [HttpGet("search/{query}")]
        public async Task<ActionResult<List<SavedTrack>>> SearchSpotify(string query)
        {
            var tracks = await _spotifyService.SearchAndFilterTracksAsync(query);

            if (tracks.Count == 0)
            {
                return NotFound("Треки не знайдені у Spotify."); 
            }

            return Ok(tracks); 

        
        [HttpGet]
        public ActionResult<List<SavedTrack>> GetAllSaved()
        {
            var tracks = _storage.GetAll();
            return Ok(tracks); 
        }

        
        [HttpGet("{id}")]
        public ActionResult<SavedTrack> GetSavedById(string id)
        {
            var tracks = _storage.GetAll();
            var track = tracks.FirstOrDefault(t => t.Id == id);

            if (track == null)
            {
                return NotFound("Трек не знайдено у локальній базі."); 
            }

            return Ok(track);
        }

        
        [HttpPost]
        public ActionResult Create([FromBody] SavedTrack newTrack)
        {
            _storage.Add(newTrack);
            return StatusCode(201); 
        }

        
        [HttpPut("{id}")]
        public ActionResult Update(string id, [FromBody] SavedTrack updatedTrack)
        {
            var tracks = _storage.GetAll();
            var track = tracks.FirstOrDefault(t => t.Id == id);

            if (track == null) return NotFound("Трек для оновлення не знайдено.");

            
            _storage.Delete(id);
            updatedTrack.Id = id;
            _storage.Add(updatedTrack);

            return Ok("Трек успішно оновлено!"); 
        }

        
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var tracks = _storage.GetAll();

            if (!tracks.Any(t => t.Id == id))
            {
                return NotFound("Трек для видалення не знайдено.");
            }

            _storage.Delete(id);
            return NoContent(); 
        }
    }
}