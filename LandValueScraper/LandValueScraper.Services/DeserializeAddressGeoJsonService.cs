using LandValueScraper.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace LandValueScraper.Services;

public static class DeserializeAddressGeoJsonService
{
    private static readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "testdata.txt");

    //deserializes the address data for Lake Oswego and West Linn
    public static List<DeserializedAddressGeoJsonDTO> Deserialize()
    {
        List<DeserializedAddressGeoJsonDTO> deserializedGeoJson = new List<DeserializedAddressGeoJsonDTO>();

        //parse each object individually because geojson schemas are like that :middlefinger:
        using (StreamReader streamReader = new StreamReader(_filePath))
        {
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                DeserializedAddressGeoJsonDTO deserializedGeoJsonDTO = JsonConvert.DeserializeObject<DeserializedAddressGeoJsonDTO>(line);

                if (deserializedGeoJsonDTO.properties.city == "WEST LINN"
                    || deserializedGeoJsonDTO.properties.city == "LAKE OSWEGO" 
                    || deserializedGeoJsonDTO.properties.city == "ADDRESS")
                {
                    deserializedGeoJson.Add(deserializedGeoJsonDTO);
                }
            }
        }
        Console.WriteLine(deserializedGeoJson.Count);
        return deserializedGeoJson;
    }
}

