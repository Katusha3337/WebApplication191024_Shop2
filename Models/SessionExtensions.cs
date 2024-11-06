using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebApplication191024_Shop.Models
{
    public static class SessionExtensions
    {
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore 
        };

        public static void Set<T>(this ISession session, string key, T value)
        {
            var jsonData = JsonConvert.SerializeObject(value, _jsonSettings);
            session.SetString(key, jsonData);
        }

        public static T Get<T>(this ISession session, string key)
        {
            var jsonData = session.GetString(key);
            return jsonData == null ? default(T) : JsonConvert.DeserializeObject<T>(jsonData, _jsonSettings);
        }
    }
}
