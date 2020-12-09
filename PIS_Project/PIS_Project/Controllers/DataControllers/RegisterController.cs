using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PIS_Project.Models.DataClasses;


namespace PIS_Project.Controllers.DataControllers
{
    public class RegisterController
    {
        private CardsController Cards;
        public RegisterController()
        {
            Cards = new CardsController();
        }
        public RegisterController(CardsController controller)
        {
            Cards = controller;
        }
        public Dictionary<int, Dictionary<string, object>> GetList(int id_user)
        {
            var users = new UsersRegister().GetUserByID(id_user).ID_organization;
            var preproc = Cards.GetCards().Where(i => i.ID_MU == users).ToList();
            var result = new Dictionary<int, Dictionary<string, object>>();
            var prop = (new Card()).GetType().GetProperties();
            foreach (var card in preproc)
            {
                var dict = new Dictionary<string, object>();
                foreach (var pr in prop)
                {
                    dict.Add(pr.Name, pr.GetValue(card));
                }
                result.Add(card.ID, dict);
            }
            return result;
        }

        [Notify]
        [Logging]
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
                new_card.Status = Cards.GetStatusByID(new_card.id_status).Name;
                new_card.MU = Cards.GetMUByID(new_card.ID_MU).Name;
                Cards.Card.Add(new_card);
                Cards.SaveChanges();
            }
            else { throw new ArgumentException(validation.Information); }
        }
        [Logging]
        public void UpdateCard(int id, Dictionary<string, object> changedValues)
        {
            var validation = ValidationController.CheckValidation((new Card()).GetType(), changedValues);
            if (validation.Result)
            {
                var current_card = Cards.Card.FirstOrDefault(i => i.ID == id);
                foreach (var change in changedValues)
                {
                    var prop = current_card.GetType().GetProperty(change.Key);
                    prop.SetValue(current_card, change.Value);
                }
                Cards.SaveChanges();
            }
            else { throw new ArgumentException(validation.Information); }
        }
        [Logging]
        public void UploadFile(int id, byte[] file)
        {
            UpdateCard(id, (new Dictionary<string, object>() { { "scan_frame", file } }));
        }
        [Logging]
        public void DeleteFile(int id)
        {
            UploadFile(id, new byte[] { });
        }
        public byte[] ExportDoc(int id)
        {
            var card = Cards.GetCardByID(id);
            if (card.document == null)
            {
                UpdateCard(card.ID, new Dictionary<string, object> {
                    {"document",ReportTemplate.GetDocByID(10, new Dictionary<string, object>()
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
            return Cards.GetCardByID(card.ID).document;
        }
        [Logging]
        public void ChangeStatus(int id_card, int id_status)
        {
            UpdateCard(id_card, new Dictionary<string, object>() { { "id_status", id_status } });
        }
        public int[] AvailableStatuses(int id_status)
        {
            if (id_status == 0)
                return new int[0];
            else
            {
                var st = Cards.Status.Where(i => i.ID != 0).ToArray();
                var result = new List<int>();
                foreach (var s in st)
                    result.Add(s.ID);
                return result.ToArray();
            }
        }
        [Logging]
        public void DeleteEntry(int id_card)
        {
            Cards.Card.Remove(Cards.Card.Where(i=>i.ID== id_card).FirstOrDefault());
            Cards.SaveChanges();
        }
    }
}