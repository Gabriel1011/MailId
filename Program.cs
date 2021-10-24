using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using EAGetMail;
using iText.Html2pdf;
using SelectPdf;

namespace MailId
{
    class Program
    {
        static void Main(string[] args)
        {

            ReadEmail();

            Console.WriteLine("Hello World!");
        }

        public static void ReadEmail()
        {
            var oServer = new MailServer("imap.gmail.com", "space.just.it@gmail.com", "rvdxvfptcznlabiy", ServerProtocol.Imap4)
            {
                SSLConnection = true,
                Port = 993
            };
            
            MailClient oClient = new MailClient("TryIt");
            oClient.GetMailInfosParam.GetMailInfosOptions = GetMailInfosOptionType.NewOnly;

            oClient.Connect(oServer);

            foreach (var info in oClient.GetMailInfos().Where(e => !e.Read))
            {
                Mail oMail = oClient.GetMail(info);

                System.Console.WriteLine(oMail.Subject + " - Marcado como lido!!");
                //oClient.MarkAsRead(info, true);

                ConvertHtml(oMail.HtmlBody, oMail.Subject);

                // var count = oMail.Attachments.ToList().Count;
                // for (int j = 0; j < count; j++)
                // {
                //     oMail.Attachments[j].SaveAs(oServer.MapPath("~/Inbox") + "\\" + oMail.Attachments[j].Name, true); // true for overWrite file
                // }
            }
        }

        public static void SendEmail()
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("space.just.it@gmail.com", "rvdxvfptcznlabiy") //ijhqgfxxpbeoeplx
            };

            var msg = new System.Net.Mail.MailMessage();
            msg.To.Add("gabriel.rodrigues.almeida1@gmail.com");

            msg.From = new System.Net.Mail.MailAddress("space.just.it@gmail.com");
            msg.Subject = "Teste de mensagem";
            msg.Body = "Teste";
            client.Send(msg);
        }

        public static void ConvertHtml(string html, string subject)
        {
            var teste = new Stopwatch();

            teste.Start();

            File.WriteAllText($@"C:\dev\MailId\Docs\hmtl_{Guid.NewGuid()}.html", html);

            #region SelectPdf
            //html = html.Replace("StrTag", "<").Replace("EndTag", ">");

            HtmlToPdf htmlToPdf = new HtmlToPdf();
            PdfDocument pdfDocument = htmlToPdf.ConvertHtmlString(html);

            pdfDocument.Save($@"C:\dev\MailId\Docs\{subject}_{Guid.NewGuid()}.pdf");
            pdfDocument.Close();

            teste.WriteConsoleConversor("Gerador SelectPdf");
            #endregion

            #region Html2pdf

            teste.ResetTimer();

            var arquivo = $@"C:\dev\MailId\Docs\{subject}_{Guid.NewGuid()}.pdf";
            using var fileHtml = File.Create(arquivo);
            byte[] info = new UTF8Encoding(true).GetBytes(html);
            fileHtml.Write(info, 0, info.Length);
            fileHtml.Dispose();

            using (FileStream htmlSource = File.Open(arquivo, FileMode.Open))
            using (FileStream pdfDest = File.Open($@"C:\dev\MailId\Docs\teste_{Guid.NewGuid()}.pdf", FileMode.OpenOrCreate))
            {
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(htmlSource, pdfDest, converterProperties);
            }

            teste.WriteConsoleConversor("Gerador Html2pdf");

            #endregion

            #region IronPDF

            teste.ResetTimer();

            var Renderer = new IronPdf.ChromePdfRenderer();
            Renderer.RenderHtmlAsPdf(html).SaveAs($@"C:\dev\MailId\Docs\{subject}_{Guid.NewGuid()}.pdf");

            teste.WriteConsoleConversor("Gerador IronPDF");

            #endregion

            #region DinkPDF

            teste.ResetTimer();

            new DinkToPDF().ConverterDink(html, subject);

            teste.WriteConsoleConversor("Conversor DinkToPDF");

            #endregion
        }
    }
}
