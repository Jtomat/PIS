using Microsoft.AspNet.Identity;
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIS_Project.Models.DataClasses
{
    [Serializable]
    public sealed class NotifyAttribute : OnMethodBoundaryAspect
    {
        private object _locker = new object();
        private static Controllers.DataControllers.NotificationController _notificator = new Controllers.DataControllers.NotificationController();
        public override void OnSuccess(MethodExecutionArgs args)
        {
            var id_s = "1";
            var f = HttpContext.Current.User.Identity.GetUserId();
            var d_id = (new UsersRegister()).GetIDByName(HttpContext.Current.User.Identity.Name).ToString();
            if (!string.IsNullOrEmpty(d_id))
                id_s = d_id;
            var id_user = int.Parse(id_s);
            var values = (Dictionary<string,object>)args.Arguments.FirstOrDefault();
            var card_id = (new CardsRegister()).Cards.OrderBy(i => i.ID).ToList();
                card_id.Reverse(); 
            var car = card_id.First().ID;
            _notificator.Log("Добавление", (values[values.Keys.First()] as string[])[0], car);

        }
        public override void OnException(MethodExecutionArgs args)
        {
           
        }
    }
}