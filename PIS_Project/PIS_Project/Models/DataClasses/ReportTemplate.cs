using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using TemplateEngine.Docx;
using Word = Microsoft.Office.Interop.Word;

namespace PIS_Project.Models.DataClasses
{
    public static class ReportTemplate
    {
        private static Dictionary<KeyValuePair<int, string>, byte[]> _templates
        {
            get
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resourses\");
                var filesPath = Directory.GetFiles(path, "*#*.docx", SearchOption.AllDirectories);
                var result = new Dictionary<KeyValuePair<int, string>, byte[]>();
                foreach (var temp in filesPath)
                {
                    var values = temp.Substring(temp.LastIndexOf('\\')+1).Split(new[] {'#','.' });
                    var key =int.Parse(values[0]);
                    var name = values[1];
                    var stream = File.ReadAllBytes(temp);
                    result.Add(new KeyValuePair<int, string>(key,name),stream);
                }
                return result;
            }
        }
        public static byte[] GetDocByID(int key, Dictionary<string, object> values)
        {
            var fileGroup = _templates.FirstOrDefault(i=>i.Key.Key==key);
            var temp_name = AppDomain.CurrentDomain.BaseDirectory + "\\" + Enumerable.Range(1, 5000).OrderBy(g => Guid.NewGuid()).First() + ".docx";
            File.WriteAllBytes(temp_name, fileGroup.Value);

            var valuesToFill = new Content();
            foreach (var field in values)
            {
                if ((!(field.Value is IEnumerable<string>)) && (!(field.Value.GetType().IsArray)))
                {
                    if (field.Value is DateTime)
                        valuesToFill.Fields.Add(new FieldContent(field.Key, ((DateTime)field.Value).ToString("d MMMM yyyy")));
                    else
                        valuesToFill.Fields.Add(new FieldContent(field.Key, field.Value.ToString()));
                }
                else
                {
                    if (field.Value is byte[])
                        valuesToFill.Images.Add((new ImageContent(field.Key, (byte[])field.Value)));
                }
            }
            using (var outputDoc = new TemplateProcessor(temp_name).SetRemoveContentControls(true))
            {
                outputDoc.FillContent(valuesToFill);
                outputDoc.SaveChanges();
            }
            var res = File.ReadAllBytes(temp_name);
            File.Delete(temp_name);
            return res;
        }
        public static byte[] GetDocByName(string name, Dictionary<string, object> values)
        {
            return GetDocByID(_templates.FirstOrDefault(i => i.Key.Value == name).Key.Key, values);
        }
    }
}