import { useEffect, useState } from "react";
import { Toaster, toast } from "sonner";
import Playlist from "./components/Playlist";
import PlaylistForm from "./components/PlaylistForm";
import "./App.css";

function App() {
  const [playlists, setPlaylists] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState();

  const addPlaylist = async (playlist) => {
    const response = await fetch("/api/playlists", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(playlist),
    });

    if (!response.ok) {
      toast("Failed to create playlist");
      return;
    }

    const data = await response.json();

    setPlaylists([data, ...playlists]);

    toast("Successfully created playlist");
  };

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);

      try {
        var response = await fetch("/api/playlist");
        if (!response.ok) {
          throw new Error("Something went wrong");
        }

        const data = await response.json();
        setPlaylists(data);
        toast("Finished loading data");
      } catch (err) {
        toast(`Error: ${err}`);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) return <p>Loading...</p>;

  if (error) return <p>{error}</p>;

  return (
    <>
      <Toaster position="top-right" richColors />
      <h1>My Playlist</h1>
      <PlaylistForm onAdd={addPlaylist} />
      <div className="accordion" id="playlists">
        {playlists.map((playlist) => (
          <Playlist key={playlist.id} playlist={playlist} />
        ))}
      </div>
    </>
  );
}

export default App;
