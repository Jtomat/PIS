using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using RegisterService.DataClasses;

namespace RegisterService
{
    public class CardsRegister
    {
        public Card GetCardByID(int id)
        {
            return GetCards().FirstOrDefault(i=>i.ID==id);
        }
        public Card[] GetFilteredBy(string[] selectedAttr, string[] value)
        {
            if (selectedAttr.Length != value.Length)
                throw new ArgumentOutOfRangeException("Count of filters and values must be equals.");
            var data = GetCards();
            for (int i = 0; i < selectedAttr.Length; i++)
                switch (selectedAttr[i])
                {
                    case "Photo":
                        if (value[i] == "True")
                            data = data.OrderBy(j => j.Photo != null);
                        else
                            data = data.OrderBy(j => j.Photo == null);
                        break;
                    case "Type":
                        data = data.OrderBy(j => j.Type == value[i]);
                        break;
                    case "OwnerTraits":
                        if(value[i]=="True")
                            data = data.OrderBy(j =>!string.IsNullOrEmpty(j.OwnerTraits));
                        else
                            data = data.OrderBy(j =>string.IsNullOrEmpty(j.OwnerTraits));
                        break;
                    case "Status":
                            data = data.OrderBy(j => j.Status==value[i]);
                        break;
                    case "SpecMarks":
                        foreach(var mark in value[i].Split(new[] {',',' '},StringSplitOptions.RemoveEmptyEntries))
                           data = data.OrderBy(j => j.SpecMarks.Contains(mark));
                        break;
                    case "Sterelisation":
                        if (value[i] == "True")
                            data = data.OrderBy(j => j.DateSterelisation!=DateTime.MinValue);
                        else
                            data = data.OrderBy(j => j.DateSterelisation == DateTime.MinValue);
                        break;
                    case "MU":
                        data = data.OrderBy(j=>j.MU==value[i]);
                        break;
                }
            return data.ToArray();
        }
        public Card[] GetSortedBy(string selectedAttr, bool upper)
        {
            switch (selectedAttr)
            {
                case "ID":
                    if (upper)
                        return GetCards().OrderBy(i => i.ID).ToArray();
                    return GetCards().OrderBy(i => i.ID).Reverse().ToArray();
                case "SEX":
                    if (upper)
                        return GetCards().OrderBy(i => i.Sex).ToArray();
                    return GetCards().OrderBy(i => i.Sex).Reverse().ToArray();
                case "Type":
                    if (upper)
                        return GetCards().OrderBy(i => i.Type).ToArray();
                    return GetCards().OrderBy(i => i.Type).Reverse().ToArray();
                case "BirthDay":
                    if (upper)
                        return GetCards().OrderBy(i => i.BirthDay).ToArray();
                    return GetCards().OrderBy(i => i.BirthDay).Reverse().ToArray();
                case "ID-Mark":
                    if (upper)
                        return GetCards().OrderBy(i => i.ID_Mark).ToArray();
                    return GetCards().OrderBy(i => i.ID_Mark).Reverse().ToArray();
                case "ID-Chip":
                    if (upper)
                        return GetCards().OrderBy(i => i.ID_Chip).ToArray();
                    return GetCards().OrderBy(i => i.ID_Chip).Reverse().ToArray();
                case "Name":
                    if (upper)
                        return GetCards().OrderBy(i => i.Name).ToArray();
                    return GetCards().OrderBy(i => i.Name).Reverse().ToArray();
                case "Photo":
                    if (upper)
                        return GetCards().OrderBy(i => i.Photo).ToArray();
                    return GetCards().OrderBy(i => i.Photo).Reverse().ToArray();
                case "DateSterelisation":
                    if (upper)
                        return GetCards().OrderBy(i => i.DateSterelisation).ToArray();
                    return GetCards().OrderBy(i => i.DateSterelisation).Reverse().ToArray();
                case "Status":
                    if (upper)
                        return GetCards().OrderBy(i => i.Status).ToArray();
                    return GetCards().OrderBy(i => i.Status).Reverse().ToArray();
                case "DateStatusChange":
                    if (upper)
                        return GetCards().OrderBy(i => i.DateStatusChange).ToArray();
                    return GetCards().OrderBy(i => i.DateStatusChange).Reverse().ToArray();
            }
            return GetCards().ToArray();
        }
        protected SqlConnection connectToDB = new SqlConnection()
        {
            ConnectionString = "workstation id=PISProject.mssql.somee.com;packet size=4096;user id=jtomatos_SQLLogin_1;pwd=bvl14xaa5f;data source=PISProject.mssql.somee.com;persist security info=False;initial catalog=PISProject"
        };
        public List<Card> GetList(string org)
        {
            return GetCards().ToList().FindAll(i=>(i.MU == org));
        }
        private IEnumerable<Card> GetCards()
        {
            var result = new List<Card>();
            if (connectToDB.State != System.Data.ConnectionState.Open)
            {
                connectToDB.Open();
            }
            var command = new SqlCommand(
                $"select Card.ID,sex,type,birthday,id_mark,id_chip,Card.name,sterelisation_date,spec_mark,owner_traits,id_status,Status.name,date_status_change,MU,local_place from Card join Status on Status.ID = id_status"
                , connectToDB);
            var reader = command.ExecuteReader();
            while (reader == null)
                Thread.Sleep(10);
            while (reader.Read())
            {
                var support_memory = new Card()
                {
                    ID = int.Parse(reader[0].ToString()),
                    Sex = ((Boolean)reader[0])?Card.SexAnimal.Male:
                        (!(Boolean)reader[0]) ? Card.SexAnimal.Female: Card.SexAnimal.Undefined,
                    Type = reader[2].ToString(),
                    BirthDay = DateTime.Parse(reader[3].ToString()),
                    ID_Mark =int.Parse(reader[4].ToString()),
                    ID_Chip =int.Parse(reader[5].ToString()),
                    Name = reader[6].ToString(),
                    DateSterelisation = string.IsNullOrEmpty(reader[7].ToString())?
                        DateTime.MinValue:DateTime.Parse(reader[7].ToString()),
                    SpecMarks = reader[8].ToString(),
                    OwnerTraits = reader[9].ToString(),
                    _status = new KeyValuePair<int, string>(int.Parse(reader[10].ToString()), (reader[11].ToString())),
                    DateStatusChange = DateTime.Parse(reader[12].ToString()),
                    MU = reader[13].ToString(),
                    LocalPlace = reader[14].ToString()
                };
                var k = reader[8].ToString();
                result.Add(support_memory);
            }
            reader.Close();
            var getDoc = new SqlCommand($"select cast(convert(varchar(max), photo, 1) as xml) as ph, "+
                "cast(convert(varchar(max), document, 1) as xml) as dc,"+
                " cast(convert(varchar(max), scan_frame, 1) as xml) as sc from Card", connectToDB);
            var xmlRes = getDoc.ExecuteReader();
            int index = 0;
            while (xmlRes.Read())
            {
                result[index].Photo =!string.IsNullOrEmpty(xmlRes[0].ToString())?new MemoryStream(Encoding.UTF8.GetBytes(xmlRes[0].ToString())):null;
                result[index].File = !string.IsNullOrEmpty(xmlRes[1].ToString()) ? new MemoryStream(Encoding.UTF8.GetBytes(xmlRes[1].ToString())):null;
                result[index].ScanFrame = !string.IsNullOrEmpty(xmlRes[2].ToString())?new MemoryStream(Encoding.UTF8.GetBytes(xmlRes[2].ToString())):null;
                yield return result[index];
                index++;
            }
            xmlRes.Close();
            // вернуть значение надо
            // OperationContext.Current.GetCallbackChannel<IServiceCallBack>().ReciveUsers(result.ToArray());
        }
    }
}