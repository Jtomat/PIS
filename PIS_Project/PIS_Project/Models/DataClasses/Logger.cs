using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Reflection;
using PIS_Project.Models.DataClasses;

namespace PIS_Project.Models.DataClasses
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
        {
            LogRecords = Set<LogRecords>();
        }

        public DbSet<LogRecords> LogRecords { get; set; }
        /// <summary>
        /// Запись информационного сообщения в лог
        /// </summary>
        /// <param name="id_card">Номер карты</param>
        /// <param name="id_user">Номер пользователя</param>
        /// <param name="toLogging">Сообщение для записи</param>
        public void WriteInfo(int id_card, int id_user, string toLogging)
        {
            WriteRecord(MessageType.INFO, id_card, id_user,toLogging);
        }
        /// <summary>
        /// Запись предупреждения в лог
        /// </summary>
        /// <param name="id_card">Номер карты</param>
        /// <param name="id_user">Номер пользователя</param>
        /// <param name="toLogging">Сообщение для записи</param>
        public void WriteWarning(int id_card, int id_user, string toLogging)
        {
            WriteRecord(MessageType.WARN, id_card, id_user, toLogging);
        }
        /// <summary>
        /// Запись ошибки в лог
        /// </summary>
        /// <param name="id_card">Номер карты</param>
        /// <param name="id_user">Номер пользователя</param>
        /// <param name="toLogging">Сообщение для записи</param>
        public void WriteError(int id_card, int id_user, string toLogging)
        {
            WriteRecord(MessageType.ERROR, id_card, id_user, toLogging);
        }
        /// <summary>
        /// Запись сообщения в лог
        /// </summary>
        /// <param name="message">Тип сообщения</param>
        /// <param name="id_card">Номер карты</param>
        /// <param name="id_user">Номер пользователя</param>
        /// <param name="toLogging">Сообщение для записи</param>
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
                record.ID_card = (new CardsRegister()).Cards.OrderBy(i => i.ID).ToList().Last().ID;
            LogRecords.Add(record);
            SaveChanges();
        }
        /// <summary>
        /// Получить запись по её номеру
        /// </summary>
        /// <param name="id">Номер записи</param>
        /// <returns></returns>
        public LogRecords GetRecordByID(int id)
        {
            return LogRecords.FirstOrDefault(i => i.ID == id);
        }
        /// <summary>
        /// Получить массив записей между двумя датами
        /// </summary>
        /// <param name="from">Дата начала</param>
        /// <param name="to">Дата оканчания</param>
        /// <returns></returns>
        public LogRecords[] GetrecordsByDates(DateTime from, DateTime to)
        {
            return LogRecords.Where(i => i.Date >= from && i.Date <= to).ToArray();
        }
    }

}