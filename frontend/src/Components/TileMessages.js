import { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCircle } from '@fortawesome/free-solid-svg-icons';
import {useNavigate} from "react-router-dom";

export default function TileMessages({heading, status, pagePath}) {
    const head = {heading};
    const statuss = {status};
    const navigate = useNavigate();

    let message = 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren';

    const [showMessageBody, setShowMessageBody] = useState(false);



    function handlePiP() {
        navigate(pagePath);
    }
    function clickHandler() {
        setShowMessageBody(!showMessageBody);
    }


    return (
        <>
            <div className="tile-message">
                <div className="message-head" onClick={clickHandler}>
                    <h3> {heading} </h3>

                    <label>
                        Status: 

                    <FontAwesomeIcon icon={faCircle} size="2x" className={status}/>
                    </label>


                </div>

                <div className={`message-body ${showMessageBody ? 'open' : ''}`} onClick={clickHandler}>
                    {message}
                </div>
                <button className="button" onClick={handlePiP}>Zum Protokoll</button>

            </div>


        </>
    )
}