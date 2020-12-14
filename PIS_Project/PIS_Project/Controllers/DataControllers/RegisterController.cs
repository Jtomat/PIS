using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIS_Project.Models.DataClasses;
using System.Web.Mvc;
using System.IO;

namespace PIS_Project.Controllers.DataControllers
{
    public class RegisterController : Controller
    {

        public Dictionary<string, string> fieldsDict = new Dictionary<string, string>
        {
            {"name", "Кличка"},
            {"birthday", "День рождения"},
            {"sex", "Пол"}
        };

        public ActionResult Sort(Dictionary<string, string> filters, string sortOrder, bool upper = false)
        {
            SelectList fields = new SelectList(fieldsDict, "Key", "Value");
            ViewBag.Fields = fields;
            List<Card> card;
            if (filters.ContainsKey("field"))
                card = Cards.GetFilteredBy(filters);
            else
                card = Cards.GetCards().ToList().GetRange(0, 3);
            if (!String.IsNullOrEmpty(sortOrder))
            {
                ViewData[sortOrder] = !upper;
                card = Cards.GetSortedBy(card, sortOrder, (bool)ViewData[sortOrder]);
            }
            return View(card);
        }

        public ActionResult Card(int id_card)
        {
            return View(Cards.GetCardByID(id_card));
        }

        public ActionResult EditCard(int id_card)
        {
            return View(Cards.GetCardByID(id_card));
        }

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
        public void AddCard(Dictionary<string, object> values)
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
                Cards.Cards.Add(new_card);
                Cards.SaveChanges();
            }
            else { throw new ArgumentException(validation.Information); }
        }

        //[Logging]
        [HttpPost]
        public RedirectToRouteResult UpdateCard(Card card)
        {
            if (card.photo == null)
            {
                DeleteFile(card, "photo");
            }

            var prop = (new Card()).GetType().GetProperties();
            var changedValues = new Dictionary<string, object>();


            foreach (var pr in prop)
            {
                changedValues.Add(pr.Name, pr.GetValue(card));
            }

            var validation = ValidationController.CheckValidation((new Card()).GetType(), changedValues);
            if (validation.Result)
            {
                var current_card = Cards.Cards.FirstOrDefault(i => i.ID == card.ID);
                foreach (var change in changedValues)
                {
                    var pro = current_card.GetType().GetProperty(change.Key);
                    pro.SetValue(current_card, change.Value);
                }
                Cards.SaveChanges();
                return RedirectToAction("Card", "Register", new { id_card = card.ID });
            }
            else { throw new ArgumentException(validation.Information); }
        }

        // [Logging]
        [HttpPost]
        public String UploadFile(Card card, string prop)
        {
            HttpPostedFileBase file = Request.Files["FileData"];
            BinaryReader reader = new BinaryReader(file.InputStream);

            return Convert.ToBase64String(reader.ReadBytes((int)file.ContentLength));
        }

        // [Logging]
        public void DeleteFile(Card card, string prop)
        {
            typeof(Card).GetProperty(prop).SetValue(card, new byte[] { });
        }

        public void ExportDoc(int id)
        {
            var card = Cards.GetCardByID(id);
            if (card.document == null)
            {
                card.document = ReportTemplate.GetDocByID(10, new Dictionary<string, object>()
                {
                    { "ID_Card", card.ID.ToString() },
                    { "Date", DateTime.Now },
                    { "Sex", card.sex.ToString("F") },
                    { "Spec_mark", card.spec_mark },
                    { "Sterilization_date", card.sterilization_date },
                    { "Photo", card.photo!=null?card.photo:new byte[0] },
                    { "Town", card.local_place.Split(' ')[0] },
                    { "Local", card.local_place },
                    { "MU", card.MU },
                    { "ID_mark", card.id_mark.ToString() },
                    { "Date_found", DateTime.Now }
                });
                UpdateCard(card);
            }

            System.IO.File.WriteAllBytes(@"C:\Users\Анастасия\Desktop\ИСиТ 3 курс\ПИС\7 лаба\apple.docx", Cards.GetCardByID(card.ID).document);

        }

        [Logging]
        public void ChangeStatus(int id_card, int id_status)
        {
            //UpdateCard(id_card, new Dictionary<string, object>() { { "id_status", id_status } });
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
            Cards.Cards.Remove(Cards.Cards.Where(i => i.ID == id_card).FirstOrDefault());
            Cards.SaveChanges();
        }
    }
}