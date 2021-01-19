﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIS_Project.Models.DataClasses;
using System.Web.Mvc;
using 
    EvoWordToPdf;
using System.IO;
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
            if (ArrayOfData.ContainsKey("SIN"))
            {
                ArrayOfData["SIN"] = ConvertToGuid(ArrayOfData["SIN"].ToString());
            }
            if (ArrayOfData.ContainsKey("password"))
            {
                var password = "";
                var sp_pass = Split(ArrayOfData["password"].ToString(), 16);
                foreach (var p_chunk in sp_pass)
                    password += ConvertToGuid(p_chunk);
                ArrayOfData["password"] = password;
            }
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

        public static Guid ConvertToGuid(string text)
        {
            if (text.Length > 16)
                throw new ArgumentOutOfRangeException("Значение должно быть не больше 16 символов.");
            var con_one = new byte[16];
            var con_ind = con_one.Length - 1;
            for (int i = text.Length - 1; i > -1; i--)
            {
                con_one[con_ind] = (byte)(text[i]);
                con_ind--;
            }
            for (int i = 0; i < con_ind; i++)
                con_one[i] = 0;
            var reslult = new Guid(con_one);
            return reslult;
        }
        static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
    }
}