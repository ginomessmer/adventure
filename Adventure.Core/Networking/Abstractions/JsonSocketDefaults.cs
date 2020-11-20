using Newtonsoft.Json;

namespace Adventure.Core.Networking.Abstractions
{
    public static class JsonSocketDefaults
    {

        public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented
        };
    }
}