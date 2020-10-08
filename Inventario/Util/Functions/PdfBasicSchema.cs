using iText.Html2pdf;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Linq;
using Util.Enums;

namespace Util.Functions
{  
    public class PdfBasicSchema : IEventHandler
    {
        PdfXObject stationery;
        string mark;
        static string resourceRoute;

        public PdfBasicSchema(PdfDocument pdf, string source, string resRoute)
        {
            resourceRoute = resRoute;
            PdfDocument template = new PdfDocument(new PdfReader(source));
            PdfPage page = template.GetPage(1);
            stationery = page.CopyAsFormXObject(pdf);
            template.Close();
        }

        public PdfBasicSchema(PdfDocument pdf, string source, string watermark, string resRoute)
        {
            resourceRoute = resRoute;
            PdfDocument template = new PdfDocument(new PdfReader(source));
            PdfPage page = template.GetPage(1);
            stationery = page.CopyAsFormXObject(pdf);
            mark = watermark;
            template.Close();
        }

        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument pdf = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdf);
            pdfCanvas.AddXObject(stationery, 0, 0);

            if (mark != null && mark.Trim() != "")
            {
                //Rectangle rect = new Rectangle(36, 400, 36, 64);
                PdfFont font = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(StandardFonts.HELVETICA));
                Paragraph p = new Paragraph(mark).SetFont(font).SetFontSize(100);
                pdfCanvas.SaveState();
                PdfExtGState gs1 = new PdfExtGState();
                gs1.SetFillOpacity(0.5f);
                pdfCanvas.SetExtGState(gs1);
                Canvas canvas = new Canvas(pdfCanvas, pdf, pdf.GetDefaultPageSize())
                    .ShowTextAligned(p, 297, 450, 1, TextAlignment.CENTER, VerticalAlignment.TOP, 0.785398F);
                pdfCanvas.RestoreState();
                canvas.Close();
            }
        }

        public static string GenerateHeader(string titulo, string codigo, DateTime fecha, string version, string resourceRoute, string imgName)
        {
            string htmlStr =
                "<html>" +
                    "<head>" +
                        "<link rel='stylesheet' type='text/css' href='" + resourceRoute + eRoutes.Html + "/styleBackground.css'>" +
                    "</head>" +
                    "<body style='font-family: Arial, Helvetica, sans-serif'>";
            string header =
                "<table class='mb-1'>" +
                    "<tr class='align-center'>" +
                        "<td style='width: 25%' colspan='1'><img style='max-width: 5cm;' src='" + resourceRoute + eRoutes.Html + "/" + imgName + "'></td>" +
                        "<td style='width: 75%' colspan='3' class='font-size5'>{RegistroX}</td>" +
                    "</tr>" +
                    "<tr class='mb-1 align-center font-size2'>" +
                        "<td colspan='2'>Fecha: {FechaX}</td>" +
                        "<td colspan='2' class='align-center'>Versión: {VersionX}</td>" +
                    "</tr>" +
                "</table>";
            header = header.Replace("{RegistroX}", titulo);
            header = header.Replace("{CodigoX}", codigo);
            header = header.Replace("{FechaX}", fecha.ToShortDateString());
            header = header.Replace("{VersionX}", version);

            htmlStr = htmlStr.Insert(htmlStr.Count(), header);
            htmlStr = htmlStr.Insert(htmlStr.Count(), "</body></html>");
            string source = System.IO.Path.GetTempFileName().Split(".")[0] + ".pdf";
            PdfWriter writer = new PdfWriter(source);
            PdfDocument pdfDoc = new PdfDocument(writer);
            pdfDoc.SetDefaultPageSize(PageSize.A4.Rotate());
            ConverterProperties converterProperties = new ConverterProperties();
            HtmlConverter.ConvertToPdf(htmlStr, pdfDoc, converterProperties);
            pdfDoc.Close();
            writer.Close();
            return source;
        }

        public static void GenerateFooter(string intermediateDestination, string finalDestination)
        {
            PdfWriter finalWriter = new PdfWriter(finalDestination);
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(intermediateDestination), finalWriter);
            pdfDoc.SetDefaultPageSize(PageSize.A4.Rotate()); // orientación horizontal
            Document doc = new Document(pdfDoc);

            float xFeet = pdfDoc.GetDefaultPageSize().GetX() + 36;
            float yFeet = pdfDoc.GetDefaultPageSize().GetBottom() + 20;

            //float footertWidth = pdfDoc.GetPage(1).GetPageSize().GetWidth() - 80; //orientación vertical
            float footertWidth = pdfDoc.GetPage(1).GetPageSize().GetWidth() - 72;
            float footerHeight = 15F;

            Rectangle footerRectangle = new Rectangle(xFeet, yFeet, footertWidth, footerHeight);

            int n = pdfDoc.GetNumberOfPages();
            for (int i = 1; i <= n; i++)
            {
                PdfPage page = pdfDoc.GetPage(i);
                PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
                Canvas canvasFooter = new Canvas(pdfCanvas, pdfDoc, footerRectangle);

                float[] width = { 0.5F };
                Table footerTable = new Table(width);
                //footerTable.SetWidth(523F);
                footerTable.SetWidth(footertWidth);
                footerTable.SetFontSize(10F);
                footerTable.SetTextAlignment(TextAlignment.RIGHT);

                footerTable.AddCell("Página " + i + " de " + n);
                canvasFooter.Add(footerTable);
                canvasFooter.Close();
            }

            pdfDoc.Close();
            finalWriter.Close();
            doc.Close();
        }
    }
}
