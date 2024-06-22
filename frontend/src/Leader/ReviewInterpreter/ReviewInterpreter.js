import React from "react";
import ReviewTileCategory from "./ReviewTileCategory";
import { Box } from "@mui/material";

export default function ReviewInterpreter({ schema }) {
  const renderCategories = (categories) => {
    return (
      <>
        {categories.map((category) => (
          <ReviewTileCategory key={category.ID} category={category} />
        ))}

        <Box mb={15} />
      </>
    );
  };

  return <>{renderCategories(schema.Schema)}</>;
}
