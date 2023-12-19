using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace ProcessCost.IntegrationTests;

public static class HttpClientExtensions
{
    public static Task<HttpResponseMessage> SendPost<T>(this HttpClient client, string url, T body)
    {
        var json = JsonConvert.SerializeObject(body);
        var content = new StringContent(
            json,
            Encoding.UTF8,
            MediaTypeHeaderValue.Parse("application/json"));

        return client.PostAsync(url, content);
    }

    public static async Task<T> ParseTo<T>(this HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json)!;
    }
}