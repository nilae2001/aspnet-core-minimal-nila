import { useState, useRef } from "react";

export default function PlaylistForm({ onAdd }) {
  const [title, setTitle] = useState("");

  const dialogRef = useRef();

  const openDialog = () => dialogRef.current?.showModal();

  const closeDialog = () => dialogRef.current?.close();

  const handleSubmit = (e) => {
    e.preventDefault();

    onAdd({ title });

    setTitle("");

    closeDialog();
  };

  return (
    <>
      <button className="btn btn-primary" onClick={openDialog}>
        New Playlist
      </button>

      <dialog ref={dialogRef}>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label className="form-label">Title</label>
            <input
              className="form-control"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
            />
            <div className="hstack gap-3">
              <button className="btn btn-success">Save</button>
              <button
                className="btn btn-danger"
                onClick={closeDialog}
                type="button"
              >
                Cancel
              </button>
            </div>
          </div>
        </form>
      </dialog>
    </>
  );
}
