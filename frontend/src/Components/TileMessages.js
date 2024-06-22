import { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCircle } from '@fortawesome/free-solid-svg-icons';
import { faInfoCircle, faEnvelope as envelopeSolid } from '@fortawesome/free-solid-svg-icons';
import { faEnvelopeOpen as envelopeRegular } from '@fortawesome/free-regular-svg-icons';
import {useNavigate} from "react-router-dom";

export default function TileMessages({heading, reviewComment, pagePath, statusClass, payload, isClicked,
                                         onClick}) {

    const navigate = useNavigate();
    const [showMessageBody, setShowMessageBody] = useState(false);

    function editProtocol() {
        if(payload === null){
            navigate(pagePath);
        } else {
            navigate(pagePath, { state: {payload}});
        }

    }

    function clickedProtocolHandler() {
        onClick();
    }


    function clickHandler() {
        setShowMessageBody(!showMessageBody);
        clickedProtocolHandler();
    }


    return (
        <>
            <div className="tile-message">
                <div className="message-head" onClick={clickHandler}>
                    <h3>
                        {payload && payload.isClosed ? (
                            <FontAwesomeIcon
                                icon={faInfoCircle}
                                style={{ marginRight: '10px' }}
                            />
                        ) : (
                            <FontAwesomeIcon
                                icon={isClicked ? envelopeRegular : envelopeSolid}
                                style={{ marginRight: '10px' }}
                            />
                        )}
                        {heading}{' '}
                         </h3>

                    <label>
                        Status: 

                    <FontAwesomeIcon icon={faCircle} size="2x" className={statusClass}/>
                    </label>


                </div>

                <div className={`message-body ${showMessageBody ? 'open' : ''}`} onClick={clickHandler} >
                    <p>
                        {reviewComment}
                    </p>
                </div>
                <button className="button" onClick={editProtocol}>Zum Protokoll</button>

            </div>


        </>
    )
}