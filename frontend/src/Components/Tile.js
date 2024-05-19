import { useNavigate } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

export default function Tile({ pagePath, icon, description }) {

    const navigate = useNavigate();

    function clickHandler() {
        navigate(pagePath);
    }


    return (
        <>
            <div className="tile" onClick={clickHandler}>

                <FontAwesomeIcon icon={icon} size="2x" className='tile-icon'/>
                <h3>{description}</h3>
            </div>

        </>
    )
}