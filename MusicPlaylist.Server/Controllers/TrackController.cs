using Microsoft.AspNetCore.Mvc;
using MusicPlaylist.Server.Models;
using MusicPlaylist.Server.Services;

namespace MusicPlaylist.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackController : ControllerBase
    {
        public readonly TrackService _trackService;

        public TrackController(TrackService trackService)
        {
            this._trackService = trackService;
        }

        [HttpGet("Tracks")]
        public async Task<IActionResult> GetTracks(CancellationToken ct)
        {
            var tracks = await _trackService.GetTracksAsync(ct);

            return Ok(tracks);
        }

        [HttpGet("Tracks/{id}")]
        public async Task<IActionResult> GetTrackById(int id, CancellationToken ct)
        {
            var track = await _trackService.GetTrackByIdAsync(id, ct);

            if (track == null)
                return NotFound();

            return Ok(track);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateTrack(Track newTrack, CancellationToken ct)
        {
            var track = await _trackService.CreateTrackAsync(newTrack, ct);

            return Created($"/api/tracks/{track.Id}", track);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateTrack(int id, Track updatedTrack, CancellationToken ct)
        {
            var result = await _trackService.UpdateTrackAsync(id, updatedTrack, ct);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteTrack(int id, CancellationToken ct)
        {
            var result = await _trackService.DeleteTrackAsync(id, ct);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
