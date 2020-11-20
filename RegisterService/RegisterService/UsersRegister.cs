using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using RegisterService.DataClasses;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;

namespace RegisterService
{
    public class UsersRegister 
    {
        private HttpContext _context;
        private IPrincipal _principalUser = new GenericPrincipal(new GenericIdentity("null"), new string[] { });
        public IPrincipal Identity { get{ return _principalUser; } }
        private User _currentUser;
        public User CurrentUser { 
            get { return _currentUser; } 
            set {
                if (_currentUser != value&& _currentUser!=null)
                {
                    _currentUser = value;
                    UpdateUser(_currentUser);
                }
            } }
        protected SqlConnection connectToDB = new SqlConnection()
        {
            ConnectionString = "workstation id=PISProject.mssql.somee.com;packet size=4096;user id=jtomatos_SQLLogin_1;pwd=bvl14xaa5f;data source=PISProject.mssql.somee.com;persist security info=False;initial catalog=PISProject"
        };
        public UsersRegister(HttpContext httpContext)
        {
            _context = httpContext;
        }
        public List<User> Users 
        {
            get {
                return GetUsers().ToList().FindAll(i=>i.Confirmed);
            }
        }
        public HttpContext Context { get { return _context; } }
        public List<User> Requsts
        {
            get
            {
                return GetUsers().ToList().FindAll(i => !i.Confirmed);
            }
        }
        public UsersRegister(HttpContext httpContext, Guid SIN, string password)
        {
            _context = httpContext;
            if (string.IsNullOrEmpty(SIN.ToString()) && string.IsNullOrEmpty(password))
                throw new ArgumentNullException("username, password");
            if (string.IsNullOrEmpty(SIN.ToString()))
                throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            var u = GetUsers().ToList();
            var user_c = Users.FirstOrDefault(i => i.SIN == SIN && i.Password == password);
            if(user_c==null)
                throw new Exception("Unknown Username or Password");
            CurrentUser = user_c;
            _principalUser = new GenericPrincipal(
               identity: new GenericIdentity(user_c.FIO), 
               roles: user_c.Roles.Split(new char[] {'|'},StringSplitOptions.RemoveEmptyEntries));
        }
        public void UpdateUser(User changed)
        {
            (new SqlCommand($"update Users set FIO='{changed.FIO}', organization='{changed.ORG}'," +
                $" Role='{changed.Roles}', email='{changed.Email}'," +
                $"phone='{changed.Phone}' where id='{changed.ID}'",connectToDB)).ExecuteReader();
        }
        public bool SendRegInfo(Guid SIN)
        {
            if (connectToDB.State != System.Data.ConnectionState.Open)
            {
                connectToDB.Open();
            }
            var query = new SqlCommand(
                $"select (case when((select Count(*) from(select * from Users where SIN = '{SIN}') as CC) = 1) then '1'	else '0' END) from Users", connectToDB).ExecuteReader();
            query.Read();
            return query[1].ToString() != "1";
        }
        public void SendRegReq(Guid SIN, string[] data, Object doc)
        {
            if (connectToDB.State != System.Data.ConnectionState.Open)
            {
                connectToDB.Open();
            }
            var query = new SqlCommand(
                $"insert into Users (FIO,email,phone,organistion,SIN,password,Doc) values('{data[0]}','{data[1]}','{data[2]}','{data[3]}','{SIN}','{data[4]}',@DOC)", connectToDB);
            query.Parameters.AddWithValue("@DOC", (new BinaryReader((Stream)doc)).ReadBytes((int)((Stream)doc).Length));
        }
        private IEnumerable<User> GetUsers()
        {
            var result = new List<User>();
            if (connectToDB.State != System.Data.ConnectionState.Open)
            {
                connectToDB.Open();
            }
            var command = new SqlCommand(
                $"select * from Users"
                , connectToDB);
            var reader = command.ExecuteReader();
            while (reader == null)
                Thread.Sleep(10);
            while (reader.Read())
            {
                var support_memory = new User()
                {
                    ID = int.Parse(reader[0].ToString()),
                    FIO = reader[1].ToString(),
                    Roles = reader[2].ToString(),
                    Email = reader[3].ToString(),
                    Phone = reader[4].ToString(),
                    ORG = reader[5].ToString(),
                    SIN = Guid.Parse(reader[6].ToString()),
                    Password = reader[7].ToString(),
                    Confirmed = ((Boolean)reader[8])
                };
                var k = reader[8].ToString();
                result.Add(support_memory);
            }
            reader.Close();
            var getDoc = new SqlCommand($"select cast(convert(varchar(max), Doc, 1) as xml) from Users", connectToDB);
            var xmlRes = getDoc.ExecuteReader();
            int index = 0;
            while (xmlRes.Read())
            {
                result[index].Doc = new MemoryStream(Encoding.UTF8.GetBytes(xmlRes[0].ToString()));
                yield return result[index];
                index++;
            }
            xmlRes.Close();
            // вернуть значение надо
           // OperationContext.Current.GetCallbackChannel<IServiceCallBack>().ReciveUsers(result.ToArray());
        }
    }
    
}