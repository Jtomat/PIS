using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Reflection;
using PIS_Project.Models.DataClasses;

namespace PIS_Project.Controllers.DataControllers
{
    /// <summary>
    /// Класс, осуществляющий запись данных в Log.
    /// </summary>
    public class Logger : DbContext
    {
        private enum MessageType:int 
        {
            INFO=1,
            WARN=2,
            ERROR=4
        }
        public Logger()
            : base("DBConnection")
        { }
        public DbSet<LogRecords> LogRecords { get; set; }
        public void WriteInfo(int id_card, int id_user, string toLogging)
        {
            WriteRecord(MessageType.INFO, id_card, id_user,toLogging);
        }
        public void WriteWarning(int id_card, int id_user, string toLogging)
        {
            WriteRecord(MessageType.WARN, id_card, id_user, toLogging);
        }
        public void WriteError(int id_card, int id_user, string toLogging)
        {
            WriteRecord(MessageType.ERROR, id_card, id_user, toLogging);
        }
        private void WriteRecord(MessageType message,int id_card, int id_user,string toLogging)
        {
            var record = new LogRecords()
            {
                Date=DateTime.Now,
                ID_user=id_user,
                Changes=$"[{message.ToString("F")}] {toLogging}"
            };
            if (id_card != -1)
                record.ID_card = id_card;
            else
                record.ID_card = (new CardsController()).Cards.OrderBy(i => i.ID).ToList().Last().ID;
            LogRecords.Add(record);
            SaveChanges();
        }
    }

}