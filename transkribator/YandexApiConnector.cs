using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using transkribator.Dtos;

namespace transkribator;

public class YandexApiConnector
{
    string? privateKey;

    public YandexApiConnector(string privateKeyFile)
    {
        using var streamReader = File.OpenText(privateKeyFile);
        privateKey = streamReader.ReadLine();
    }

    public async Task<string> ConvertStartAsync(string fileUri)
    {
        using var httpclient = new HttpClient();

        // адрес сервера
        var url = "https://transcribe.api.cloud.yandex.net/speech/stt/v2/longRunningRecognize";

        var data = new YandexTranscribePOSTAggregator(fileUri);

        var myContent = JsonConvert.SerializeObject(data);
        
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Api-Key", privateKey);
        request.Content = new StringContent(myContent, Encoding.UTF8, "application/json");
       
        using var response = await httpclient.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();
        var substring = responseString.Substring(responseString.IndexOf("id", StringComparison.Ordinal) + 6);
        return substring.Remove(substring.IndexOf("\"", StringComparison.Ordinal));
    }

    public async Task<YandexTranscribeGETAggregator?> TryGetResultAsync(string operationId)
    {
        using var httpclient = new HttpClient();
        var url = $"https://operation.api.cloud.yandex.net/operations/{operationId}";
        
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Api-Key", privateKey);
        using var response = await httpclient.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();
        
        return JsonConvert.DeserializeObject(responseString, typeof(YandexTranscribeGETAggregator)) as YandexTranscribeGETAggregator;
    }
}