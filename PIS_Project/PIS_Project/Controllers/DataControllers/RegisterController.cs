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
    /// <summary>
    /// План
    /// (подтянуть изменения Миши), продолжить отлаживать, сесть за курсач
    /// </summary>




    public class RegisterController : Controller
    {
        public int id_user;

        public ActionResult WaitingRoom()
        {
            return View();
        }

        public ActionResult Index(Dictionary<string, string> filters, string sortField, string act = "filtering", bool upper = false)
        {
            //var id_user = (new PIS_Project.Models.DataClasses.UsersRegister()).GetIDByName(HttpContext.User.Identity.Name); //Временно!!!
            var id_user = 8;
            var user = new UsersRegister().GetUserByID(id_user);
            if (user.Confirmed == true)
            {
                SelectList fields = new SelectList(fieldsDict, "Key", "Value");
                ViewBag.Fields = fields;

                bool checkFilters = false;
                if (filters != null && filters.Count > 2)
                {
                    foreach (KeyValuePair<string, string> pair in filters)
                    {
                        if (pair.Key != "field" && !string.IsNullOrEmpty(pair.Value))
                            checkFilters = true;
                    }
                }
                if (act == "reset")
                {
                    Session["savedFilt"] = new Dictionary<string, string>();
                }

                Dictionary<string, string> savedFilt = (Dictionary<string, string>)Session["savedFilt"];

                var ur = new UsersRegister();
                ViewBag.Role = user.ID_role.ToString();
                List<Card> card;
                if (checkFilters)
                {
                    card = Cards.GetFilteredBy(filters).Where(a => a.local_place ==
                    ur.Organizations.FirstOrDefault(i=>i.ID== user.ID_organization).Contacts && a.ID_MU == 4).ToList();
                }
                else if (savedFilt != null && savedFilt.Count > 0)
                {
                    card = Cards.GetFilteredBy(savedFilt).Where(a => a.local_place ==
                    ur.Organizations.FirstOrDefault(i => i.ID == user.ID_organization).Contacts && a.ID_MU == 4).ToList();
                }
                else
                    card = Cards.GetCards().Where(a => a.local_place ==
                    ur.Organizations.FirstOrDefault(i => i.ID == user.ID_organization).Contacts && a.ID_MU == 4).ToList();

                if (!String.IsNullOrEmpty(sortField))
                {
                    ViewData[sortField] = !upper;
                    card = Cards.GetSortedBy(card, sortField, (bool)ViewData[sortField]).Where(a => a.local_place ==
                    ur.Organizations.FirstOrDefault(i => i.ID == user.ID_organization).Contacts && a.ID_MU == 4).ToList();
                }
                return View(card);
            }
            else
            {
                return View("WaitingRoom");
            }
        }

        public ActionResult ShowRegister(Dictionary<string, string> filters, string sortField, string act = "filtering", bool upper = false)
        {
            //var id_user = (new PIS_Project.Models.DataClasses.UsersRegister()).GetIDByName(HttpContext.User.Identity.Name); //Временно!!!
            var id_user = 8;
            var ur = new UsersRegister();
            var user = ur.GetUserByID(id_user);
            if (user.Confirmed == true)
            {
                SelectList fields = new SelectList(fieldsDict, "Key", "Value");
            ViewBag.Fields = fields;

            bool checkFilters = false;
            if (filters != null && filters.Count > 2)
            {
                foreach (KeyValuePair<string, string> pair in filters)
                {
                    if (pair.Key != "field" && !string.IsNullOrEmpty(pair.Value))
                        checkFilters = true;
                }
            }
            if (act == "reset")
            {
                Session["savedFilt"] = new Dictionary<string, string>();
            }

            Dictionary<string, string> savedFilt = (Dictionary<string, string>)Session["savedFilt"];

            ViewBag.Role = user.ID_role.ToString();
            List<Card> card;
            if (checkFilters)
            {
                card = Cards.GetFilteredBy(filters).Where(a => a.local_place ==
                    ur.Organizations.FirstOrDefault(i => i.ID == user.ID_organization).Contacts && a.ID_MU == 4).ToList();
            }
            else if (savedFilt != null && savedFilt.Count > 0)
            {
                card = Cards.GetFilteredBy(savedFilt).Where(a => a.local_place ==
                    ur.Organizations.FirstOrDefault(i => i.ID == user.ID_organization).Contacts && a.ID_MU == 4).ToList();
            }
            else
                card = Cards.GetCards().Where(a => a.local_place ==
                    ur.Organizations.FirstOrDefault(i => i.ID == user.ID_organization).Contacts && a.ID_MU == 4).ToList();

            if (!String.IsNullOrEmpty(sortField))
            {
                ViewData[sortField] = !upper;
                card = Cards.GetSortedBy(card, sortField, (bool)ViewData[sortField]).Where(a => a.ID_MU == user.ID_organization).ToList();
            }
            return View(card);
            }
            else
            {
                return View("WaitingRoom");
            }
        }


        public ActionResult ShowRegisterCatched(Dictionary<string, string> filters, string sortField, string act = "filtering", bool upper = false)
        {
            //var id_user = (new PIS_Project.Models.DataClasses.UsersRegister()).GetIDByName(HttpContext.User.Identity.Name); //Временно!!!
            var id_user = 8;
            var user = new UsersRegister().GetUserByID(id_user);
            if (user.Confirmed == true)
            {
                ViewBag.User_Role = user.ID_role.ToString();
            ViewBag.Role = user.ID_role.ToString();
            bool checkFilters = false;
            if (filters != null && filters.Count > 2 )
            {
                foreach (KeyValuePair<string, string> pair in filters)
                {
                    if (pair.Key != "field" && !string.IsNullOrEmpty(pair.Value))
                        checkFilters = true;
                }
            }
            var CatchedCards = new RegisterOfCatched();
            if (act == "reset")
            {
                Session["savedFilt"] = new Dictionary<string, string>();
            }

            Dictionary<string, string> savedFilt = (Dictionary<string, string>)Session["savedFilt"];

            List<Card> card;
            if (checkFilters)
            {
                card = CatchedCards.GetFilteredBy(filters).Where(c => c.Added != true).ToList();
            }
            else if (savedFilt != null && savedFilt.Count > 0)
            {
                card = CatchedCards.GetFilteredBy(savedFilt).Where(c => c.Added != true).ToList();
            }
            else
                card = CatchedCards.GetCards().Where(c => c.Added != true).ToList();
            
            if (!String.IsNullOrEmpty(sortField))
            {
                ViewData[sortField] = !upper;
                card = CatchedCards.GetSortedBy(card, sortField, (bool)ViewData[sortField]).Where(c => c.Added != true).ToList();
            }

            return View("ShowRegisterCatched", card);
            }
            else
            {
                return View("WaitingRoom");
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            //var id_user = (new PIS_Project.Models.DataClasses.UsersRegister()).GetIDByName(HttpContext.User.Identity.Name); //Временно!!!
            var id_user = 8;
            ViewBag.Id_User = default(int);
            if (id_user != -1)
            {
                ViewBag.Id_User = id_user;
                var users_role = new UsersRegister().GetUserByID(id_user).ID_role;
                ViewBag.User_Role = users_role;
                ViewBag.MU = new UsersRegister().GetUserByID(id_user).ID_organization;
                
                var user = new UsersRegister().GetUserByID(id_user);
                if (user.Confirmed == true)
                {
                    ViewBag.Role = user.ID_role.ToString();
                }
                else
                {
                    return View("WaitingRoom");
                }
            }
            else
            {
                throw new ArgumentException("Незареганным пользователям запрещено создавать карты");
            }
            
            ViewBag.Params = new Dictionary<string, object>();
            return View();
        }

        public Dictionary<string, string> fieldsDict = new Dictionary<string, string>
        {
            {"name", "Кличка"},
            {"birthday", "День рождения"},
            {"sex", "Пол"}
        };

        public ActionResult Sort(Dictionary<string, string> filters, string sortField, string act = "filtering", bool upper = false)
        {
            bool checkFilters = false;
            if (filters != null && filters.Count > 2)
            {
                foreach (KeyValuePair<string, string> pair in filters)
                {
                    if (pair.Key != "field" && !string.IsNullOrEmpty(pair.Value))
                        checkFilters = true;
                }
            }

            if (act == "reset")
            {
                Session["savedFilt"] = new Dictionary<string, string>();
            }

            Dictionary<string, string> savedFilt = (Dictionary<string, string>)Session["savedFilt"];
            List<Card> card;
            if (checkFilters)
            {
                card = Cards.GetFilteredBy(filters);
                Session["savedFilt"] = filters;
            }
            else if (savedFilt != null && savedFilt.Count > 0)
            {
                card = Cards.GetFilteredBy(savedFilt);
            }
            else
                card = Cards.GetCards().ToList();

            if (!String.IsNullOrEmpty(sortField))
            {
                ViewData[sortField] = !upper;
                card = Cards.GetSortedBy(card, sortField, (bool)ViewData[sortField]);
            }

            return View("Sort", card);
        }


        //GetCardByID?
        public ActionResult Card(int id_card)
        {
            //var id_user = (new PIS_Project.Models.DataClasses.UsersRegister()).GetIDByName(HttpContext.User.Identity.Name); //Временно!!!
            int id_user = 8;
            ViewBag.Id_User = default(int);
            if (id_user != default(int))
            {
                ViewBag.Id_User = id_user;
                var users_role = new UsersRegister().GetUserByID(id_user).ID_role;
                ViewBag.User_Role = users_role;
                var user = new UsersRegister().GetUserByID(id_user);
                if (user.Confirmed == true)
                {
                    ViewBag.Role = user.ID_role.ToString();
                }
                else
                {
                    return View("WaitingRoom");
                }
            }
            var card = Cards.GetCardByID(id_card);
            ViewBag.Sex = card.sex == Models.DataClasses.Card.SexAnimal.Male ? "Мужской" : "Женский";
            return View(card);
        }

        public ActionResult EditCard(int id_card)
        {

            int id_user = 8;
            int users_role = 0;
            if (id_user != default(int))
            {
                users_role = new UsersRegister().GetUserByID(id_user).ID_role;
                ViewBag.User_Role = users_role;
                var user = new UsersRegister().GetUserByID(id_user);
                if (user.Confirmed == true)
                {
                    ViewBag.Role = user.ID_role.ToString();
            }
            else
            {
                return View("WaitingRoom");
            }
        }
            if (users_role == 0 || users_role == 1 || users_role == 2)
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

        [HttpGet]
        public ActionResult CatchedCardCreate(int id_card)
        {
            //var id_user = (new PIS_Project.Models.DataClasses.UsersRegister()).GetIDByName(HttpContext.User.Identity.Name); //Временно!!!
            int id_user = 8;
            var card = Cards.GetCardByID(id_card);
            ViewBag.Id_User = default(int);
            if (id_user != default)
            {
                ViewBag.Id_User = id_user;
                var users_org = new UsersRegister().GetUserByID(id_user).ID_organization;
                ViewBag.User_Org = users_org;
                card.ID_MU = users_org;
                var user = new UsersRegister().GetUserByID(id_user);
                if (user.Confirmed == true)
                {
                    ViewBag.Role = user.ID_role.ToString();
                }
                else
                {
                    return View("WaitingRoom");
                }
            }
            card.Added = true;
            ViewBag.Card = card;
            ViewBag.Sex = card.sex == Models.DataClasses.Card.SexAnimal.Male ? "Мужской" : "Женский";
            Session.Add("old_card_id", id_card);
            Session.Add("newcard", card);
            return View(card);
        }

        [HttpGet]
        public ActionResult CatchedCard()
        {
            var newcard = Session["newcard"] as Card;
            var card = newcard;
            ViewBag.Card = newcard;
            return View(card);
        }

        [HttpPost]
        public ActionResult CatchedCard(Card newcard)
        {
            //var id_user = (new PIS_Project.Models.DataClasses.UsersRegister()).GetIDByName(HttpContext.User.Identity.Name); 
            //Временно!!!
            int id_user = 8;
            ViewBag.Id_User = default(int);
            if (id_user != default)
            {
                ViewBag.Id_User = id_user;
                var users_org = new UsersRegister().GetUserByID(id_user).ID_organization;
                ViewBag.User_Org = users_org;
                newcard.ID_MU = users_org;
                var user = new UsersRegister().GetUserByID(id_user);
                if (user.Confirmed == true)
                {
                }
                else
                {
                    return View("WaitingRoom");
                }
            }

            if (newcard.photo == null)
            {
                DeleteFile(newcard, "photo");
            }

            if (newcard.scan_frame_1 == null)
            {
                DeleteFile(newcard, "scan_frame_1");
            }

            //if (newcard.scan_frame_2 == null)
            //{
            //    DeleteFile(newcard, "scan_frame_2");
            //}

            string ownerTraitString = "";

            Dictionary<string, string> traitsLocal = new Dictionary<string, string>();
            traitsLocal["1"] = "ошейник";
            traitsLocal["2"] = "одежда";
            traitsLocal["3"] = "шлейка";
            traitsLocal["4"] = "чип";

            if (newcard.setOwnerTraits != null)
            {
                foreach (KeyValuePair<string, bool> pair in newcard.setOwnerTraits)
                {
                    if (pair.Value)
                    {
                        ownerTraitString += traitsLocal[pair.Key] + ", ";
                    }
                }

                if (ownerTraitString == "")
                {
                    newcard.owner_traits = "нет";
                }
                else
                {
                    newcard.owner_traits = ownerTraitString.Remove(ownerTraitString.Length - 2, 2);
                }
            }
            if (newcard.setAnimalTypeValues != null)
            {
                uint animalType = Convert.ToUInt32($"000{newcard.setAnimalTypeValues["species"]}{newcard.setAnimalTypeValues["size"]}{newcard.setAnimalTypeValues["hire_size"]}{newcard.setAnimalTypeValues["hire_type"]}", 2);
                newcard.type = (Card.AnimalType)animalType;
            }

            var prop = (new Card()).GetType().GetProperties();
            var changedValues = new Dictionary<string, object>();

            foreach (var pr in prop)
            {
                changedValues.Add(pr.Name, pr.GetValue(newcard));
            }

            //var validation = ValidationController.CheckValidation((new Card()).GetType(), changedValues);
            //if (validation.Result)
            //{
                ViewBag.Card = newcard;
                var card = newcard;
            Session["newcard"]= card;
            ViewBag.Sex = card.sex == Models.DataClasses.Card.SexAnimal.Male ? "Мужской" : "Женский";
                return View(card);
            //}
            //else
            //{
            //    throw new ArgumentException(validation.Information);
            //}
        }

        [Logging]
        [Notify]
        [HttpPost]
        public ActionResult CatchedCardAdd(Card card, string action)
        {
            var somecard = Session["newcard"] as Card; 
                var prop = (new Card()).GetType().GetProperties();
                var changedValues = new Dictionary<string, object>();

                foreach (var pr in prop)
                {
                    if (pr.GetValue(card) != null)
                    {
                        if (pr.Name != "getOwnerTraits" && pr.Name != "stringAnimalType" && pr.Name != "getAnimalTypeValues" && pr.Name != "photo")
                            changedValues.Add(pr.Name, pr.GetValue(card));
                        if (pr.Name == "photo")
                        {
                            var el = pr.GetValue(card);
                            changedValues.Add(pr.Name, card.photo);
                        }
                    }

                    //if(pr.Name == "getOwnerTraits")
                    //    changedValues.Add("setOwnerTraits", pr.GetValue(card));
                }

                var validation = ValidationController.CheckValidation((new Card()).GetType(), changedValues);
                if (validation.Result)
                {
                    var new_card = card;
                    new_card.Status = Cards.GetStatusByID(new_card.id_status).Name;
                    new_card.MU = Cards.GetMUByID(new_card.ID_MU).Name;
                var catch_card = Cards.GetCardByID(int.Parse(Session["old_card_id"].ToString()));
                catch_card.Added = true;
                    Cards.Cards.Add(new_card);
                    Cards.SaveChanges();
                    Session.Remove("old_card_id");
                    Session.Remove("newcard");
                    return RedirectToAction("Sort");
                }
                else { throw new ArgumentException(validation.Information); }
        }

        [HttpGet]
        public ActionResult EditCatchedCard()
        {
            var newcard = Session["newcard"] as Card;
            var card = newcard;
            ViewBag.Card = newcard;
            return View(card);
        }

        //[Logging]
        //[Notify]
        [HttpPost]
        public ActionResult Create(Dictionary<string, object> values)
        {
            string ownerTraitString = "";

            if (values["SetOwnerTraits"] != null )
            {
                foreach (var val in values["SetOwnerTraits"] as string[])
                {
                        ownerTraitString += val.ToLower() + ", ";
                }

                if (ownerTraitString == "")
                {
                    values["owner_traits"] = "нет";
                }
                else
                {
                    values["owner_traits"] = ownerTraitString.Remove(ownerTraitString.Length - 2, 2);
                }
            }
            values.Remove("SetOwnerTraits");
            if ((values["id_chip"] as string[])[0] == "")
            {
                values.Remove("id_chip");
            }
            if ((values["id_mark"] as string[])[0] == "")
            {
                values.Remove("id_mark");
            }
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
                new_card.Added = true;
                Cards.Cards.Add(new_card);
                Cards.SaveChanges();
                return RedirectToAction("Sort");
            }
            else { throw new ArgumentException(validation.Information); }
        }

        //[Logging]
        [HttpPost]
        public RedirectToRouteResult UpdateCard(Card card)
        {
            if (card.photo == null)
            {
                try
                {
                    DeleteFile(card, "photo");
                }
                catch { }
            }

            if (card.scan_frame_1 == null)
            {
                try
                {
                    DeleteFile(card, "scan_frame_1");
                }
                catch { }
            }

            /*if (card.scan_frame_2 == null)
            {
                try
                {
                    DeleteFile(card, "scan_frame_2");
                }
                catch { }
            }*/

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
                string size = card.setAnimalTypeValues["size"] == "1" ? "0" + card.setAnimalTypeValues["size"] : card.setAnimalTypeValues["size"];
                uint animalType = Convert.ToUInt32($"000{card.setAnimalTypeValues["species"]}{size}{card.setAnimalTypeValues["hire_size"]}{card.setAnimalTypeValues["hire_type"]}", 2);
                card.type = (Card.AnimalType)animalType;
            }

            var prop = (new Card()).GetType().GetProperties();
            var changedValues = new Dictionary<string, object>();

            foreach (var pr in prop)
            {
                if (pr.GetValue(card) != null)
                {
                    if (pr.Name != "getOwnerTraits" && pr.Name != "stringAnimalType" && pr.Name != "getAnimalTypeValues" && pr.Name != "photo")
                        changedValues.Add(pr.Name, pr.GetValue(card));
                    if (pr.Name == "photo")
                    {
                        var el = pr.GetValue(card);
                        changedValues.Add(pr.Name, card.photo);
                    }
                }

                //if(pr.Name == "getOwnerTraits")
                //    changedValues.Add("setOwnerTraits", pr.GetValue(card));
            }

            var validation = ValidationController.CheckValidation((new Card()).GetType(), changedValues);
            if (validation.Result)
            {
                var id = card.ID;
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
                current_card.Status = Cards.GetStatusByID(current_card.id_status).Name;
                current_card.MU = Cards.GetMUByID(current_card.ID_MU).Name;
                Cards.SaveChanges();

                return RedirectToAction("Card", "Register", new { id_card = id });

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

                System.IO.File.WriteAllBytes(@"Карточка учета животного №" + card.ID.ToString() + ".docx", Cards.GetCardByID(card.ID).document);
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

                System.IO.File.WriteAllBytes(@"Акт первичного осмотра животного №" + card.ID.ToString() + ".docx", Cards.GetCardByID(card.ID).document);
            }
        }
        //[Logging]
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
        [HttpGet]
        public ActionResult Delete(int id_card)
        {
            //var id_user = (new PIS_Project.Models.DataClasses.UsersRegister()).GetIDByName(HttpContext.User.Identity.Name); //Временно!!!
            int id_user = 8;
            ViewBag.Id_User = default(int);
            if (id_user != default(int))
            {
                ViewBag.Id_User = id_user;
                var users_role = new UsersRegister().GetUserByID(id_user).ID_role;
                ViewBag.User_Role = users_role;
                var user = new UsersRegister().GetUserByID(id_user);
                if (user.Confirmed == true)
                {
                    ViewBag.Role = user.ID_role.ToString();
                }
            else
            {
                return View("WaitingRoom");
            }
        }
            var card = Cards.GetCardByID(id_card);
            ViewBag.Sex = card.sex == Models.DataClasses.Card.SexAnimal.Male ? "Мужской" : "Женский";
            return View(card);
        }
        //[Logging]
        [HttpPost]
        public RedirectToRouteResult Delete(int id, bool t = true)
        {
            //var id_user = (new PIS_Project.Models.DataClasses.UsersRegister()).GetIDByName(HttpContext.User.Identity.Name); //Временно!!!
            int id_user = 8;
            if (id_user != default(int))
            {
                ViewBag.Id_User = id_user;
                var user = new UsersRegister().GetUserByID(id_user);
                if (user.Confirmed == true)
                {
                }
                else
                {
                    return RedirectToAction("WaitingRoom");
                }
                var users_role = new UsersRegister().GetUserByID(id_user).ID_role;
                if (users_role == 1 || users_role == 0 || users_role == 2)
                {
                    Cards.Cards.Remove(Cards.Cards.Where(i => i.ID == id).FirstOrDefault());
                    Cards.SaveChanges();
                    return RedirectToAction("Sort");
                }
                else
                {
                    throw new ArgumentException("У вас нет прав на удаление карт");
                }
            }
            else
            {
                throw new ArgumentException("У вас нет прав на удаление карт");
            }
        }
    }
}