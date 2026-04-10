using Microsoft.EntityFrameworkCore;
using MusicPlaylist.Server.Models;
namespace MusicPlaylist.Server.Data;

public class PlaylistDb : DbContext
{
    public PlaylistDb(DbContextOptions<PlaylistDb> options) : base(options) {}

    public DbSet<Playlist> Playlists { get; set; }

    public DbSet<Track> Tracks { get; set; }
}