using PIS_Project.Models.DataClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PIS_Project.Models.DataClasses
{
    public class CardsRegister : DbContext
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
            List<Card> filtredCards = new List<Card>();

            foreach (Card card in result)
            {

                if (filters.ContainsKey("sex") && card.sex != (Card.SexAnimal)int.Parse(filters["sex"]))
                {
                    continue;
                }

                if (filters.ContainsKey("status") && card.id_status != int.Parse(filters["status"]))
                {
                    continue;
                }

                if (filters.ContainsKey("animal_size") && (!card.getAnimalTypeValues.ContainsKey("size") || card.getAnimalTypeValues["size"] != filters["animal_size"]))
                {
                    continue;
                }

                if (filters.ContainsKey("animal_species") && (!card.getAnimalTypeValues.ContainsKey("species") || card.getAnimalTypeValues["species"] != filters["animal_species"]))
                {
                    continue;
                }

                if (filters.ContainsKey("animal_hire_size") && (!card.getAnimalTypeValues.ContainsKey("hire_size") || card.getAnimalTypeValues["hire_size"] != filters["animal_hire_size"]))
                {
                    continue;
                }

                if (filters.ContainsKey("animal_hire_type") && (!card.getAnimalTypeValues.ContainsKey("hire_type") || card.getAnimalTypeValues["hire_type"] != filters["animal_hire_type"]))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(filters["birthday1"]) && card.birthday < DateTime.Parse(filters["birthday1"]))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(filters["birthday2"]) && card.birthday > DateTime.Parse(filters["birthday2"]))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(filters["sterilization_date_1"]) && card.birthday < DateTime.Parse(filters["sterilization_date_1"]))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(filters["sterilization_date_2"]) && card.birthday > DateTime.Parse(filters["sterilization_date_2"]))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(filters["status_date_1"]) && card.birthday < DateTime.Parse(filters["change_status_1"]))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(filters["status_date_2"]) && card.birthday > DateTime.Parse(filters["change_status_2"]))
                {
                    continue;
                }

                if (!filters.ContainsKey("owner_traits_5"))
                {
                    if (filters.ContainsKey("owner_traits_1"))
                    {
                        if (!card.getOwnerTraits["collar"])
                        {
                            continue;
                        }
                    }
                    if (filters.ContainsKey("owner_traits_2"))
                    {
                        if (!card.getOwnerTraits["harness"])
                        {
                            continue;
                        }
                    }
                    if (filters.ContainsKey("owner_traits_3"))
                    {
                        if (!card.getOwnerTraits["clothing"])
                        {
                            continue;
                        }
                    }
                    if (filters.ContainsKey("owner_traits_4"))
                    {
                        if (!card.getOwnerTraits["chip"])
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    if (filters.ContainsKey("owner_traits_5") && card.owner_traits != "нет")
                    {
                        continue;
                    }
                }


                filtredCards.Add(card);
            }

            foreach (var card in result)
            {
                foreach (var filtCard in filtredCards)
                {
                    filtCard.Status = GetStatusByID(card.id_status).Name;
                    filtCard.MU = GetMUByID(card.ID_MU).Name;
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
        public CardsRegister()
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
        
    }
}