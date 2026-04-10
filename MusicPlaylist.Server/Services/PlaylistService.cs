using Microsoft.EntityFrameworkCore;
using MusicPlaylist.Server.Data;
using MusicPlaylist.Server.DTOs;
using MusicPlaylist.Server.Models;

namespace MusicPlaylist.Server.Services
{
    public class PlaylistService
    {
        public readonly PlaylistDb _db;

        public PlaylistService(PlaylistDb db)
        {
            this._db = db;
        }

        public async Task<IEnumerable<Playlist>> GetPlaylistsAsync(CancellationToken ct)
        {
            var playlists = await _db.Playlists.Include(p => p.Tracks).ToListAsync(ct);

            return playlists;
        }

        public async Task<Playlist> GetPlaylistByIdAsync(int id, CancellationToken ct)
        {
            var playlist = await _db.Playlists.Include(p => p.Tracks).SingleOrDefaultAsync(p => p.Id == id, ct);

            return playlist;
        }

        public async Task<Playlist> CreatePlaylistAsync(PlaylistDTO newPlaylist, CancellationToken ct)
        {
            var playlist = new Playlist { Title = newPlaylist.Title };

            _db.Playlists.Add(playlist);

            await _db.SaveChangesAsync(ct);

            return playlist;
        }

        public async Task<bool> UpdatePlaylistAsync(int id, Playlist updatedPlaylist, CancellationToken ct)
        {
            var playlist = await _db.Playlists.FindAsync(new object[] { id }, ct);

            if (playlist == null)
                return false;

            _db.Attach(playlist);

            playlist.Title = updatedPlaylist.Title;

            var result = await _db.SaveChangesAsync(ct);

            return result > 0;
        }

        public async Task<bool> DeletePlaylistAsync(int id, CancellationToken ct)
        {
            var playlist = await _db.Playlists.FindAsync(new object[] { id }, ct);

            if (playlist == null)
                return false;

            _db.Playlists.Remove(playlist);

            var result = await _db.SaveChangesAsync(ct);

            return result > 0;
        }
    }
}
