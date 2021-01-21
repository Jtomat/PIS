using Microsoft.AspNet.Identity;
using PIS_Project.Models.DataClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using PIS_Project.Models.DataControllers;
using System.Web.Mvc;

namespace PIS_Project.Controllers.DataControllers
{
    public class NotificationController: DbContext
    {
        public NotificationController() :
    base("DBConnection")
        {
            Notifications = Set<Notifications>();
        }
        public DbSet<Notifications> Notifications { get; set; }
        public void Log(string logName, string logText,int id_card)
        {
            var d_id = "1";
            if (!string.IsNullOrEmpty((HttpContext.Current.User.Identity.GetUserId())))
            {
                d_id = (HttpContext.Current.User.Identity.GetUserId());
                d_id = (new UsersRegister()).GetIDByName(HttpContext.Current.User.Identity.Name).ToString();
            }
            var notif = new Notifications()
            {
                id_user = int.Parse(d_id),
                id_card = id_card,
                date = DateTime.Now,
                actionType = logName + "|" +logText
            };
            Notifications.Add(notif);
            SaveChanges();
            Notify(notif);
        }
        private void Notify(Notifications log)
        {
            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress("pisprojectsender@gmail.com");

                var admins = (new UsersRegister()).Users.Where(i=>i.ID_role==0);
                foreach(var adm in admins)
                    if(!string.IsNullOrEmpty(adm.email))
                        mail.To.Add(new MailAddress(adm.email));
                mail.Subject = $"Register's notification [{DateTime.Now.ToShortDateString()}-{DateTime.Now.ToShortTimeString()}]";
                using (var users = new UsersRegister())
                    mail.Body = $"Пользователь: [{log.id_user}] {users.GetUserByID(log.id_user).FIO}\n"+
                        $"Номер карты: {log.id_card}\nДействие: {log.actionType}";

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                //client.UseDefaultCredentials = false; 

                //client.Host = "smtp.yandex.ru";
                //client.Port = 587; //не менять ВАЖНО
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                //        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                //        System.Security.Cryptography.X509Certificates.X509Chain chain,
                //        System.Net.Security.SslPolicyErrors sslPolicyErrors) {
                //            return true;
                //        };
                client.Credentials = new NetworkCredential("pisprojectsender@gmail.com", "gbcghjtrn");
                client.EnableSsl = true;
                //if (!string.IsNullOrEmpty(mail.From.DisplayName))
                    client.Send(mail);
            }
        }
    }
}