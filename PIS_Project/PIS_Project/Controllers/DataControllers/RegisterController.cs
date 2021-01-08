using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PIS_Project.Models.DataClasses;
using System.Web.Mvc;
using System.Drawing;
using System.IO;

namespace PIS_Project.Controllers.DataControllers
{
    public class RegisterController : Controller
    {
        public int id_user;

        public ActionResult Index()
        {
            Cards = new CardsRegister();
            ViewBag.Table = GetCards();
            return View();
        }

        [HttpGet]
        public ActionResult ShowRegister(int id)
        {
            id_user = id;
            Cards = new CardsRegister();
            ViewBag.Table = GetList(id);
            ViewBag.Id_User = id;
            var users_role = new UsersRegister().GetUserByID(id).ID_role;
            ViewBag.User_Role = users_role;
            return View();
        }

        [HttpGet]
        public ActionResult ShowRegisterCatched(int user_id)
        {
            id_user = user_id;
            Cards = new RegisterOfCatched();
            var preproc = new RegisterOfCatched().GetCards();
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

            ViewBag.Table = result;
            ViewBag.Id_User = user_id;
            var users_role = new UsersRegister().GetUserByID(user_id).ID_role;
            ViewBag.User_Role = users_role;
            return View();
        }

        [HttpGet]
        public ActionResult RegisterCatchedGetCardByID(int id, int user_id)
        {
            id_user = user_id;
            Cards = new RegisterOfCatched();
            var preproc = new List<Card> { Cards.GetCardByID(id) };
            ViewBag.Id_User = default(int);
            if (id_user != default(int))
            {
                ViewBag.Id_User = id_user;
                var users_role = new UsersRegister().GetUserByID(id_user).ID_role;
                ViewBag.User_Role = users_role;
            }

            var result = new Dictionary<int, Dictionary<string, object>>();
            var prop = (new Card()).GetType().GetProperties();
            foreach (var card in preproc)
            {
                var dict = new Dictionary<string, object>();
                foreach (var pr in prop)
                {
                    if (pr.Name == "type" && pr.GetValue(card) != null)
                    {
                        var type = (int)pr.GetValue(card);
                        dict.Add(pr.Name, type);
                        continue;
                    }
                    if (pr.Name == "date_status_change" && pr.GetValue(card) != null)
                    {
                        var date = DateTime.Parse(pr.GetValue(card).ToString()).ToString("yyyy-MM-dd");
                        dict.Add(pr.Name, date);
                        continue;
                    }
                    if (pr.Name == "birthday" && pr.GetValue(card) != null)
                    {
                        var date = DateTime.Parse(pr.GetValue(card).ToString()).ToString("yyyy-MM-dd");
                        dict.Add(pr.Name, date);
                        continue;
                    }
                    if (pr.Name == "sterilization_date" && pr.GetValue(card) != null)
                    {
                        var date = DateTime.Parse(pr.GetValue(card).ToString()).ToString("yyyy-MM-dd");
                        dict.Add(pr.Name, date);
                        continue;
                    }
                    dict.Add(pr.Name, pr.GetValue(card));
                }
                result.Add(card.ID, dict);
            }
            ViewBag.CardData = result;
            foreach (var b in ViewBag.CardData.Values)
            {
                ViewBag.Card = b;
                continue;
            }
            ViewBag.Id = ViewBag.Id = GetCards().Count + 1;
            ViewBag.MU = new UsersRegister().GetUserByID(id_user).ID_organization;
            ViewBag.Params = new Dictionary<string, object>();
            return View();
        }

        [HttpGet]
        public ActionResult CreateNewCard(int id_user)
        {
            ViewBag.Id_User = default(int);
            if (id_user != default(int))
            {
                ViewBag.Id_User = id_user;
                var users_role = new UsersRegister().GetUserByID(id_user).ID_role;
                ViewBag.User_Role = users_role;
            }
            ViewBag.MU = new UsersRegister().GetUserByID(id_user).ID_organization;
            ViewBag.Id = GetCards().Count + 1;
            ViewBag.Params = new Dictionary<string, object>();
            return View();
        }

