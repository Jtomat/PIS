using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PIS_Project.Models.DataClasses;
using System.Diagnostics.Tracing;

namespace PIS_Project.Models.DataClasses
{
    public class UsersRegister:DbContext
    {
        public UsersRegister() 
            : base("DBConnection")
        {
            Users = Set<Users>();
            Organizations = Set<Organizations>();
            Roles = Set<Roles>();
        }
        internal DbSet<Users> Users { get; set; }
        internal DbSet<Organizations> Organizations { get; set; }
        internal DbSet<Roles> Roles { get; set; }
        public Dictionary<int, string> AllRole
        {
            get
            {
                var result = new Dictionary<int, string>();
                foreach (var f in Roles)
                    result.Add(f.ID, f.Name);
                return result;
            }
        }
        public int GetIDByName(string name)
        {
            var us = Users.ToArray();
            var ds = us.FirstOrDefault(i => string.Compare(i.Name, name) == 0);
            if (ds != null)
                return ds.ID;
            return -1;
        }

        public Users GetUserByID(int id)
        {
            var result = Users.Where(i => i.Confirmed).FirstOrDefault(i => i.ID == id);
            if (result != null)
            {
                result.Organization = Organizations.FirstOrDefault(i => i.ID == result.ID_organization).Name;
                result.Role = Roles.FirstOrDefault(i => i.ID == result.ID_role).Name;
            }
            return result;
        }
        public List<Users> Requests 
        {
            get
            {
                var list = Users.Where(i => !i.Confirmed).ToList();
                foreach (var result in list)
                {
                    if (result.ID_organization > -1)
                        result.Organization = Organizations.FirstOrDefault(i => i.ID == result.ID_organization).Name;
                    if (result.ID > -1)
                        result.Role = Roles.FirstOrDefault(i => i.ID == result.ID_role).Name;
                }
                return list;
            }
        }
        public Users GetRegReqByID(int reqId)
        {
            return Requests.FirstOrDefault(i => i.ID == reqId);
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
            var validation = Controllers.DataControllers.ValidationController.CheckValidation((new Users()).GetType(), ArrayOfData);
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
            var valid = Controllers.DataControllers.ValidationController.CheckValidation((new Users()).GetType(),changes);
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