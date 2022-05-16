using Oanda.RestApi.Models;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Oanda.RestApi.Services;

internal partial class OandaRestApi
{
    private readonly HttpClient httpClient = new();

    private readonly HttpClient streamingHttpClient = new();

    private readonly JsonSerializerOptions jsonSerializerOptions;

    private readonly JsonSerializerOptions orderSerializerOptions;

    public OandaRestApi(string oandaRestApiUrl, string oandaStreamingApiUrl,  string oandaApiKey)
    {
        httpClient.BaseAddress = new Uri(oandaRestApiUrl);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oandaApiKey);
        httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(MediaTypeNames.Application.Json));

        streamingHttpClient.BaseAddress = new Uri(oandaStreamingApiUrl);
        streamingHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oandaApiKey);
        //streamingHttpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(MediaTypeNames.Application.Octet));

        jsonSerializerOptions = new(JsonSerializerDefaults.Web)
        {
            WriteIndented = false,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        orderSerializerOptions = new(JsonSerializerDefaults.Web)
        {
            NumberHandling = JsonNumberHandling.WriteAsString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }
    private static async Task DumpAsync(Stream? strm)
    {
        if (strm == null)
        {
            return;
        }

        using StreamReader streamReader = new StreamReader(strm);
        string streamContent = await streamReader.ReadToEndAsync();
        Console.WriteLine(streamContent);
    }

    private static async Task SaveToFileAsync(Stream? strm, string filePath)
    {
        if (strm == null)
        {
            return;
        }

        using StreamReader streamReader = new StreamReader(strm);
        
        string streamContent = await streamReader.ReadToEndAsync();

        using StreamWriter sw = new(filePath);

        sw.AutoFlush = true;

        sw.Write(streamContent);
    }
}
