using PDFGenerator.iText;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Diagnostics;
using System.IO;
using PDFGenerator.Utilities;

namespace PDFGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            //Declaring fonts
            var HELVETICA_12 = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);
            var HELVETICA_8 = FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.BLACK);
            var HELVETICA_BOLD_8 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, BaseColor.BLACK);
            var HELVETICA_BOLD_10 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.BLACK);
            var HELVETICA_BOLD_12 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK);


            string randomText = CommonUtility.LoremIpsum(10, 10, 2, 5, 1);

            Document document = new Document(PageSize.A4, 48f, 48f, 140f, 48f);
            var pdfDir = Directory.CreateDirectory($"{baseDir}\\PdfDocuments");
            var pdfFilePath = $"{pdfDir.FullName}\\Test.pdf";

            if (File.Exists(pdfFilePath))
            {
                File.Delete(pdfFilePath);
            }
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfFilePath, FileMode.Create));
            writer.PageEvent = new ITextEvents();
            document.Open();
            document.NewPage();

            Paragraph para = new Paragraph(randomText + "\n\n", HELVETICA_8);
            para.Alignment = Element.ALIGN_LEFT;
            document.Add(para);

            //Creating pdf table
            PdfPTable table = new PdfPTable(3);

            table.DefaultCell.Phrase = new Phrase() { Font = HELVETICA_8 };

            PdfPCell header1Cell = new PdfPCell();
            header1Cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            header1Cell.AddElement(new Paragraph("LOREM IPSUM", HELVETICA_BOLD_8));
            table.AddCell(header1Cell);


            PdfPCell header2Cell = new PdfPCell();
            header2Cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            header2Cell.AddElement(new Paragraph("LECTUS PROIN", HELVETICA_BOLD_8));
            table.AddCell(header2Cell);

            PdfPCell header3Cell = new PdfPCell();
            header3Cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            header3Cell.AddElement(new Paragraph("LACINIA AT QUIS RISUS", HELVETICA_BOLD_8));
            table.AddCell(header3Cell);


            table.AddCell(new Phrase("Faucibus et molestie ac", HELVETICA_8));
            table.AddCell(new Phrase("Ut porttitor leo a diam", HELVETICA_8));
            table.AddCell(new Phrase("Nunc id cursus metus", HELVETICA_8));

            table.AddCell(new Phrase("Quis auctor elit sed vulputate", HELVETICA_8));
            table.AddCell(new Phrase("Nibh tortor id aliquet lectus", HELVETICA_8));
            table.AddCell(new Phrase("Convallis aenean et tortor at risus viverra", HELVETICA_8));


            table.AddCell(new Phrase("Faucibus et molestie ac", HELVETICA_8));
            table.AddCell(new Phrase("Ut porttitor leo a diam", HELVETICA_8));
            table.AddCell(new Phrase("Nunc id cursus metus", HELVETICA_8));

            document.Add(table);
            document.Add(new Chunk("\n"));

            //Creating List 

            PdfPCell cell = new PdfPCell();


            List list = new List(List.ORDERED, 20f);
            list.SetListSymbol("\u2022");
            list.IndentationLeft = 20f;


            list.Add(new ListItem("Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", HELVETICA_8));
            list.Add(new ListItem("Quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea.", HELVETICA_8));
            list.Add(new ListItem("Nulla facilisi nullam vehicula ipsum a arcu cursus. A lacus vestibulum sed arcu non odio. ", HELVETICA_8));
            list.Add(new ListItem("Cras ornare arcu dui vivamus arcu felis bibendum. Nisl nisi scelerisque eu ultrices vitae. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", HELVETICA_8));
            list.Add(new ListItem("Purus semper eget duis at tellus at.", HELVETICA_8));


            cell.AddElement(list);
            cell.Border = Rectangle.NO_BORDER;


            document.Add(list);

            //document.Add(table5);

            //Close the document 
            document.Close();

            //Launch the document if you have a file association set for PDF's 
            // Process AcrobatReader = new Process();
            //AcrobatReader.StartInfo.FileName = pdfFilePath;
            //AcrobatReader.Start();


            Process.Start("cmd.exe ", @"/c " + pdfFilePath);

        }
    }
}
