using PIS_Project.Models.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PIS_Project.Controllers.DataControllers;

namespace PIS_Project.Controllers
{
    public class UsersController : Controller
    {
        UsersRegister db = new UsersRegister();
        // GET: Users
        [Route("Users/UsersList")]
        public ActionResult Index()
        {
            //var id_user = (new PIS_Project.Models.DataClasses.UsersRegister()).GetIDByName(HttpContext.User.Identity.Name); //Временно!!!
            var id_user = 8;
            var user = new UsersRegister().GetUserByID(id_user);
            if (user.ID_role == 0)
            {
                ViewBag.List = db.Requests;
                return View("UsersList");
            }
            else
            {
                return RedirectToAction("Index", "Register");
            }
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