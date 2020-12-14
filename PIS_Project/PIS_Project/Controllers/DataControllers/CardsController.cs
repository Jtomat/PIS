using PIS_Project.Models.DataClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PIS_Project.Controllers.DataControllers
{
    public class CardsController : DbContext
    {
        internal DbSet<Card> Cards { get; set; }
        internal DbSet<Status> Status { get; private set; }
        internal DbSet<MUS> MUS { get; private set; }
        internal Status GetStatusByID(int id)
        {
            return Status.FirstOrDefault(i => i.ID == id);
        }
        
        internal MUS GetMUByID(int id)
        {
            return MUS.FirstOrDefault(i => i.ID == id);
        }

        public virtual List<Card> GetCards()
        {
            var result = Cards.ToList();
            foreach (var card in result)
            {
                card.Status = GetStatusByID(card.id_status).Name;
                card.MU = GetMUByID(card.ID_MU).Name;
            }
            return result;
        }

        public List<Card> GetFilteredBy(Dictionary<string, string> filters)
        {
            var result = GetCards();
            var prop = (new Card()).GetType().GetProperty(filters["field"]);
            List<Card> filtredCards = new List<Card>();

            result.ForEach(i =>
            {
                string value = (string)prop.GetValue(i);
                if (value != null && value.ToLower().Contains(filters["search"]))
                    filtredCards.Add(i);
            });

            using (var cards = new CardsController())
            {
                foreach (var card in filtredCards)
                {
                    card.Status = GetStatusByID(card.id_status).Name;
                    card.MU = GetMUByID(card.ID_MU).Name;
                }
            }
            return filtredCards;
        }

        public List<Card> GetSortedBy(List<Card> cards, string sortOrder, bool upper)
        {
            var result = cards;
            var prop = (new Card()).GetType().GetProperty(sortOrder);
            if (upper)
                result = result.OrderBy(i => prop.GetValue(i)).ToList();
            else
                result = result.OrderBy(i => prop.GetValue(i)).Reverse().ToList();

            foreach (var card in result)
            {
                card.Status = GetStatusByID(card.id_status).Name;
                card.MU = GetMUByID(card.ID_MU).Name;
            }

            return result;
        }
        public CardsController()
            : base("DBConnection")
        {

            Cards = Set<Card>();
            Status = Set<Status>();
            MUS = Set<MUS>();
        }
        public virtual Card GetCardByID(int id)
        {
            var card = Cards.FirstOrDefault(i => i.ID == id);
            card.Status = GetStatusByID(card.id_status).Name;
            card.MU = GetMUByID(card.ID_MU).Name;
            return card;
        }

        //public System.Data.Entity.DbSet<PIS_Project.Models.DataClasses.Card> Cards { get; set; }

        //public System.Data.Entity.DbSet<PIS_Project.Models.DataClasses.Card> Cards { get; set; }

        //public System.Data.Entity.DbSet<PIS_Project.Models.DataClasses.Card> Cards { get; set; }
    }
}