using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Text;

namespace PDFGenerator.iText
{
    public class ITextEvents : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate, footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;


        #region Fields
        private string _header;
        #endregion

        #region Properties
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        #endregion


        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 100);
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {

            }
            catch (System.IO.IOException ioe)
            {

            }
        }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);

            //Declaring different Fonts that needs to be used in the PDF

            iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            var HELVETICA_12 = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);
            var HELVETICA_8 = FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.BLACK);
            var HELVETICA_BOLD_8 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, BaseColor.BLACK);
            var HELVETICA_BOLD_10 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.BLACK);
            var HELVETICA_BOLD_12 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK);


            //Create PdfTable object
            PdfPTable pdfTab = new PdfPTable(4);

            //We will have to create separate cells to include image logo and 2 separate strings
            //Row 1
            var headerDetailHd = new Phrase("LOREM IPSUM DOLOR", HELVETICA_BOLD_12);
            var headerDetailTxt = new Phrase("Lorem Ipsum dolor sit \nAMET SED DO QUIS \n+01 234 5678901", HELVETICA_8);
            PdfPCell detailsCell = new PdfPCell();
            detailsCell.Colspan = 2;
            detailsCell.FixedHeight = 80f;
            detailsCell.AddElement(headerDetailHd);
            detailsCell.AddElement(headerDetailTxt);
            PdfPCell logoCell = new PdfPCell();
            logoCell.Colspan = 2;
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string logoPath = $"{baseDir}\\images\\logo.png";

            iTextSharp.text.Image logoImg = iTextSharp.text.Image.GetInstance(logoPath);

            logoImg.ScaleAbsolute(102f, 245f);      // Image Size
            logoImg.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
            //logoImg.ScaleAbsolute(200, 100f);
            logoCell.AddElement(logoImg);


            String text = "Page " + writer.PageNumber + " of ";
            String footerAddress = "Tellus cras adipiscing enim eu turpis egestas pretium aenean";
            String footerTel = "+01 23 4567 8901";


            //Add paging to header
            /*
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.SetTextMatrix(document.PageSize.GetRight(200), document.PageSize.GetTop(45));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 12);
                //Adds "12" in Page 1 of 12
                cb.AddTemplate(headerTemplate, document.PageSize.GetRight(200) + len, document.PageSize.GetTop(45));
            }
            */
            //Add paging to footer
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(30));
                cb.ShowText(text);
                //cb.EndText();
                float len = bf.GetWidthPoint(text, 8);
                cb.SetTextMatrix(document.PageSize.GetLeft(38), document.PageSize.GetBottom(30));
                cb.ShowText(footerAddress);
                cb.SetTextMatrix(document.PageSize.GetLeft(38), document.PageSize.GetBottom(20));
                cb.ShowText(footerTel);
                cb.EndText();
                // float len2 = bf.GetWidthPoint(footerAddress, 7);
                cb.AddTemplate(footerTemplate, document.PageSize.GetRight(180) + len, document.PageSize.GetBottom(30));
            }

            //Row 2
            PdfPCell moreDetialsCell = new PdfPCell();
            moreDetialsCell.Colspan = 3;
            Chunk patientHeading = new Chunk("LOREM: ", HELVETICA_BOLD_8);
            Chunk patientName = new Chunk("Quam Adipiscing Vitae", HELVETICA_8);
            Chunk patientDetails = new Chunk(", Faucibus ", HELVETICA_8);
            Phrase patientDetailPhrase = new Phrase(patientHeading);
            patientDetailPhrase.Add(patientName);
            moreDetialsCell.AddElement(patientDetailPhrase);

            PdfPCell dateCell = new PdfPCell();
            Chunk dateHeading = new Chunk("DATE   ", HELVETICA_BOLD_8);
            string currentDate = DateTime.Now.ToString("dd MMM yyyy");
            Chunk currentDateChunk = new Chunk(currentDate, HELVETICA_8);
            Paragraph dateParagraph = new Paragraph(dateHeading);
            dateParagraph.Add(currentDateChunk);
            dateParagraph.Alignment = Element.ALIGN_RIGHT;
            dateCell.AddElement(dateParagraph);


            //Row 3
            PdfPCell pdfCell5 = new PdfPCell(new Phrase("Date:" + PrintTime.ToShortDateString(), baseFontBig));
            PdfPCell pdfCell6 = new PdfPCell();
            PdfPCell pdfCell7 = new PdfPCell(new Phrase("TIME:" + string.Format("{0:t}", DateTime.Now), baseFontBig));


            //set the alignment of all three cells and set border to 0
            logoCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            dateCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pdfCell5.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell6.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfCell7.HorizontalAlignment = Element.ALIGN_CENTER;

            moreDetialsCell.VerticalAlignment = Element.ALIGN_TOP;
            pdfCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfCell7.VerticalAlignment = Element.ALIGN_MIDDLE;

            detailsCell.Border = 0;
            logoCell.Border = 0;
            moreDetialsCell.Border = 0;
            dateCell.Border = 0;

            pdfCell5.Border = 0;
            pdfCell6.Border = 0;
            pdfCell7.Border = 0;


            //add all three cells into PdfTable


            pdfTab.AddCell(detailsCell);
            pdfTab.AddCell(logoCell);
            pdfTab.AddCell(moreDetialsCell);
            pdfTab.AddCell(dateCell);
            //pdfTab.AddCell(pdfCell7);

            pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.WidthPercentage = 100;
            //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;


            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write
            //Third and fourth param is x and y position to start writing
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
            //set pdfContent value

            //Move the pointer and draw line to separate header section from rest of page
            cb.MoveTo(40, document.PageSize.Height - 130);
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 130);
            cb.Stroke();

            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(40, document.PageSize.GetBottom(50));
            cb.LineTo(document.PageSize.Width - 200, document.PageSize.GetBottom(50));
            cb.Stroke();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            headerTemplate.BeginText();
            headerTemplate.SetFontAndSize(bf, 12);
            headerTemplate.SetTextMatrix(0, 0);
            headerTemplate.ShowText((writer.PageNumber - 1).ToString());
            headerTemplate.EndText();

            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 12);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText((writer.PageNumber - 1).ToString());
            footerTemplate.EndText();


        }

    }
}
