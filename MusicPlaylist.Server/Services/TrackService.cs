using Microsoft.EntityFrameworkCore;
using MusicPlaylist.Server.Data;
using MusicPlaylist.Server.Models;

namespace MusicPlaylist.Server.Services
{
    public class TrackService
    {
        public readonly PlaylistDb _db;

        public TrackService(PlaylistDb db)
        {
            this._db = db;
        }

        public async Task<IEnumerable<Track>> GetTracksAsync(CancellationToken ct)
        {
            var tracks = await _db.Tracks.Include(t => t.Playlist).ToListAsync(ct);

            return tracks;
        }

        public async Task<Track> GetTrackByIdAsync(int id, CancellationToken ct)
        {
            var track = await _db.Tracks.Include(t => t.Playlist).SingleOrDefaultAsync(t => t.Id == id, ct);

            return track;
        }

        public async Task<Track> CreateTrackAsync(Track newTrack, CancellationToken ct)
        {
            _db.Tracks.Add(newTrack);

            await _db.SaveChangesAsync(ct);

            return newTrack;
        }

        public async Task<bool> UpdateTrackAsync(int id, Track updatedTrack, CancellationToken ct)
        {
            var track = await _db.Tracks.FindAsync(new object[] { id }, ct);

            if (track == null)
                return false;

            _db.Attach(track);

            track.Title = updatedTrack.Title;

            track.Artist = updatedTrack.Artist;

            var result = await _db.SaveChangesAsync(ct);

            return result > 0;
        }

        public async Task<bool> DeleteTrackAsync(int id, CancellationToken ct)
        {
            var track = await _db.Tracks.FindAsync(new object[] { id }, ct);

            if (track == null)
                return false;

            _db.Tracks.Remove(track);

            var result = await _db.SaveChangesAsync(ct);

            return result > 0;
        }
    }
}