        [HttpGet]
        public ActionResult GetCardByID(int id, int id_user)
        {
            var preproc = new List<Card> { Cards.GetCardByID(id) };
            ViewBag.Id_User = default(int);
            if (id_user != default(int))
            {
                ViewBag.Id_User = id_user;
                var users_role = new UsersRegister().GetUserByID(id_user).ID_role;
                ViewBag.User_Role = users_role;
            }
            
            var result = new Dictionary<int, Dictionary<string, object>>();
            var prop = (new Card()).GetType().GetProperties();
            foreach (var card in preproc)
            {
                var dict = new Dictionary<string, object>();
                foreach (var pr in prop)
                {
                    if (pr.Name == "date_status_change" && pr.GetValue(card) != null)
                    {
                        var date = DateTime.Parse(pr.GetValue(card).ToString()).ToString("yyyy-MM-dd");
                        dict.Add(pr.Name, date);
                        continue;
                    }
                    if (pr.Name == "birthday" && pr.GetValue(card) != null)
                    {
                        var date = DateTime.Parse(pr.GetValue(card).ToString()).ToString("yyyy-MM-dd");
                        dict.Add(pr.Name, date);
                        continue;
                    }
                    if (pr.Name == "sterilization_date" && pr.GetValue(card) != null)
                    {
                        var date = DateTime.Parse(pr.GetValue(card).ToString()).ToString("yyyy-MM-dd");
                        dict.Add(pr.Name, date);
                        continue;
                    }
                    dict.Add(pr.Name, pr.GetValue(card));
                }
                result.Add(card.ID, dict);
            }
            ViewBag.CardData = result;
            return View();
        }

        public Dictionary<string, string> fieldsDict = new Dictionary<string, string>
        {
            {"name", "Кличка"},
            {"birthday", "День рождения"},
            {"sex", "Пол"}
        };

        public ActionResult Sort(Dictionary<string, string> filters, string sortOrder, string action, bool upper = false)
        {
            SelectList fields = new SelectList(fieldsDict, "Key", "Value");
            ViewBag.Fields = fields;

            bool checkFilters = false;
            if (filters != null && (filters.Count > 2 || filters.ContainsKey("field")))
            {
                foreach (KeyValuePair<string, string> pair in filters)
                {
                    if (pair.Key != "field" && !string.IsNullOrEmpty(pair.Value))
                        checkFilters = true;
                }
            }


            List<Card> card;
            if (checkFilters)
            {
                card = Cards.GetFilteredBy(filters, action);
            }
            else
                card = Cards.GetCards().ToList().GetRange(0, 3);
            if (!String.IsNullOrEmpty(sortOrder))
            {
                ViewData[sortOrder] = !upper;
                card = Cards.GetSortedBy(card, sortOrder, (bool)ViewData[sortOrder]);
            }
            return View(card);
        }


        //GetCardByID?
        public ActionResult Card(int id_card)
        {
            int id_user = 1;
            ViewBag.Id_User = default(int);
            if (id_user != default(int))
            {
                ViewBag.Id_User = id_user;
                var users_role = new UsersRegister().GetUserByID(id_user).ID_role;
                ViewBag.User_Role = users_role;
            }
            var card = Cards.GetCardByID(id_card);
            ViewBag.Sex = card.sex == Models.DataClasses.Card.SexAnimal.Male ? "Мужской" : "Женский";
            return View(card);
        }

        public ActionResult EditCard(int id_card)
        {

            int id_user = 1;
            int users_role = 0;
            if (id_user != default(int))
            {
                users_role = new UsersRegister().GetUserByID(id_user).ID_role;
                ViewBag.User_Role = users_role;
            }
            if (users_role == 1 || users_role == 2)
            {
                var card = Cards.GetCardByID(id_card);
                return View(card);
            }
            else
            {
                ViewBag.User_Role = users_role;
                return View("Card", Cards.GetCardByID(id_card));
            }
        }

        private CardsRegister Cards;
        public RegisterController()
        {
            Cards = new CardsRegister();
        }
        public RegisterController(CardsRegister controller)
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

