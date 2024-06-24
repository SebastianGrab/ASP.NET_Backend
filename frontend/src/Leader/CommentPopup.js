import { useState } from "react";

export default function CommentPopup({ handleSendBack, handleClose }) {
    const [comment, setComment] = useState("");


    const handleCommentChange = (e) => {
        setComment(e.target.value);
    };

    const handleSendBackWithComment = () => {
        if (comment.trim() !== "") { // Prüfe, ob das Kommentarfeld nicht leer oder nur Leerzeichen enthält
            handleSendBack(comment);
            setComment(""); // Setze das Kommentarfeld zurück
        }
    };

    const isSubmitDisabled = comment.trim() === "";

    return (
        <div className="dialog">
            <div className="popup-inner">
                <h2>Kommentar hinzufügen</h2>
                <textarea
                    value={comment}
                    onChange={handleCommentChange}
                    placeholder="Geben Sie den Grund für die Überarbeitung ein..."
                    required
                />
                <div className="row">
                    <button className="button" onClick={handleSendBackWithComment} disabled={isSubmitDisabled}>Absenden</button>
                    <button className="button" onClick={handleClose}>Abbrechen</button>
                </div>
            </div>
        </div>
    );
}
