using System;
using System.Net;
using System.Net.Mail;
using EAGetMail;

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
            // site de como implmentar o aegetemail https://www.thecodehubs.com/read-email-from-gmail-and-outlook-in-c-sharp/

            var oServer = new MailServer("imap.gmail.com", "space.just.it@gmail.com", "rvdxvfptcznlabiy", ServerProtocol.Imap4);
            MailClient oClient = new MailClient("TryIt");

            oServer.SSLConnection = true;
            oServer.Port = 993;

            oClient.GetMailInfosParam.GetMailInfosOptions = GetMailInfosOptionType.NewOnly;

            oClient.Connect(oServer);
            MailInfo[] infos = oClient.GetMailInfos();

            for (int i = infos.Length - 1; i > 0; i--)
            {
                MailInfo info = infos[i];
                Mail oMail = oClient.GetMail(info);

                System.Console.WriteLine(oMail.Subject);

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
    }
}
