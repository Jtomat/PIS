using Microsoft.AspNet.Identity;
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace PIS_Project.Models.DataControllers
{
    [Serializable]
    internal sealed class LoggingAttribute : OnMethodBoundaryAspect
    {
        private readonly Exception exception;
        private readonly string method;
        private static Logger _logger = new Logger();
        public Exception Exception { get { return exception; } }
        public string MethodName { get; set; }
        public List<object> Arguments { get; set; }
        public string Result { get; set; }
        public override void OnEntry(MethodExecutionArgs args)
        {
            
            //var stringBuilder = new StringBuilder();

            //stringBuilder.Append("Entering ");
            //AppendCallInformation(args, stringBuilder);
            //Logger.WriteLine(stringBuilder.ToString());

            //Logger.Indent();
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
            var @params = args.Method.GetParameters();
            if (action == "Update")
            {

                var dict_type = typeof(Dictionary<string, object>);
                var id_card = args.Arguments.GetArgument(0);
                var values = args.Arguments.FirstOrDefault(i=>i.GetType()==dict_type);
                var id_user = HttpContext.Current.User.Identity.GetUserId();
                var mes = $"Updating: ";
                foreach (var pair in ((Dictionary<string, object>)values))
                {
                    mes += $"Property {pair.Key} set {pair.Value}. ";
                }

                
            }
            //Logger.Unindent();

            //var stringBuilder = new StringBuilder();

            //stringBuilder.Append("Exiting ");
            //AppendCallInformation(args, stringBuilder);

            //if (!args.Method.IsConstructor && ((MethodInfo)args.Method).ReturnType != typeof(void))
            //{
            //    stringBuilder.Append(" with return value ");
            //    stringBuilder.Append(args.ReturnValue);
            //}

            //Logger.WriteLine(stringBuilder.ToString());
        }
        public override void OnException(MethodExecutionArgs args)
        {
            //Logger.Unindent();

            //var stringBuilder = new StringBuilder();

            //stringBuilder.Append("Exiting ");
            //AppendCallInformation(args, stringBuilder);

            //if (!args.Method.IsConstructor && ((MethodInfo)args.Method).ReturnType != typeof(void))
            //{
            //    stringBuilder.Append(" with exception ");
            //    stringBuilder.Append(args.Exception.GetType().Name);
            //}

            //Logger.WriteLine(stringBuilder.ToString());
        }
        private static void AppendCallInformation(MethodExecutionArgs args, StringBuilder stringBuilder)
        {
            //var declaringType = args.Method.DeclaringType;
            //Formatter.AppendTypeName(stringBuilder, declaringType);
            //stringBuilder.Append('.');
            //stringBuilder.Append(args.Method.Name);

            //if (args.Method.IsGenericMethod)
            //{
            //    var genericArguments = args.Method.GetGenericArguments();
            //    Formatter.AppendGenericArguments(stringBuilder, genericArguments);
            //}

            //var arguments = args.Arguments;

            //Formatter.AppendArguments(stringBuilder, arguments);
        }
    }
}