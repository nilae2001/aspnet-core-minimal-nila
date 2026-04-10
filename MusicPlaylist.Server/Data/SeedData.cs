using Microsoft.EntityFrameworkCore;
using MusicPlaylist.Server.Models;

namespace MusicPlaylist.Server.Data;

public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = serviceProvider.GetRequiredService<PlaylistDb>();

        if (context is null)
        {
            throw new InvalidOperationException("Playlist context not defined");
        }

        if (context.Playlists.Any())
        {
            return; // db already seeded
        }

        context.AddRange(new List<Playlist>{
            new Playlist { Id = 1, Title = "Classics" },
            new Playlist { Id = 2, Title = "Throwback" },
        });

        context.AddRange(new List<Track>
        {
            new Track { Id = 1, Title = "Bohemian Rhapsody", Artist = "Queen", PlaylistId = 2 },
            new Track { Id = 2, Title = "Billy Jean", Artist = "Michael Jackson", PlaylistId = 2 },
            new Track { Id = 3, Title = "Uptown Girl", Artist = "Billy Joel", PlaylistId = 1 }
        });

        context.SaveChanges();
    }
}