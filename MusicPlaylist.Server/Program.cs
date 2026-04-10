using Microsoft.EntityFrameworkCore;
using MusicPlaylist.Server.Models;
using MusicPlaylist.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<PlaylistDb>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("playlistdb")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

app.MapGet("/api/playlist", async (PlaylistDb db) =>
{
    return await db.Playlists.Include(p => p.Tracks).ToListAsync();
});

app.MapGet("/api/playlist/{id}", async (int id, PlaylistDb db) =>
{
    return await db.Playlists.Include(p => p.Tracks).SingleOrDefaultAsync(p => p.Id == id) is Playlist playlist ? Results.Ok(playlist) : Results.NotFound();
});

app.MapPost("/api/playlists", async (PlaylistDTO newPlaylist, PlaylistDb db) =>
{
    var playlist = new Playlist { Title = newPlaylist.Title };

    db.Playlists.Add(playlist);

    await db.SaveChangesAsync();

    return Results.Created($"/api/playlists/{playlist.Id}", playlist);
});

app.MapPut("/api/playlist/{id}", async (int id, Playlist updatedPlaylist, PlaylistDb db) =>
{
    var playlist = await db.Playlists.FindAsync(id);

    if (playlist is null) return Results.NotFound();

    db.Attach(playlist);

    playlist.Title = updatedPlaylist.Title;
    
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/api/playlists/{id}", async (int id, PlaylistDb db) => {
    if (await db.Playlists.FindAsync(id) is Playlist playlist) {
        db.Playlists.Remove(playlist);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.MapGet("/api/tracks", async (PlaylistDb db) => {

    return await db.Tracks.Include(t => t.Playlist).ToListAsync();
});

app.MapGet("/api/tracks/{id}", async (int id, PlaylistDb db) => {
    return await db.Tracks.Include(t => t.Playlist).SingleOrDefaultAsync(t => t.Id == id) is Track track ? Results.Ok(track) : Results.NotFound();
});

app.MapPost("/api/tracks", async (Track track, PlaylistDb db) => {
    // Verify the playlist exists
    var playlistExists = await db.Playlists.AnyAsync(p => p.Id == track.PlaylistId);
    if (!playlistExists)
        return Results.BadRequest($"Playlist with ID {track.PlaylistId} does not exist.");

    db.Tracks.Add(track);
    await db.SaveChangesAsync();
    return Results.Created($"/api/tracks/{track.Id}", track);
});

app.MapPut("/api/tracks/{id}", async (int id, Track updatedTrack, PlaylistDb db) => {

    var track = await db.Tracks.FindAsync(id);

    if (track is null) return Results.NotFound();

    track.Title = updatedTrack.Title;

    track.Artist = updatedTrack.Artist;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/api/tracks/{id}", async(int id, PlaylistDb db) => {

    if( await db.Tracks.FindAsync(id) is Track track) {
        db.Tracks.Remove(track);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowLocalhost");

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