        public Dictionary<int, Dictionary<string, object>> GetCards()
        {
            var preproc = Cards.GetCards().ToList();
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

       [Logging]
        [Notify]
        [HttpPost]
        public void AddCard(Dictionary<string, object> values)
        {
            var validation = ValidationController.CheckValidation((new Card()).GetType(), values);
            if (validation.Result)
            {
                var new_card = new Card();
                foreach (var change in values)
                {
                    var prop = new_card.GetType().GetProperty(change.Key);
                    prop.SetValue(new_card, prop.GetValue(validation.ValidData));
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

            if (card.scan_frame_1 == null)
            {
                DeleteFile(card, "scan_frame_1");
            }

            if (card.scan_frame_2 == null)
            {
                DeleteFile(card, "scan_frame_2");
            }

            string ownerTraitString = "";

            Dictionary<string, string> traitsLocal = new Dictionary<string, string>();
            traitsLocal["1"] = "ошейник";
            traitsLocal["2"] = "одежда";
            traitsLocal["3"] = "шлейка";
            traitsLocal["4"] = "чип";

            if (card.setOwnerTraits != null)
            {
                foreach (KeyValuePair<string, bool> pair in card.setOwnerTraits)
                {
                    if (pair.Value)
                    {
                        ownerTraitString += traitsLocal[pair.Key] + ", ";
                    }
                }

                if (ownerTraitString == "")
                {
                    card.owner_traits = "нет";
                }
                else
                {
                    card.owner_traits = ownerTraitString.Remove(ownerTraitString.Length - 2, 2);
                }
            }
            if (card.setAnimalTypeValues != null)
            {
                uint animalType = Convert.ToUInt32($"000{card.setAnimalTypeValues["species"]}{card.setAnimalTypeValues["size"]}{card.setAnimalTypeValues["hire_size"]}{card.setAnimalTypeValues["hire_type"]}", 2);
                card.type = (Card.AnimalType)animalType;
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
                List<string> execFields = new List<string>(
                    new string[] {
                        "stringAnimalType",
                        "getAnimalTypeValue",
                        "setAnimalTypeValues",
                        "setOwnerTraits",
                        "getOwnerTraits",
                    });

                foreach (var change in changedValues)
                {
                    var pro = current_card.GetType().GetProperty(change.Key);
                    if (!execFields.Contains(pro.Name))
                        pro.SetValue(current_card, change.Value);
                }
                Cards.SaveChanges();
                return RedirectToAction("Card", "Register", new { id_card = card.ID });
            }
            else { throw new ArgumentException(validation.Information); }
        }
        //[Logging]
        [HttpPost]
        public string UploadFile()
        {
            HttpPostedFileBase file = Request.Files["FileData"];
            BinaryReader reader = new BinaryReader(file.InputStream);
            return Convert.ToBase64String(reader.ReadBytes((int)file.ContentLength));
        }
        //[Logging]
        public void DeleteFile(Card card, string prop)
        {
            typeof(Card).GetProperty(prop).SetValue(card, new byte[] { });
        }
        public void ExportDoc(int id, int doc_id)
        {
            var card = Cards.GetCardByID(id);

            string wool = "";
            string size = "";

            switch (card.getAnimalTypeValues["size"])
            {
                case "11":
                    size = "Большой";
                    break;
                case "10":
                    size = "Средний";
                    break;
                case "1":
                    size = "Маленький";
                    break;

            }
            switch (card.getAnimalTypeValues["hire_type"])
            {
                case "1":
                    wool = "Прямая";
                    break;
                case "0":
                    wool = "Волнистая";
                    break;
            }

            if (doc_id == 10)
            {
                card.document = ReportTemplate.GetDocByID(doc_id, new Dictionary<string, object>()
                {
                    { "ID_Card", card.ID.ToString() },
                    { "Date", DateTime.Now },
                    { "Sex", (int)card.sex == 1?"М":"Ж" },
                    { "Spec_mark", card.spec_mark },
                    { "Sterilization_date", card.sterilization_date },
                    { "Photo", card.photo!=null?card.photo:new byte[0] },
                    { "Town", card.local_place.Split(' ')[0] },
                    { "Local", card.local_place },
                    { "MU", card.MU },
                    { "ID_mark", card.id_mark.ToString() },
                    { "Date_found", DateTime.Now },
                    { "Wool", wool},
                    { "Size", size},
                });
                UpdateCard(card);

                System.IO.File.WriteAllBytes(@"C:\Users\Анастасия\Desktop\ИСиТ 3 курс\ПИС\7 лаба\Карточка учета животного №" + card.ID.ToString() + ".docx", Cards.GetCardByID(card.ID).document);
            }
            else
            {
                card.document = ReportTemplate.GetDocByID(doc_id, new Dictionary<string, object>()
                {
                    { "ID_Card", card.ID.ToString() },
                    { "Sex", (int)card.sex == 1?"М":"Ж" },
                    { "Birthday", card.birthday.ToString("MM.yyyy")},
                    { "MU", card.MU },
                    { "ID_Chip", card.id_chip.ToString()},
                    { "Wool", wool},
                });
                UpdateCard(card);

                System.IO.File.WriteAllBytes(@"C:\Users\Анастасия\Desktop\ИСиТ 3 курс\ПИС\7 лаба\Акт первичного осмотра животного №" + card.ID.ToString() + ".docx", Cards.GetCardByID(card.ID).document);
            }
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
            Cards.Cards.Remove(Cards.Cards.Where(i=>i.ID== id_card).FirstOrDefault());
            Cards.SaveChanges();
        }
    }
}