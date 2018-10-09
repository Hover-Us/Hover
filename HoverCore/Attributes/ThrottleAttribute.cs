//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.Extensions.Caching.Memory;
//using System;
//using System.Net;

//namespace Hover.Attributes
//{
//    [AttributeUsage(AttributeTargets.Method)]
//    public class ThrottleAttribute : ActionFilterAttribute
//    {   // gist.github.com/zarxor/
//        public string Name { get; set; }
//        public int Seconds { get; set; }

//        private static MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions());

//        public override void OnActionExecuting(ActionExecutingContext c)
//        {
//            var key = string.Concat(Name, GetIP(c));

//            if (!Cache.TryGetValue(key, out bool entry))
//            {
//                var cacheEntryOptions = new MemoryCacheEntryOptions()
//                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(Seconds));

//                Cache.Set(key, true, cacheEntryOptions);
//            }
//            else
//            {
//                c.Result = new ContentResult { Content = $"You may only perform this action every { Seconds.ToString() } seconds." };
//                c.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
//            }
//        }

//        private string GetIP(ActionExecutingContext c)
//        {   // Putting ThrottleAttribute on HOLD and letting Cloudflare do its thing
//            Microsoft.Extensions.Primitives.StringValues gotIt;
//            c.HttpContext.Request.Headers.TryGetValue("HTTP_CF_CONNECTING_IP", out gotIt);
//            if (gotIt.Count > 0) return gotIt.ToString();
//            else return c.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
//        }
//    }
//}
