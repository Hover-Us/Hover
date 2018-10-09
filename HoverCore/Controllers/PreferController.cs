using System.Threading.Tasks;
using Hover.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.DataProtection;

namespace Hover.Controllers
{

    [Route("[controller]")]
    public class PreferController : ControllerBase
    {
        public User HoverUser { get; set; }
        private Settings Settings { get; set; }
        public HoverDb Db { get; set; }
        private readonly ILogger<UserController> _logger;

        public PreferController(Microsoft.Extensions.Options.IOptions<Settings> settings, ILogger<UserController> logger, HoverDb db, IDataProtectionProvider provider)
        {
            Settings = settings.Value;
            _logger = logger;
            Db = db;
        }

        // static /prefer? will pass the querystring 
        [HttpGet]
       // [Hover.Attributes.Throttle(Name = "prefer", Seconds = 1)]
        public async Task<ActionResult<string>> GetAsync(string u)
        {
            string snippetWeKeep = "We keep a one way hash so if you sign up again on <a href=/>the homepage</a>, we will not send you another welcome email, but <b>you can</b> sign up again and <b>we hope you do</b>.";
            string Unsubscribed = "You are Unsubscribed and your encrypted email address has been deleted from our database. " + snippetWeKeep;
            string information;

            if (u == null)
            {
                information = "URL formatting error.";
            }
            else if (u.Length != 44)
            {
                information = $"Length { u.Length } of \"{ u }\"";
            }
            else if (HttpContext.Request.Query.ContainsKey("remove"))
            {
                string Result = await Models.User.UserOptOut(u);
                if (!string.IsNullOrEmpty(Result)) // Something didn't work right...
                    return Result;
                else
                    return Unsubscribed;
            }
            else
            {
                HoverUser = await Models.User.CreateAsync(u, Db);

                if (HoverUser != null && !string.IsNullOrEmpty(HoverUser.hash))
                {
                    if (HoverUser.optout)
                        return Unsubscribed;
                    else
                    {
                        string UnsubscribeLink = "?u=" + HoverUser.hash + "&remove";
                        return $"Hello { HoverUser.username },<br><br>If you wish to never hear from us, click <a class=button-a href={ UnsubscribeLink } style=\"background:#222;border:1px solid #000;font-family:sans-serif;font-size:15px;line-height:15px;text-decoration:none;padding:3px 6px;border-radius:4px\">Unsubscribe</a> and your encrypted email address will be deleted from our database. { snippetWeKeep } To change your name from <i>{ HoverUser.username }</i>, just sign up again, no need to unsubscribe.";
                    }
                }
                else information = "Not found.";
            }
            return $"Something didn't work right: { information }<br> Please retry or if you prefer, email { Settings.MailboxAddressUsAddress }";
        }
    }
}
