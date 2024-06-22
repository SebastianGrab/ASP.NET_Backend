import { useNavigate } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

export default function Tile({ pagePath, icon, description, info, payload, highlight }) {

    const navigate = useNavigate();

    function clickHandler() {
        if(payload === null){
            navigate(pagePath);
        } else {
            navigate(pagePath, { state: {payload}});
        }

    }


    return (
        <>
            <div className={`tile ${highlight ? 'highlight' : ''}`} onClick={clickHandler}>

                <FontAwesomeIcon icon={icon} size="2x" className='tile-icon'/>
                <h3>{description}</h3>
                <p>{info}</p>
            </div>

        </>
    )
}