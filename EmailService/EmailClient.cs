using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace Easy.Public.Email
{
    public class EmailClient
    {

        private SmtpClient smtpClient;
        private MailAddress from;
        private EmailClient(MailAddress from, String server, Int32 port, String username, String password) 
        {
            this.from = from;

            this.smtpClient = new SmtpClient(server, port);
            this.smtpClient.UseDefaultCredentials = false;
            this.smtpClient.Credentials = new NetworkCredential(username, password);
        
        }
        private EmailClient(MailAddress from, SmtpClient smtpClient) 
        {
            this.from = from;
            this.smtpClient = smtpClient;
        }
        public static EmailClient CreateEmailClient(MailAddress from,String server,Int32 port ,String username,String password)
        {
            return new EmailClient(from, server, port, username, password);
        }
        public static EmailClient CreateEmailClient(MailAddress from, SmtpClient smtpClient)
        {
            return new EmailClient(from, smtpClient);
        }

        public void Send(MailAddress to, String subject, String content,Boolean isBodyHtml,Boolean isAsync)
        {
            this.Send(new MailAddress[] { to }, subject, content, isBodyHtml, isAsync);
        }
        public void Send(MailAddress[] tolist, String subject, String content, Boolean isBodyHtml, Boolean isAsync)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = this.from;
            mailMessage.SubjectEncoding = UTF8Encoding.UTF8;
            mailMessage.Subject = subject;
            mailMessage.BodyEncoding = UTF8Encoding.UTF8;
            mailMessage.Body = content;
            mailMessage.IsBodyHtml = isBodyHtml; 

            foreach (MailAddress mailAddress in tolist)
            {
                mailMessage.To.Add(mailAddress);
            }
            this.Send(mailMessage, isAsync);
        }
        public void Send(MailMessage message, Boolean isAsync)
        {
            if (isAsync)
            {
                smtpClient.SendAsync(message, null);
            }
            else
            {
                smtpClient.Send(message);
            }
        }
    }
}
