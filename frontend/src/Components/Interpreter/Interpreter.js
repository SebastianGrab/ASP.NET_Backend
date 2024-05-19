import Tile from '../Tile';
import InputElement from './InputElement';
import TileCategory from './TileCategory';
import { saveProtocol } from '../../API/saveProtocol';
import { useState, useEffect } from "react";

export default function Interpreter({ schema }) {

    const [showDialog, setShowDialog] = useState(false);




    const renderCategories = (categories) => {

        return (
            <>

                {categories.map((category) => (

                    <TileCategory key={category.ID} category={category} />

                ))
                }
            </>

        );

    };





    return (
        <>

            {renderCategories(schema.Schema)}


        </>
    );


};