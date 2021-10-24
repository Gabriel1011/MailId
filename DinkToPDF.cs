using DinkToPdf;
using System;
using System.IO;

namespace MailId
{
    public class DinkToPDF
    {
        public void ConverterDink(string html, string subject)
        {
            var converter = new BasicConverter(new PdfTools());

            converter.PhaseChanged += Converter_PhaseChanged;
            converter.ProgressChanged += Converter_ProgressChanged;
            converter.Finished += Converter_Finished;
            converter.Warning += Converter_Warning;
            converter.Error += Converter_Error;

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                        FooterSettings = { FontSize = 9, Right = "Page [page] of [toPage]" }
                    }
                }
            };

            byte[] pdf = converter.Convert(doc);

            var file = $@"C:\dev\MailId\Docs";

            if (!Directory.Exists(file)) Directory.CreateDirectory(file);

            using var stream = new FileStream($@"{file}\{subject}_{Guid.NewGuid()}.pdf", FileMode.Create)

            stream.Write(pdf, 0, pdf.Length);

            //Console.ReadKey();
        }

        private static void Converter_Error(object sender, DinkToPdf.EventDefinitions.ErrorArgs e)
        {
            //Console.WriteLine("[ERROR] {0}", e.Message);
        }

        private static void Converter_Warning(object sender, DinkToPdf.EventDefinitions.WarningArgs e)
        {
            //Console.WriteLine("[WARN] {0}", e.Message);
        }

        private static void Converter_Finished(object sender, DinkToPdf.EventDefinitions.FinishedArgs e)
        {
            //Console.WriteLine("Conversion {0} ", e.Success ? "successful" : "unsucessful");
        }

        private static void Converter_ProgressChanged(object sender, DinkToPdf.EventDefinitions.ProgressChangedArgs e)
        {
            //Console.WriteLine("Progress changed {0}", e.Description);
        }

        private static void Converter_PhaseChanged(object sender, DinkToPdf.EventDefinitions.PhaseChangedArgs e)
        {
            //Console.WriteLine("Phase changed {0} - {1}", e.CurrentPhase, e.Description);
        }
    }
}
