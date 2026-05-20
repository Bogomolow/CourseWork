using Microsoft.AspNetCore.Mvc;
using CourseWork7.Models;
using CourseWork7.Services;
using CourseWork7.Storage;

namespace CourseWork7.Controllers
{
    // Атрибути, які вказують, що це API-контролер, і шлях до нього буде /api/tracks
    [ApiController]
    [Route("api/[controller]")]
    public class TracksController : ControllerBase
    {
        private readonly SpotifyService _spotifyService;
        private readonly TrackStorage _storage;

        public TracksController()
        {
            // Створюємо екземпляри наших класів логіки та бази даних
            _spotifyService = new SpotifyService();
            _storage = new TrackStorage();
        }

        // ==========================================
        // 1. РОБОТА З ПУБЛІЧНИМ API (Spotify)
        // ==========================================

        // GET: api/tracks/search/Eminem
        [HttpGet("search/{query}")]
        public async Task<ActionResult<List<SavedTrack>>> SearchSpotify(string query)
        {
            var tracks = await _spotifyService.SearchAndFilterTracksAsync(query);

            if (tracks.Count == 0)
            {
                return NotFound("Треки не знайдені у Spotify."); // Повертаємо 404
            }

            return Ok(tracks); // Повертаємо 200 OK і список треків
        }

        // ==========================================
        // 2. РОБОТА З ВЛАСНИМИ ДАНИМИ (CRUD - JSON)
        // ==========================================

        // READ ALL (Отримати всі збережені треки)
        // GET: api/tracks
        [HttpGet]
        public ActionResult<List<SavedTrack>> GetAllSaved()
        {
            var tracks = _storage.GetAll();
            return Ok(tracks); // 200 OK
        }

        // READ ONE (Отримати один збережений трек за ID)
        // GET: api/tracks/{id}
        [HttpGet("{id}")]
        public ActionResult<SavedTrack> GetSavedById(string id)
        {
            var tracks = _storage.GetAll();
            var track = tracks.FirstOrDefault(t => t.Id == id);

            if (track == null)
            {
                return NotFound("Трек не знайдено у локальній базі."); // 404
            }

            return Ok(track); // 200 OK
        }

        // CREATE (Зберегти новий трек у файл)
        // POST: api/tracks
        [HttpPost]
        public ActionResult Create([FromBody] SavedTrack newTrack)
        {
            _storage.Add(newTrack);
            return StatusCode(201); // 201 Created (як вимагає методичка!)
        }

        // UPDATE (Оновити існуючий трек)
        // PUT: api/tracks/{id}
        [HttpPut("{id}")]
        public ActionResult Update(string id, [FromBody] SavedTrack updatedTrack)
        {
            var tracks = _storage.GetAll();
            var track = tracks.FirstOrDefault(t => t.Id == id);

            if (track == null) return NotFound("Трек для оновлення не знайдено.");

            // Логіка оновлення: видаляємо старий запис, ставимо новий, але зберігаємо оригінальний ID
            _storage.Delete(id);
            updatedTrack.Id = id;
            _storage.Add(updatedTrack);

            return Ok("Трек успішно оновлено!"); // 200 OK
        }

        // DELETE (Видалити трек)
        // DELETE: api/tracks/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var tracks = _storage.GetAll();

            if (!tracks.Any(t => t.Id == id))
            {
                return NotFound("Трек для видалення не знайдено.");
            }

            _storage.Delete(id);
            return NoContent(); // 204 No Content (успішне видалення без повернення тіла, вимога методички!)
        }
    }
}