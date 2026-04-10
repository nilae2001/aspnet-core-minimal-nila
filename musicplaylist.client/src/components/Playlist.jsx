export default function Playlist({ playlist, children }) {
  return (
    <div className="accordion-item">
      <h2 className="accordion-header">
        <button
          className="accordion-button collapsed"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target={`#${playlist.id}`}
        >
          {playlist.title}
        </button>
      </h2>
      <div
        id={playlist.id}
        className="accordion-collapse collapse"
        data-bs-parent="#playlists"
      >
        {playlist.tracks?.map((track) => (
          <p key={track.id}>{track.title}</p>
        ))}
      </div>
    </div>
  );
}
