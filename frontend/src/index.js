import React, { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { App } from "./App";
import { BrowserRouter } from 'react-router-dom';
import "./styles.css";
import { Themed } from "./theme";

const root = createRoot(document.getElementById("root"));
root.render(
  <StrictMode>


    <Themed>
    <BrowserRouter>
    <App />
    </BrowserRouter>
    </Themed>
  </StrictMode>
);