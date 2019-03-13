using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace Horseplay.Classes
{
    public class Mail
    {
        SmtpClient Smtp;
        string Status = "";
        MailMessage Email;

        public Mail()
        {
            //setting up gmail server

            Email = new MailMessage();
            Smtp = new SmtpClient();
            Smtp.Host = "smtp.webio.pl";
            Smtp.EnableSsl = true;
            Smtp.Port = 465;//or 465 or 25 or 587
            Smtp.UseDefaultCredentials = false;
            var ConfManager = ConfigurationManager.AppSettings["NOREPLY_PASSWORD"];
            Smtp.Credentials = new System.Net.NetworkCredential("no-reply@systo.pl", ConfManager);
            Smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        }
        public void Send(string inputEmail, string subject, string textBody, string htmlBody)
        {
            try
            {
                //creating an email

                MailAddress fromAddress = new MailAddress("no-reply@systo.pl", "systo.pl");
                this.Email.From = fromAddress;
                this.Email.To.Add(inputEmail);
                this.Email.Subject = subject;
                this.Email.Body = textBody;
                ContentType mimeType = new ContentType("text/html");
                AlternateView alternate = AlternateView.CreateAlternateViewFromString(htmlBody, mimeType);
                this.Email.AlternateViews.Add(alternate);
                Smtp.Send(this.Email);
                this.Status = "Wysyłanie wiadomości zakończone sukcesem";
            }
            catch (Exception ex)
            {
                this.Status = "Error: " + ex.ToString();
            }
        }

    }
}