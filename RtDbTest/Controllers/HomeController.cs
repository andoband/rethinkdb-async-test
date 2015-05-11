using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RethinkDb;

namespace RtDbTest.Controllers
{
    public class HomeController : Controller
    {
        public static IConnectionFactory ConnectionFactory = RethinkDb.Configuration.ConfigurationAssembler.CreateConnectionFactory("some-cluster-name");
        public static IDatabaseQuery Db = Query.Db("some-db-name");
        public static ITableQuery<User> tUsers = Db.Table<User>("Users");

        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route(""), HttpPost]
        public ActionResult IndexPost(int num)
        {
            using (var conn = ConnectionFactory.Get())
                conn.Run(tUsers.Get("some-user-id"));
            return Json(new { success = true });
        }

        [Route("async"), HttpPost]
        public async Task<ActionResult> IndexPostAsync(int num)
        {
            using (var conn = await ConnectionFactory.GetAsync())
                await conn.RunAsync(tUsers.Get("some-user-id"));            
            return Json(new { success = true });
        }
    }

    [DataContract]
    public class User
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string email { get; set; }
        
        [DataMember]
        public string firstName { get; set; }
        
        [DataMember]
        public string lastName { get; set; }
        
        [DataMember]
        public string hash { get; set; }
    }
}