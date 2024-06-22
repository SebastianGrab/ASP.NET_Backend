import React from 'react';
import jsPDF from 'jspdf';
import 'jspdf-autotable';
import DRKLogo from '../Resources/Images/DRK_logo_logIn.png';

const headerImage = DRKLogo;

const pdfStyles = {
  table: {
    startY: 10,
    theme: 'plain',
    styles: {
      font: 'Helvetica',
      fontSize: 12,
      halign: 'left',
      valign: 'middle',
      textColor: [0, 0, 0],
      lineWidth: 0.25,
      cellPadding: 2,
      overflow: 'linebreak',
    },
    columnStyles: {
      0: { fontStyle: 'bold', cellPadding: { left: 2, right: 5 } },
      1: { cellPadding: { left: 2, right: 10 } },
    },
    alternateRowStyles: {
      fillColor: [240, 240, 240],
    },
    headStyles: {
      fillColor: [200, 0, 0],
      textColor: [255, 255, 255],
      lineWidth: 0.5,
      lineColor: [200, 0, 0],
    },
    bodyStyles: {
      lineWidth: 0.25,
      lineColor: [0, 0, 0],
    },
    margin: { top: 20 },
    columnWidth: 'wrap',
  },
};

class PdfDownload extends React.Component {
  handleConvertToPdf = (protocol) => {
    const doc = new jsPDF();
    let startY = 40; // Initial startY position with space for header
    const pageHeight = doc.internal.pageSize.height; // Get page height

    let pageNumber = 1;
    this.addPageHeader(doc, pageNumber);

    protocol.Schema.forEach(schema => {
      startY = this.addHeader(doc, schema.Kategorie, startY, pageHeight);

      const rows = this.getRows(schema);

      const spaceLeft = pageHeight - startY;
      const schemaHeight = rows.length * 10; // Assuming each row is 10 units tall
      if (schemaHeight > spaceLeft) {
        doc.addPage();
        pageNumber++;
        this.addPageHeader(doc, pageNumber);
        startY = 40; // Reset startY position after header
        this.addHeader(doc, schema.Kategorie, startY, pageHeight);
      }

      doc.autoTable({
        ...pdfStyles.table,
        body: rows,
        startY: startY + 10,
      });

      startY = doc.autoTable.previous.finalY + 10;
    });

    // Protocol name
    //const id = {protocol:id};
    const auftragsnummer = protocol.Schema[0].Inputs[1].Value || 'unknown';
    const fileName = `protocol_${auftragsnummer}.pdf`;
    doc.save(fileName);
    console.log(protocol);
  };

  addPageHeader = (doc) => {
    doc.setFont('Helvetica', 'normal');
    doc.setFontSize(10);
    doc.setTextColor(0, 0, 0);

    doc.addImage(headerImage, 'PNG', (doc.internal.pageSize.width / 2) - 15, 5, 30, 20); // Add image in the center
    doc.setLineWidth(0.5);
    doc.line(10, 30, doc.internal.pageSize.width - 10, 30); // Draw a line below the header

  };

  addHeader = (doc, headerText, startY, pageHeight) => {
    if (startY > pageHeight - 20) {
      doc.addPage();
      this.addPageHeader(doc);
      startY = 40;
    }
    doc.setFont('Helvetica', 'bold'); // Set font to bold Helvetica
    doc.setFontSize(16); // Set font size for headers
    doc.setTextColor(200, 0, 0); // Set header text color to DRK red
    doc.text(headerText, 10, startY);
    return startY;
  };

  getRows = (schema) => {
    return schema.Inputs.reduce((acc, input) => {
      const value = input.Value === true ? 'zutreffend' : input.Value;
      if (value !== undefined) {
        acc.push([input.Label, value]);
      }
      if (input.HelperNames) {
        for (let i = 0; i < input.HelperNames.length; i++) {
          acc.push([input.Label, input.HelperNames[i]]);
        }
      }
      return acc;
    }, []);
  };

  render() {
    return (
      <div>
        <input className="button" value="Download PDF" type="button" onClick={() => this.handleConvertToPdf(this.props.protocol)}></input>
      </div>
    );
  }
}

export default PdfDownload;
