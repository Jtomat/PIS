using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PIS_Project.Models.DataClasses;

namespace PIS_Project.Models.DataControllers
{
    public class CardRegister : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        internal DbSet<Status> Status { get; private set; }
        internal DbSet<MUS> MUS { get; private set; }
        public List<Card> GetList(int id_user)
        {
            return null;
        }
        internal Status GetStatusByID(int id)
        {
            return Status.FirstOrDefault(i=>i.ID==id);
        }
        internal MUS GetMUByID(int id)
        {
            return MUS.FirstOrDefault(i => i.ID == id);
        }
        public List<Card> GetFilteredBy(Dictionary<string,object> filters)
        {
            var result = Cards.ToList();
            foreach (var filter in filters)
            {
                var prop = (new Card()).GetType().GetProperty(filter.Key);
                result = result.Where(i=>prop.GetValue(i)==filter.Value).ToList();
            }
            return result;
        }
        public List<Card> GetSortedBy(Dictionary<string, bool> filters)
        {
            var result = Cards.ToList();
            foreach (var filter in filters)
            {
                var prop = (new Card()).GetType().GetProperty(filter.Key);
                if(filter.Value)
                    result = result.OrderBy(i => prop.GetValue(i)).ToList();
                else
                    result = result.OrderBy(i => prop.GetValue(i)).Reverse().ToList();
            }
            return result;
        }
        public CardRegister()
            : base("DBConnection") { }
        public Card GetCardByID(int id)
        {
           return Cards.FirstOrDefault(i=>i.ID==id);
        }
        internal void AddCard(Dictionary<string, object> values)
        {
            var validation = ValidationController.CheckValidation((new Card()).GetType(), values);
            if (validation.Result)
            {
                var new_card = new Card();
                foreach (var change in values)
                {
                    var prop = new_card.GetType().GetProperty(change.Key);
                    prop.SetValue(new_card, change.Value);
                }
                SaveChanges();
            }
            else { throw new ArgumentException(validation.Information); }
        }
        public void UpdateCard(Card cardNewData,Dictionary<string,object> changedValues)
        {
            var validation = ValidationController.CheckValidation((new Card()).GetType(), changedValues);
            if (validation.Result)
            {
                var current_card = Cards.FirstOrDefault(i => i.ID == cardNewData.ID);
                foreach (var change in changedValues)
                {
                    var prop = current_card.GetType().GetProperty(change.Key);
                    prop.SetValue(current_card, change.Value);
                }
                SaveChanges();
            }
            else { throw new ArgumentException(validation.Information); }
        }
        public void UploadFile(Card cardNewData, byte[] file)
        {
            UpdateCard(cardNewData,(new Dictionary<string, object>() { { "document", file } }));        
        }
        public void DeleteFile(Card cardNewData)
        {
            UploadFile(cardNewData,new byte[] { });
        }
        public byte[] ExportDoc(Card card)
        {
            if (card.document == null)
            {
                UpdateCard(card, new Dictionary<string, object> {
                    {"document",ReportTemplate.GetDoc(10, new Dictionary<string, object>()
                    {
                        {"Card_ID",card.ID },
                        {"Date",DateTime.Now },
                        {"Sex", card.sex.ToString("F")},
                        {"Spec_mark",card.spec_mark },
                        {"Sterilization_date",card.sterilization_date },
                        {"Photo",card.photo },
                        {"Town",card.local_place.Split(' ')[0] },
                        {"Local",card.local_place },
                    })
                    }
                });

            }
            return GetCardByID(card.ID).document; 
        }
    }
}