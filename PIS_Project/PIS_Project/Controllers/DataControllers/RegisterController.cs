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