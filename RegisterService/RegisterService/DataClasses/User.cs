using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;

namespace RegisterService.DataClasses
{
    public class User: SoapHeader
    {
        public int ID;
        public string FIO;
        public string Roles;
        public string Email;
        public string Phone;
        public string ORG;
        public bool Confirmed;
        protected internal Stream Doc;
        protected internal Guid SIN;
        protected internal string Password;
        public override bool Equals(object obj)
        {
            var newU = (User)obj;
            if(this.FIO != newU.FIO||
                this.Roles!=newU.Roles||this.Email!=newU.Email||
                this.Phone!=newU.Phone||this.ORG!=newU.ORG)
                return false;
            return true;
        }
    }
}