using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using RegisterService.DataClasses;
namespace RegisterService
{
    /// <summary>
    /// Сводное описание для Register
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]
    public class Register : System.Web.Services.WebService
    {
        public readonly User authentification;
        public UsersRegister UsersRegister;

        [WebMethod]
        [SoapHeader("authentification")]
        public bool CheckAuth()
        {
            (User).IsInRole("");
            UsersRegister = new UsersRegister(Context);//,Guid.Parse("5f02214b-d629-4394-a9b1-f82db0a7d9fb"),"1");
            return (User).Identity.IsAuthenticated;
        }
        [WebMethod]
        public string HelloWorld()
        {
            return "Привет всем!";
        }
    }
}
