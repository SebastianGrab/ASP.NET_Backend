import React from "react";

export const UpdateProtocolButtons = ({handleShowDialog, handleSave}) => {

    return(
        <>
            <div className="row" style={{marginBottom: "10px"}}>
    <input
      className="button"
      value="Protokoll absenden!"
      type="button"
      onClick={handleShowDialog}
      style={{display: "flex",
        justifyContent: "flex-start",
        marginRight: `8.333%`,
      }}
    ></input>
            <input
      className="button-scnd"
      value="Protokoll speichern!"
      type="button"
      onClick={handleSave}
      style={{display: "flex",
        justifyContent: "flex-start",
        marginRight: `8.333%`,
      }}
    ></input>
  </div>
        </>
    )



}
