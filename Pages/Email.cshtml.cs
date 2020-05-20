using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cit_eTrike.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;

namespace Cit_eTrike.Pages
{
    public class EmailModel : PageModel
    {
        DataAccess dataAccess = new DataAccess();
        public void OnGet()
        {

        }
        public JsonResult OnPost([FromBody]EmailMsg emailMsg)
        {
            string emailTo = dataAccess.GetEmail();

            MailMessage mail = new MailMessage();
            mail.To.Add(emailTo);
            mail.Subject = emailMsg.Name + " - " + emailMsg.Subject;
            mail.Body = emailMsg.Message + "\n" + emailMsg.Name;
            mail.IsBodyHtml = false;
            mail.Sender = new MailAddress("WebService@cit-etrike.de");          

            if (emailMsg.IsValidEmail(emailMsg.Email) != false)
            {
                mail.From = new MailAddress(emailMsg.Email);

                SmtpClient smtpClient = new SmtpClient("w019cc1a.kasserver.com");
                smtpClient.Port = 587;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new System.Net.NetworkCredential("m051a2c2", "DzFEHDd4pAzdK2yX");
                smtpClient.Send(mail);

                return new JsonResult(emailMsg);
            }
            else
            {
                emailMsg.Email = "false";
                return new JsonResult(emailMsg);
            }            
        }
    }
}