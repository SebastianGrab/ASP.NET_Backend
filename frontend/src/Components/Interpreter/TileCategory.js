import { useState, useEffect } from "react";
import { InputElement } from "./InputElement";
import { DropdownElementHelper } from "./DropdownElementHelper";
import DropdownElement from "./DropdownElement";
import InTileLabel from "./InTileLabel";
import Textarea from "./Textarea";
import { Mandatoryhandler } from "./InputElement";
import { Divider, Box } from "@mui/material";

export default function TileCategory({ category }) {

  const inputIDs = [];
  const [showInputs, setShowMessageBody] = useState(false);
  const [numberOfInputs, setNumberOfInputs] = useState(0);
  const [numberOfValues, setNumberOfValues] = useState(0);

  const style = {
    backgroundColor: category.Marking === true ? "red" : "initial",
  };

  function showBody() {
    setShowMessageBody(true);
  }

  function hideBody() {
    setShowMessageBody(false);
  }

  const getInputs = () => {
    setNumberOfInputs(category.Inputs.length);

    category.Inputs.forEach((input) => {
      if (input.Value) {
        setNumberOfValues(numberOfValues + 1);
        //console.log(input);
      }
    });
  };


  useEffect(() => {
    getInputs();
  }, []);

  function handleSaveSection() {
    const inputValues = {};

    // Durchgehe alle Inputs in einer Kategorie aus JSON
    category.Inputs.forEach((input) => {
      //Prüfung ob es sich um KEIN Label handelt --> Labels haben kein .type
      if (input.Element !== "label") {
        const inputElement = document.getElementById(input.ID);
        //console.log(inputElement.type);

        // Füge den Wert des Inputs dem Objekt inputValues hinzu
        if (inputElement.type === "checkbox" || inputElement.type === "radio") {
          inputValues[input.ID] = inputElement.checked;
        } else {
          inputValues[input.ID] = inputElement.value;
        }
      }
    });
    //console.log(inputValues);
  }

  const markingId = category.ID + "-MARKING";
  const markindMessageId = markingId + "-MESSAGE";

  return (
    <>
      <div
        className={`tile-message ${
          category.MARKING ? "tile-message-marking" : ""
        }`}
      >
        <div className="message-head" onClick={showBody}>
          <h2>{category.Kategorie}</h2>
          {category.MARKING === true ? <p>{category.MESSAGE}</p> : null}
        </div>

        <div
          className={`category-body ${showInputs ? "open" : ""}`}
          id={category.ID}
        >
          {category.Inputs.map(
            (input) => (
              inputIDs.push(input.ID),
              input.Element === "input" ? (
                <InputElement key={input.ID} schemaObject={input} />
              ) : input.Element === "dropdownHelper" ? (
                <DropdownElementHelper key={input.ID} schemaObject={input} />
              ) : input.Element === "dropdown" ? (
                <DropdownElement key={input.ID} schemaObject={input} />
              ) : input.Element === "label" ? (
                <InTileLabel key={input.ID} schemaObject={input} />
              ) : input.Element === "textarea" ? (
                <Textarea key={input.ID} schemaObject={input} />
              ) : input.Element === "mandatoryhandler" ? (
                <Mandatoryhandler
                  key={input.ID}
                  schemaObject={input}
                ></Mandatoryhandler>
              ) : null
            )
          )}

          <Box mt={2}></Box>
          <Divider sx={{ width: "100%", borderWidth: "2px" }} />
          <Box mt={2}></Box>

          <div className="row">
            <label htmlFor={markingId}>Diese Kategorie markieren</label>
            <input type="checkbox" id={markingId} name={markingId}></input>

            {document.getElementById(markingId) ? (
              <input
                type="text"
                id={markindMessageId}
                name={markindMessageId}
              ></input>
            ) : null}
          </div>
        </div>

        <p></p>

        <div className="row">
          {/* 
                                    <input className="button" value="Abschnitt speichern" type="button" onClick={handleSaveSection}></input>
                    */}

          <div
            className={`category-buttons ${showInputs ? "" : "hide"}`}
            onClick={hideBody}
          >
            Einklappen
          </div>
          <div
            className={`category-buttons ${showInputs ? "hide" : ""}`}
            onClick={showBody}
          >
            Ausklappen
          </div>
        </div>
      </div>
    </>
  );
}
