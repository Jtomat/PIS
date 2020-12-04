using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Reflection;
using PIS_Project.Models.DataClasses;

namespace PIS_Project.Models.DataControllers
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
        public void WriteInfo(int id_card, int id_user,string prop, object value)
        {
            WriteRecord(MessageType.INFO, id_card, id_user, prop, value);
        }
        private void WriteRecord(MessageType message,int id_card, int id_user, string prop, object value)
        {
            var record = new LogRecords()
            {
                Date=DateTime.Now,
                ID_card=id_card,
                ID_user=id_user,
                Changes=$"[{message.ToString("F")}] "
            };
            LogRecords.Add(null);
        }
    }

}