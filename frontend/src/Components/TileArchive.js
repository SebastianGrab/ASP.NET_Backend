import { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCircle } from '@fortawesome/free-solid-svg-icons';
import { useNavigate } from 'react-router-dom';

export default function TileArchive() {
    const [pdfUrl, setPdfUrl] = useState('/HvOProtokoll.pdf');

    function openPdf(){
        window.open(pdfUrl, '_blank');
    }

    return (
        <>
            <div className="tile-message">
                <div className="message-head">
                    <h3>Protokoll X</h3>
                </div>
                <div className="archive-body">
                    <body>
                        <p>Archiviert am: </p>
                        <p>Löschung am: </p>

                    </body>
                    <div className="button-container">
                    <button className="button" onClick={openPdf}>PDF öffnen</button>
                    </div>
                </div>
            </div>


        </>
    )
}