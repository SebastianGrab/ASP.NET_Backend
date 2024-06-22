import { useState, useEffect } from "react";
import ReviewInputElement from "./ReviewElement";
import ReviewDropdownElementHelper from "./ReviewDropdownElementHelper";
import ReviewDropdownElement from "./ReviewDropdownElement";
import InTileLabel from "../../Components/Interpreter/InTileLabel";
import ReviewTextarea from "./ReviewTextarea";

export default function ReviewTileCategory({ category }) {
  const [showInputs, setShowMessageBody] = useState(false);
  const [numberOfInputs, setNumberOfInputs] = useState(0);
  const [numberOfValues, setNumberOfValues] = useState(0);

  const getInputs = () => {
    setNumberOfInputs(category.Inputs.length);

    category.Inputs.forEach((input) => {
      if (input.Value) {
        setNumberOfValues(numberOfValues + 1);
      }
    });
  };

  useEffect(() => {
    getInputs();
  }, []);


  const showBody = () => {
    setShowMessageBody(true);
  };

  const hideBody = () => {
    setShowMessageBody(false);
  };

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

          {category.Inputs.map((input) =>
            input.Element === "input" ? (
              <ReviewInputElement
                key={input.ID}
                schemaObject={input}
              />
            ) : input.Element === "dropdownHelper" ? (
              <ReviewDropdownElementHelper
                key={input.ID}
                schemaObject={input}
              />
            ) : input.Element === "dropdown" ? (
              <ReviewDropdownElement key={input.ID} schemaObject={input} />
            ) : input.Element === "label" ? (
              <InTileLabel key={input.ID} schemaObject={input} />
            ) : input.Element === "textarea" ? (
              <ReviewTextarea key={input.ID} schemaObject={input} />
            ) : null
          )}
        </div>

        <p></p>

        <div className="row">
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
