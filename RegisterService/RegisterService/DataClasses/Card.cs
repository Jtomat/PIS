using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using Microsoft.Office.Interop.Word;

namespace RegisterService.DataClasses
{
    public class Card
    {
        protected static SqlConnection connectToDB = new SqlConnection()
        {
            ConnectionString = "workstation id=PISProject.mssql.somee.com;packet size=4096;user id=jtomatos_SQLLogin_1;pwd=bvl14xaa5f;data source=PISProject.mssql.somee.com;persist security info=False;initial catalog=PISProject"
        };
        public Card()
        {
            if (connectToDB.State != System.Data.ConnectionState.Open)
                connectToDB.Open();
        }
        public enum SexAnimal { Female, Male, Undefined }
        public int ID;
        public SexAnimal Sex;
        public string Type;
        public DateTime BirthDay;
        public int ID_Mark;
        public int ID_Chip;
        public string Name;
        public Stream Photo;
        public DateTime DateSterelisation;
        public string SpecMarks;
        public string OwnerTraits;
        protected internal KeyValuePair<int, string> _status;
        public string Status { get { return _status.Value; } }
        public DateTime DateStatusChange;
        public string MU;
        public string LocalPlace;
        public Stream File;
        public Stream ScanFrame;


        public int SaveChanges()
        {
            try
            {
                var sex = Sex == SexAnimal.Undefined ? "NULL" : $"'{(int)Sex}'";
                (new SqlCommand("update Card set" +
                    $"sex='{sex}',type='{Type}',birthday='{BirthDay.ToShortDateString()}'" +
                    $",id_mark='{ID_Mark}',id_chip='{ID_Chip}'," +
                    $"name='{Name}',sterelisation_date='{DateSterelisation.ToShortDateString()}'," +
                    $"spec_mark='{SpecMarks}',owner_traits='{OwnerTraits}'," +
                    $"id_status='{_status.Key}',date_status_change ='{DateStatusChange.ToShortDateString()}'," +
                    $"MU='{MU}',local_place='{LocalPlace}' where id='{ID}'", connectToDB)).ExecuteReader();
                return 0;
            }
            catch { return -1; }
        }
        public void UploadFile(string fileName)
        {
            var stream = new BinaryReader((new StreamReader(path: fileName)).BaseStream);
            File = stream.BaseStream;
            var query = new SqlCommand($"update Card set File='@DOC' where id='{ID}'", connectToDB);
            query.Parameters.AddWithValue("@DOC", (stream).ReadBytes((int)File.Length));
            query.ExecuteReader();
        }
        public void DeleteFile()
        {
            File = null;
            (new SqlCommand($"update Card set File=NULL where id='{ID}'", connectToDB)).ExecuteReader();
        }
        public void ExportDoc()
        {
            //options
            object matchCase = false;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;


            var town = (object)(new StringBuilder(LocalPlace.Split(new[] { ' ' })[0])).ToString();
            var tempF = Path.GetTempFileName();
            System.IO.File.WriteAllBytes(tempF, ResourceSys.TempExport);
            var app = new Application();
            var doc = app.Documents.Open(tempF);
            doc.Activate();
            app.Selection.Find.Execute("@TOWN", ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward,
                ref wrap, ref format, ref town, ref replace,
                ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
        }
    }
}