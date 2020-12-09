using PIS_Project.Models.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIS_Project.Controllers.DataControllers
{
    public class RegisterOfCatched: CardsController
    {
        public override List<Card> GetCards()
        { 
            var result =  Card.Where(i=>MUS.FirstOrDefault(j=>j.ID==i.ID_MU).IsCatchingOrg).ToList();
            foreach (var card in result)
            {
                card.Status = GetStatusByID(card.id_status).Name;
                card.MU = GetMUByID(card.ID_MU).Name;
            }
            return result;
        }
        public override Card GetCardByID(int id)
        {
            var card = GetCards().FirstOrDefault(i => i.ID == id);
            card.Status = GetStatusByID(card.id_status).Name;
            card.MU = GetMUByID(card.ID_MU).Name;
            return card;
        }
    }
}