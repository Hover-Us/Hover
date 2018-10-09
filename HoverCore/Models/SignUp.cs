using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hover.Controllers;
using Microsoft.AspNetCore.DataProtection;

namespace Hover.Models
{
    public class SignUp
    {

        public string name { get; set; }
        public string email { get; set; }
        public string captcha { get; set; }

        public Settings Settings { get; set; }
        private ILogger Logger;
        public HoverDb Db { get; set; }
        private IDataProtector _protector { get; set; }

        //        [Newtonsoft.Json.JsonConstructor]

        public async Task<string> UserSignup(Settings settings, ILogger<UserController> logger, HoverDb db, IDataProtectionProvider provider)
        {
            Settings = settings;
            Logger = logger;
            Db = db;
            _protector = provider.CreateProtector("Hover.Models.UserSignup");


            Task<string> callingCaptcha = ReCaptchErrorAsync(settings.CaptchaSecret);

            // Check input html page has pattern="[A-Za-z]+[A-Za-z -]+"
            name = System.Text.RegularExpressions.Regex.Matches(name, @"[A-Za-z]+[A-Za-z -]+").Cast<System.Text.RegularExpressions.Match>()
                  .Aggregate("", (s, e) => s + e.Value, s => s);

            email = email.ToLower();
            // userHash is a unique Key for a user (email)
            string userHash = Hash.GetHash(email, Settings.HashSalt, Settings.IterationCount);
            string emailEncrypted = _protector.Protect(email);

            Logger.LogInformation($"UserSignup userHash= {userHash}, emailEncrypted= {emailEncrypted}");

            string captchaError = await callingCaptcha;
            if (captchaError != null)
                return captchaError;


            // Call DataStore
            string oldEmailEncrypted = null, message = null;

            using (var cmd = Db.Hconn.CreateCommand())
            {
                await Db.ReadyConnection();
                cmd.CommandText = "UserNew";

                cmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter
                {
                    ParameterName = "@userHashIn",
                    DbType = DbType.String,
                    Value = userHash,
                });                    
                cmd.Parameters.AddWithValue("@emailEncrypted", emailEncrypted);
                cmd.Parameters.AddWithValue("@usernameIn", name);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = await cmd.ExecuteReaderAsync();
                if (reader.Read())
                {
                    if (reader["oldEmailEncrypted"] != DBNull.Value) oldEmailEncrypted = reader["oldEmailEncrypted"].ToString();
                    if (reader["message"] != DBNull.Value) message = reader["message"].ToString();
                }
            }

            // if there was something to report, no email unless the extremely rare hash collision -> different email addresses with same hash
            if (oldEmailEncrypted != null && email != _protector.Unprotect(oldEmailEncrypted))
            {
                // It is almost impossible for a hash collision on email addresses, likely something else, needs attention
                Logger.LogCritical($"UserSignup {email} Not Saved, hash collision {userHash} !? {message}");
            }
            else if (message != null)
                return message;
            

            // Send email
            var mineMessage = new MimeMessage();
            mineMessage.From.Add(new MailboxAddress(settings.MailboxAddressUpdateName, settings.MailboxAddressUpdateAddress));

            //   10/10 on mail-tester.com
            mineMessage.To.Add(new MailboxAddress(name, email));

            if (oldEmailEncrypted != null)
            {
                mineMessage.Bcc.Add(new MailboxAddress("hash collision!", settings.MailboxAddressPrivacyAddress));
            }

            // Can help lower spam rating
            mineMessage.Headers.Add("List-Unsubscribe", $"<mailto: {settings.MailboxAddressPrivacyAddress}?subject=unsubscribe{userHash}>");


            mineMessage.Subject = "Thank you for signing up " + name + ".";


            //     message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format(Mail.Templates.General.markup, name, userHash) }; // with only HTML Mime type

            var builder = new BodyBuilder
            {
                // Send both text and HTML
                TextBody = string.Format(Mail.Templates.Welcome.text, name, userHash, System.Net.WebUtility.UrlEncode(name)),
                HtmlBody = string.Format(Mail.Templates.Welcome.html, name, userHash, System.Net.WebUtility.UrlEncode(name))
            };
            mineMessage.Body = builder.ToMessageBody();

            using (var emailClient = new SmtpClient())
            {
                try
                {
                    emailClient.Connect(settings.EmailClientHost, settings.EmailClientPort, settings.EmailClientUseSsl);

                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate(settings.EmailClientUserName, settings.EmailClientPassword);

                    await emailClient.SendAsync(mineMessage);
                }
                catch (Exception ex)
                {
                    // Catch email error, log it and return something user friendly
                    Logger.LogCritical($"email to { userHash } error : { ex.ToString() }");
                    return ("We had a problem sending you an email. We will try again later.");
                }
                finally
                {
                    emailClient.Disconnect(true);
                }
            }

            return "Thank you for signing up " + name + ", an email has been sent to " + email;
        }

        public async Task<string> ReCaptchErrorAsync(string CaptchaSecret)
        {
            // Checks the captcha token with Google
            using (var webClient = new HttpClient())
            {
                HttpResponseMessage response = await webClient.GetAsync("https://www.google.com/recaptcha/api/siteverify?secret=" + CaptchaSecret + "&response=" + captcha);
                string json = await response.Content.ReadAsStringAsync();
                dynamic jsonDoc = JObject.Parse(json);
                if (jsonDoc.success == "true")
                    return null;

                if (jsonDoc.ContainsKey("error-codes"))
                    return "Error: " + jsonDoc["error-codes"]; // only returns the first error
                else
                    return "Error: Please try the Captcha again.";
            }
        }
     
    }
}
