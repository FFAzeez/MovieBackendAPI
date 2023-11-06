using iText.Html2pdf;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Layout.Element;
using iText.StyledXmlParser.Css.Media;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MovieBackendAPI.Domain.Utility
{
    public class ExportFileResponse
    {
        public string FileName { get; set; }
        public MemoryStream Stream { get; set; }
        public byte[] ZipStream { get; internal set; }
    }
    public static class PDFGenerationExtension
    {
        public static ExportFileResponse GeneratePDF(string htmlFile, string fileName)
        {
            return GeneratePDF(htmlFile, fileName, PageSize.A4);
        }


        public static ExportFileResponse GeneratePDF(string htmlFile, string fileName, PageSize pageSize)
        {
            ExportFileResponse fileGenerationResponse = new ExportFileResponse();

            // var bytes = System.Text.Encoding.UTF8.GetBytes(htmlFile);

            var input = new MemoryStream();


            using (var pdfWriter = new PdfWriter(input, new WriterProperties().SetFullCompressionMode(true).UseSmartMode()))
            {
                pdfWriter.SetCloseStream(false);


                //Set compression level

                pdfWriter.SetCompressionLevel(CompressionConstants.BEST_COMPRESSION);

                // ConverterProperties properties = new ConverterProperties();
                PdfDocument pdf = new PdfDocument(pdfWriter);
                //pdf.SetDefaultPageSize(new PageSize)
                //pdf.setLeftMargin(50);
                pdf.SetTagged();
                // PageSize pageSize = PageSize.A4.Rotate();
                //         pageSize.ApplyMargins(5, 5, 5, 5,false);

                pdf.SetDefaultPageSize(pageSize);
                ConverterProperties properties = new ConverterProperties();
                // properties.SetBaseUri(baseUri);
                MediaDeviceDescription mediaDeviceDescription
                    = new MediaDeviceDescription(MediaType.PRINT);
                mediaDeviceDescription.SetWidth(pageSize.GetWidth());

                properties.SetMediaDeviceDescription(mediaDeviceDescription);



                //var pdfDocument = new Document();

                //var currentPage = pdfDocument.Pages.Add();

                //var htmlFragment = new HtmlFragment(html);

                //currentPage.Paragraphs.Add(htmlFragment);

                //pdfDocument.Save(dataDir + "HTMLToPDF1_out.pdf");


                // properties.Size
                using (var document = HtmlConverter.ConvertToDocument(htmlFile, pdf, properties))
                {

                    document.SetMargins(0, 0, 0, 0);



                }
                input.Position = 0;
                fileGenerationResponse.FileName = $"{fileName}.pdf";
                fileGenerationResponse.Stream = input;

                return fileGenerationResponse;
            }
        }

    }


}
