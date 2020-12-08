using Microsoft.AspNet.Identity;
using PIS_Project.Models.DataClasses;
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace PIS_Project.Controllers.DataControllers
{
    [Serializable]
    internal sealed class LoggingAttribute : OnMethodBoundaryAspect
    {
        private object _locker = new object();
        private static Logger _logger = new Logger();
        public string MethodName { get; set; }
        public List<object> Arguments { get; set; }
        public string Result { get; set; }
        public override void OnEntry(MethodExecutionArgs args)
        {
        }
        private static string MethodAction(MethodBase method)
        {
            var action = "Undefined";
            if (method.Name.Contains("Add"))
                action = "Add";
            else 
            {
                if (method.Name.Contains("Update"))
                    action = "Update";
            }
            return action;
        }
        public override void OnSuccess(MethodExecutionArgs args)
        {
            var action = MethodAction(args.Method);
            var mes = "";

            var id_card = -1;
            try
            {
                id_card = ((Card)args.Arguments.GetArgument(0)).ID;
            }
            catch
            {
                id_card = (new CardsController()).Cards.OrderBy(i=>i.ID).First().ID;
            }
            var id_user = HttpContext.Current.User.Identity.GetUserId();
            var dict_type = typeof(Dictionary<string, object>);
            var values = args.Arguments.FirstOrDefault(i => i.GetType() == dict_type);
            if (!action.Contains("Delete"))
            {
                if (action.Contains("Update"))
                {
                    var array = args.Arguments.FirstOrDefault(i => (i).GetType() == typeof(byte[]));
                    if (array != null)
                    {
                        var name = args.Method.GetParameters().First(i => i.ParameterType == typeof(byte[])).Name;
                        mes += $"Property [{name}] set [new blob value]. ";
                    }
                    mes = $"Updating: ";
                }
                else
                    mes = $"Creating: ";
                foreach (var pair in ((Dictionary<string, object>)values))
                {
                    if (pair.Value.GetType() != typeof(byte[]))
                        mes += $"Property [{pair.Key}] set [{pair.Value}]. ";
                    else
                        mes += $"Property [{pair.Key}] set [new blob value]. ";
                }
            }
            else { mes = "Removing."; }
            lock(_locker)
                _logger.WriteInfo(id_card, int.Parse(id_user), mes);
        }
        public override void OnException(MethodExecutionArgs args)
        {
            var action = MethodAction(args.Method);
            var mes = $"Failure in {(action.Contains("Update")?("Update"):action.Contains("Delete")?"Remove":"Creating")}: {args.Exception.Message.Replace("\n"," ")}";
            var id_card = -1;
            try
            {
                id_card = ((Card)args.Arguments.GetArgument(0)).ID;
            }
            catch
            {
            }
            var id_user = HttpContext.Current.User.Identity.GetUserId();
            lock (_locker)
                _logger.WriteError(id_card, int.Parse(id_user), mes);

        }
        private static void AppendCallInformation(MethodExecutionArgs args, StringBuilder stringBuilder)
        {

        }
    }
}