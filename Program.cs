using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using EAGetMail;
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
            var oServer = new MailServer("imap.gmail.com", "space.just.it@gmail.com", "rvdxvfptcznlabiy", ServerProtocol.Imap4);
            MailClient oClient = new MailClient("TryIt");

            oServer.SSLConnection = true;
            oServer.Port = 993;

            oClient.GetMailInfosParam.GetMailInfosOptions = GetMailInfosOptionType.NewOnly;

            oClient.Connect(oServer);
            MailInfo[] infos = oClient.GetMailInfos();


            foreach (var info in infos.Where(e => !e.Read))
            {
                Mail oMail = oClient.GetMail(info);

                System.Console.WriteLine(oMail.Subject + " - Marcado como lido!!");
                //oClient.MarkAsRead(info, true);

                ConvertHtml(oMail.HtmlBody);

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

        public static void ConvertHtml(string html)
        {
            html = html.Replace("StrTag", "<").Replace("EndTag", ">");

            HtmlToPdf htmlToPdf = new HtmlToPdf();
            PdfDocument pdfDocument = htmlToPdf.ConvertHtmlString(html);

            pdfDocument.Save(@"C:\dev\MailId\Docs\teste.pdf");
            pdfDocument.Close();

            //var arquivo = @"C:\dev\MailId\teste.html";
            //using var teste = File.Create(arquivo);
            //byte[] info = new UTF8Encoding(true).GetBytes(html);
            //teste.Write(info, 0, info.Length);

            //if (File.Exists(arquivo))
            //{
            //    string outoputFile = @"C:\dev\MailId\teste.pdf";
            //}
        }
    }
}
