using Newtonsoft.Json;

namespace API.Test
{
    public static class HttpClientHelper
    {
        private static readonly JsonSerializer serializer = JsonSerializer.Create();

        public static async ValueTask<T> ReadJsonResponser<T>(HttpResponseMessage response)
        {
            using (Stream s = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            using (StreamReader sr = new StreamReader(s))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<T>(reader);
            }
        }
    }
}
