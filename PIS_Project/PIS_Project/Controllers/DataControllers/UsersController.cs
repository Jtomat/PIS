using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIS_Project.Models.DataClasses;

namespace PIS_Project.Controllers.DataControllers
{
    public class UsersController
    {
        static UsersRegister _register = new UsersRegister();
        public Dictionary<int, Dictionary<string, object>> Requests
        {
            get
            {
                var req = _register.Requests;
                var result = new Dictionary<int, Dictionary<string, object>>();
                foreach (var re in req)
                {
                    result.Add(re.ID, ConvertToOutput(re));
                }
                return result;
            }
        }
        public string AddRegReq(Dictionary<string, object> ArrayOfData)
        {
            return _register.AddRegReq(ArrayOfData);
        }
        public void ConfirmRegReqByID(int reqID)
        {
            _register.ConfirmRegReqByID(reqID);
        }
        public Dictionary<string, object> EditReqRole(int reqID, int newRole)
        {
            return ConvertToOutput(_register.EditReqRole(reqID, newRole));
        }
        public Dictionary<string, object> UpdateUser(int ID_user, Dictionary<string, object> changes)
        {
            return ConvertToOutput(_register.UpdateUser(ID_user, changes));
        }
        private Dictionary<string, object> ConvertToOutput(Users user)
        {
            var result = new Dictionary<string, object>();
            var props = user.GetType().GetProperties();
            foreach (var prop in props)
            {
                result.Add(prop.Name, prop.GetValue(user));
            }
            return result;
        }
    }
}