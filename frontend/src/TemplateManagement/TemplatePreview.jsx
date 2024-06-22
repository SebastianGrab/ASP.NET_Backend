import { useState, useEffect, useContext } from "react";
import AuthContext from "../API/AuthProvider";
import { useParams, useLocation } from "react-router-dom";
import Interpreter from "../Components/Interpreter/Interpreter";
import { EditTemplateDialog } from "./EditTemplateDialog";
import { deleteCall } from "../API/deleteCall";
import { getCall } from "../API/getCall";
import { Dialog, DialogTitle, DialogContent, DialogActions, TextField, Button } from '@mui/material';
import { useNavigate } from "react-router-dom";

export default function TemplatePreview() {

    const { token, userID, orgaID, setRefreshHandler} = useContext(AuthContext);
    const [openUpdateDialog, setOpenUpdateDialog] = useState(false);
  const { template } = useParams();
  const location = useLocation();
  const templateData = location.state?.payload;
  const templateContent = JSON.parse(templateData.templateContent);
  const templateID = templateData.id;
  const [open, setOpen] = useState(false);
  const navigate = useNavigate();

  const dialogUpdateHandler = () => {
    setOpenUpdateDialog(!openUpdateDialog)

    console.log(openUpdateDialog);

}

const deleteTemplate = async () => {
    const orgas = await getCall("/api/template/" + templateData.id + "/organizations?pageIndex=1&pageSize=9999999", token, "Fehler beim getten der ORgas");
    console.log(orgas);
    try{
      const response = await deleteCall("/api/template/" + templateData.id + "/remove-from-organization/" + orgaID,"Fehler beim löschen des Templates", token);
      setRefreshHandler(response + 1);

    } catch(error){
      console.log("Fehler beim löschen des Templates", error)
    }
    navigate(-1);



    // Error: One or more validation errors occurred


    // const response = await deleteCall("/api/template/" + templateData.id, "Fehler beim löschen des Templates", token);
    // console.log(response);



}



  return (
    <>
      <h1>Template Vorschau: {templateData.name}</h1>

<div className="row">
<button className="button-scnd" onClick={dialogUpdateHandler}>Bearbeiten</button>
<button className="button" onClick={deleteTemplate}>Löschen</button>

</div>

      <EditTemplateDialog open={openUpdateDialog} handleDialog={dialogUpdateHandler} templateData={templateData}></EditTemplateDialog>




        <Interpreter schema={templateContent} />

        <Dialog open={open} onClose={() => setOpen(false)}>
            <DialogTitle>Prüfung</DialogTitle>
            <DialogContent>
                <p>Wollen Sie dieses Template wirklich für Ihre Organisation entfernen?</p>
            </DialogContent>
            <DialogActions>
                <Button variant="contained" color="primary" onClick={deleteTemplate}>
                    Löschen
                </Button>
                <Button variant="contained" color="primary" onClick={() => setOpen(false)}>
                    Abbrechen
                </Button>
            </DialogActions>
        </Dialog>





    </>
  );
}
