using Microsoft.AspNet.Identity;
using PIS_Project.Models.DataClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace PIS_Project.Controllers.DataControllers
{
    public class NotificationController:DbContext
    {
        public DbSet<Notifications> Notifications { get; set; }
        public void Log(string logName, string logText,int id_card)
        {
            var notif = new Notifications()
            {
                id_user = int.Parse(HttpContext.Current.User.Identity.GetUserId()),
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
                mail.From = new MailAddress("PISOriject@yandex.ru");
                var admins = (new UsersRegister()).Users.Where(i=>i.ID_role==0);
                foreach(var adm in admins)
                    if(!string.IsNullOrEmpty(adm.email))
                        mail.To.Add(new MailAddress(adm.email));
                mail.Subject = $"Register's notification [{DateTime.Now.ToShortDateString()}-{DateTime.Now.ToShortTimeString()}]";
                using (var users = new UsersRegister())
                    mail.Body = $"Пользователь: [{log.id_user}] {users.GetUserByID(log.id_user).FIO}\n"+
                        $"Номер карты: {log.id_card}\nДействие: {log.actionType}";
                
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.yandex.ru";
                client.Port = 587; //не менять ВАЖНО
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("PISOriject@yandex.ru", "nugjgbc"); 
                client.Send(mail);
            }
        }
    }
}