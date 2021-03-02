using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMSWeb.Models
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
    public class LoggedInUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string OrganisationName { get; set; }
        public bool TextTutor { get; set; }
        public bool TalkingTutor { get; set; }
        public bool TwoCanTalk { get; set; }
        public bool PhraseBook { get; set; }
        public string UserType { get; set; }
        public bool PermAdmin
        { 
            get 
            {
                if (UserType == null) 
                {
                    return false;
                }
                return UserType.Equals("admin", StringComparison.OrdinalIgnoreCase);  
            }
        }
    }
}
