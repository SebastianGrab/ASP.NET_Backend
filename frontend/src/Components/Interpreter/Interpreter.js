import TileCategory from './TileCategory';
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
