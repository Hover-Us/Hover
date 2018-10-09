using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Hover.Models; 
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.DataProtection;

namespace Hover.Controllers
{

    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private Settings Settings { get; set; }
        private readonly ILogger<UserController> _logger;
        public HoverDb Db { get; set; }
        readonly IDataProtector _protector;

        public UserController(Microsoft.Extensions.Options.IOptions<Settings> settings, ILogger<UserController> logger, HoverDb db, IDataProtectionProvider provider)
        {
            Settings = settings.Value;
            _logger = logger;
            Db = db;
            _protector = provider.CreateProtector("Hover.Models.UserSignup");
        }
        // GET /user
        //[HttpGet]
        //[Hover.Attributes.Throttle(Name = "signUp", Seconds = 1)]
        //public ActionResult<IEnumerable<string>> Get(string e, string u) 
        //{
        //    string encrypted = (e != null) ? _protector.Protect(e) : "e Not set";
        //    string U = (u != null && u.Length > 10) ? _protector.Unprotect(u) : "u Not set";
        //    return new string[] { e, encrypted, $"U= {U}" };
        //}


        [HttpPost]
      //  [Hover.Attributes.Throttle(Name = "signUp", Seconds = 2)]
        public async Task<string> PostAsync(string body)
        {
            if (string.IsNullOrEmpty(body))
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                body = await reader.ReadToEndAsync();
            }

            SignUp userSU = JsonConvert.DeserializeObject<SignUp>(body);

            return await userSU.UserSignup(Settings, _logger, Db, _protector);
        }

    }
   
}
