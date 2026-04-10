using Microsoft.AspNetCore.Mvc;
using MusicPlaylist.Server.DTOs;
using MusicPlaylist.Server.Models;
using MusicPlaylist.Server.Services;

namespace MusicPlaylist.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        public readonly PlaylistService _playlistService;

        public PlaylistController(PlaylistService playlistService)
        {
            this._playlistService = playlistService;
        }

        [HttpGet("Playlists")]
        public async Task<IActionResult> GetPlaylists(CancellationToken ct)
        {
            var playlists = await _playlistService.GetPlaylistsAsync(ct);

            return Ok(playlists);
        }

        [HttpGet("Playlists/{id}")]
        public async Task<IActionResult> GetPlaylistById(int id, CancellationToken ct)
        {
            var playlist = await _playlistService.GetPlaylistByIdAsync(id, ct);

            if (playlist == null)
                return NotFound();

            return Ok(playlist);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreatePlaylist(PlaylistDTO newPlaylist, CancellationToken ct)
        {
            var playlist = await _playlistService.CreatePlaylistAsync(newPlaylist, ct);

            return Created($"/api/playlists/{playlist.Id}", playlist);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdatePlaylist(int id, Playlist updatedPlaylist, CancellationToken ct)
        {
            var result = await _playlistService.UpdatePlaylistAsync(id, updatedPlaylist, ct);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePlaylist(int id, CancellationToken ct)
        {
            var result = await _playlistService.DeletePlaylistAsync(id, ct);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
