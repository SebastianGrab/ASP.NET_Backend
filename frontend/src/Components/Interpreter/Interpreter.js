import Tile from '../Tile';
import { InputElement } from './InputElement';
import TileCategory from './TileCategory';
import { saveProtocol } from '../../API/archive/saveProtocol';
import { useState, useEffect } from "react";
import { DropDownWithData } from '../DropDownWithData';
import { Box } from '@mui/material';

export default function Interpreter({ schema }) {

    const renderCategories = (categories) => {

        return (
            <>

                {categories.map((category) => (

                    <TileCategory key={category.ID} category={category} />

                ))
                }

                <Box mb={10}/>

                
            </>

        );

    };





    return (
        <>

            {renderCategories(schema.Schema)}


        </>
    );


};
