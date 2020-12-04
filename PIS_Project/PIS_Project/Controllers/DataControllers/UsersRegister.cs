using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PIS_Project.Models.DataClasses;
using System.Diagnostics.Tracing;

namespace PIS_Project.Models.DataControllers
{
    public class UsersRegister:DbContext
    {
        public UsersRegister() 
            : base("DBConnection")
        { }
        private DbSet<Users> Users { get; set; }
        public List<Users> Requests 
        {
            get
            {
                return Users.Where(i => !i.Confirmed).ToList();
            }
        }
        public Users GetRegReqByID(int reqId)
        {
            return Users.FirstOrDefault(i=>i.ID==reqId);
        }
        public Users EditReqRole(int reqID, int newRole)
        {
            try
            {
                var current_user = Users.FirstOrDefault(i => i.ID == reqID);
                current_user.ID_role = newRole;
                SaveChanges();
                return current_user;
            }
            catch(Exception ex) { throw ex; }
        }
        public void ConfirmRegReqByID(int reqID)
        {
            try
            {
                var current_user = Users.FirstOrDefault(i => i.ID == reqID);
                current_user.Confirmed = true;
                SaveChanges();
            }
            catch (Exception ex) { throw ex; }
        }
        public string AddRegReq(Dictionary<string, object> ArrayOfData)
        {
            var validation = ValidationController.CheckValidation((new Users()).GetType(), ArrayOfData);
            if (validation.Result)
            {
                Users.Add((Users)validation.ValidData);
                SaveChanges();
                var res_u = Users.FirstOrDefault(i => i.SIN == ((Users)validation.ValidData).SIN);
                return $"Request was successfully sent with №{res_u.ID}.";
            }
            else { return validation.Information; }
        }
        public Users UpdateUser(int ID_user, Dictionary<string,object> changes)
        {
            var valid = ValidationController.CheckValidation((new Users()).GetType(),changes);
            if (valid.Result)
            {
                var current_user = Users.FirstOrDefault(i => i.ID == ID_user);
                foreach (var change in changes)
                {
                    var prop = current_user.GetType().GetProperty(change.Key);
                    prop.SetValue(current_user, change.Value);
                }
                SaveChanges();
                return Users.FirstOrDefault(i => i.ID == current_user.ID);
            }
            else { throw new ArgumentException(valid.Information); }
        }
    }
}