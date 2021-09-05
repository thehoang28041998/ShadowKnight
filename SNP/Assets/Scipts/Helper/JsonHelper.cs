using System;
using Newtonsoft.Json;

namespace Scipts.Helper {
    public class JsonHelper {
        public string SerializeObjectToString(object o) {
            string json = JsonConvert.SerializeObject(o, Formatting.Indented, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return json;
        }

        public string[] SerializeObjectToStringArray(object o) {
            string json = SerializeObjectToString(o);
            string[] split = json.Split(new string[] {"\r\n"}, StringSplitOptions.None);
            return split;
        }

        public T Deserialization<T>(string json) {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.Auto,  
            });
        }


        public T Deserialization<T>(string[] array) {
            string json = string.Join("", array);
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
    }
}