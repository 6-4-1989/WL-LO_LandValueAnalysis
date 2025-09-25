using LandValueScraper.Models;
using LandValueScraper.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace LandValueScraper;

public class ScrapeLandValues
{
    public static HttpClient MyClient { get; private set; }
    public static IConfiguration Configuration { get; private set; }
    private const string _baseUri = "https://api.lightboxre.com/";

    public ScrapeLandValues(HttpClient httpClient, IConfiguration configuration)
    {
        MyClient = httpClient;
        Configuration = configuration;
    }

    //gets land values of properties as records
    public static async Task<string[]> ScrapeObjects(double[] coordPair)
    {
        //get land value data
        string landValueJsonResponse = await GetAddressDataAsync(
            $"v1/parcels/us/geometry?wkt=POINT({coordPair[0]} {coordPair[1]})&limit=1");
        //get lot coverage data
        string buildingFootprintJsonResponse = await GetAddressDataAsync(
            $"v1/structures/us/geometry?wkt=POINT({coordPair[0]} {coordPair[1]})");

        string[] landValueJson = { landValueJsonResponse, buildingFootprintJsonResponse };
        return landValueJson;
    }

    private static async Task<string> GetAddressDataAsync(string query)
    {
        using HttpRequestMessage requestMessage = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_baseUri + query),
        };
        requestMessage.Headers.Add("x-api-key", Configuration.GetConnectionString("LightBoxAPIKey"));

        using HttpResponseMessage httpResponseMessage = await MyClient.SendAsync(requestMessage);
        string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
        Console.WriteLine(jsonResponse);
        return jsonResponse;
    }
}