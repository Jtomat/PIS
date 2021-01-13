using PIS_Project.Models.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIS_Project.Controllers
{
    public class UsersController : Controller
    {
        UsersRegister db = new UsersRegister();
        // GET: Users
        [Route("Users/UsersList")]
        public ActionResult Index()
        {

            ViewBag.List = db.Requests;
            return View("UsersList");
        }
        [HttpPost]
        public ActionResult Confirm(int ID, int role)
        {
            db.EditReqRole(ID, role);
            db.ConfirmRegReqByID(ID);
            return Index();
        }
    }
}