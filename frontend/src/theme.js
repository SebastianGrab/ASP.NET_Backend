import React from "react";
import { ThemeProvider, createTheme } from "@mui/material";

const theme = createTheme({
  palette: {
    mode: "light",
    primary: {
      main: "#e60005",
    },
    secondary: {
      main: "#002D55",
      light: "#49709b"
    },
  },
  typography: {
    fontSize: 16,
  },
});

export const Themed = (props) => {
  return <ThemeProvider theme={theme}>{props.children}</ThemeProvider>;
};
