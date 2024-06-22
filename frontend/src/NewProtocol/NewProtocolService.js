import { protocol } from "../API/endpoints";

export const getData = (protocolData) => {
  const plainData = protocolData;

  const inputValues = {};
  let helpers = 1;

  // Durchgehe alle Inputs in einer Kategorie aus JSON
  protocolData.Schema.forEach((category) => {
    if (category.ID === "EINSATZHELFER") {
      const inputCategory = document.getElementById(category.ID);
      console.log(category.ID);
      console.log(inputCategory.children.length);
      helpers = inputCategory.children.length - 6;
    }

    const markingId = category.ID + "-MARKING";
    const markindMessageId = markingId + "-MESSAGE";

    if(document.getElementById(markingId).checked){
        const inputCategory = document.getElementById(category.ID);
        const message = document.getElementById(markindMessageId).value;
        category.MARKING = true;
        category.MESSAGE = message;
    }
    category.Inputs.forEach((input) => {
      //Prüfung ob es sich um KEIN Label handelt --> Labels haben kein .type
      if (input.Element !== "label" && input.Element !== "dropdownHelper") {
        const inputElement = document.getElementById(input.ID);

        // Füge den Wert des Inputs dem Objekt inputValues hinzu
        if (
          (inputElement.type === "checkbox" || inputElement.type === "radio") &&
          inputElement.checked
        ) {
          input.Value = true;
          inputValues[input.ID] = inputElement.checked;
        } else if (
          (inputElement.type === "text" ||
            inputElement.type === "number" ||
            inputElement.type === "time" ||
            inputElement.type === "date") &&
          inputElement.value
        ) {
          input.Value = inputElement.value;
          inputValues[input.ID] = inputElement.value;
        } else if (
          inputElement.name === "dropdown" &&
          inputElement.value !== "-"
        ) {
          input.Value = inputElement.value;
        }
      }

      if (input.Element === "dropdownHelper") {
        input.HelperCollection = [];
        input.HelperNames = [];

        for (let i = 1; i < helpers + 1; i++) {
          const inputHelper = document.getElementById(input.ID + i);
          input.HelperCollection.push(inputHelper.id);
          input.HelperNames.push(inputHelper.value);
        }
      }
    });
  });

  return protocolData;
};

export const buildProtocolInitData = () => {
  const initialPostBody = {
    name: "string",
    isDraft: true,
    reviewComment: "string",
    isClosed: false,
    isInReview: false,
  };

  return initialPostBody;
};

export const buildProtocolContentData = (id, data) => {
  const parsedProtocolData = JSON.stringify(data);
  const body = {
    protocolId: id,
    content: parsedProtocolData,
  };

  return body;
};
export const validateData = (protocol) => {
  console.log(protocol);
  let valid = true;

  for (let i = 0; i < protocol.Schema.length; i++) {
    const category = protocol.Schema[i];
    const inputCategory = document.getElementById(category.ID);

    for (let j = 0; j < category.Inputs.length; j++) {
      const input = category.Inputs[j];

      if (input.Mandatory === 1) {
        console.log(input);
        if (!checkMandatory(input, category.Kategorie)) {
          valid = false;
          return false; // Beenden der Funktion bei erstem Fehler
        }
      }

      if ((input.Type === "text" || input.Type === "number") && input.Pattern && input.Value) {
        if (!checkPattern(input, category.Kategorie)) {
          valid = false;
          return false; // Beenden der Funktion bei erstem Fehler
        }
      }
    }
  }

  return valid;
};

export const checkMandatory = (obj, category) => {
  if (obj.DisableHandlerId && document.getElementById(obj.DisableHandlerId).checked) {
    console.log("handleDisabled")
    return true;
  } else if (!obj.Value) {
    alert(
      "Bitte tragen Sie einen Wert für das Plichfeld: '" +
      obj.Label +
      "' in Kategorie: '" +
      category +
      "' ein."
    );
    return false;
  } else {
    return true;
  }
};

export const checkPattern = (obj, kat) => {
  const pattern = new RegExp(obj.Pattern);

  if (!pattern.test(obj.Value)) {
    alert(
      "Ungültige Eingabe für das Objekt: " +
      obj.Label +
      " in Kategorie: '" +
      kat +
      "'. Benötigtes RegEx-Pattern: " +
      pattern
    );
    return false;
  } else {
    return true;
  }
};

