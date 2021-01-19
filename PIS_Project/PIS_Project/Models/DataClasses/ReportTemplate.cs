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
                    //var key =int.Parse(values[0]);
                    //var name = values[1];
                    //var stream = File.ReadAllBytes(temp);
                    //result.Add(new KeyValuePair<int, string>(key,name),stream);
                    try
                    {
                        var key = int.Parse(values[0]);
                        var name = values[1];
                        var stream = File.ReadAllBytes(temp);
                        result.Add(new KeyValuePair<int, string>(key, name), stream);
                    }
                    catch { }
                }
                return result;
            }
        }

        public static byte[] GetRegularTemp(int[] cards_id, int MUS_id)
        {
            var cards = (new CardsRegister()).GetCards()
                .Where(i => cards_id.Contains(i.ID)).ToArray();
            var mus = (new CardsRegister()).MUS.FirstOrDefault(i => i.ID == MUS_id).Name;

            return GetDocByID(13, new Dictionary<string, object>()
            {
                {"Table",cards },
                {"Pri", mus}
            });
        }
        public static void UploadNewTemp(byte[] file, int num, string name = "")
        {
            var exist = _templates.Keys.ToList().FirstOrDefault(i => i.Key == num).Key == num;
            var path = "";
            if (exist)
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    $@"Resourses\{num}#{_templates.Keys.ToList().FirstOrDefault(i => i.Key == num).Value}.docx");

            }
            else
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    $@"Resourses\{num}#{name}.docx");
            }
            File.WriteAllBytes(path, file);
        }

        public static byte[] GetDocByID(int key, Dictionary<string, object> values)
        {
            var fileGroup = _templates.FirstOrDefault(i=>i.Key.Key==key);
            if(fileGroup.Value == null)
            {
                return new byte[0];
            }
            var temp_name = AppDomain.CurrentDomain.BaseDirectory +"\\"+ Enumerable.Range(1, 5000).OrderBy(g => Guid.NewGuid()).First() + ".docx";
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
                    else
                    {
                        if ((field.Value.GetType().IsArray) && ((Card[])field.Value) != null)
                        {
                            var table = new TableContent("Table");
                            foreach (var c in ((Card[])field.Value))
                            {
                                table = table.AddRow(
                                    new FieldContent("id_v", c.id_mark.ToString()),
                                    new FieldContent("Name", c.name),
                                    new FieldContent("Card_id", c.ID.ToString()),
                                    new FieldContent("Chip_id", c.id_chip.ToString()));
                                //table= table.AddRow(new FieldContent("Name",c.name));
                                //table= table.AddRow(new FieldContent("Card_id",c.ID.ToString()));
                                // table= table.AddRow(new FieldContent("Chip_id",c.id_chip.ToString()));
                            }
                            valuesToFill.Tables.Add(table);
                        }
                    }
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