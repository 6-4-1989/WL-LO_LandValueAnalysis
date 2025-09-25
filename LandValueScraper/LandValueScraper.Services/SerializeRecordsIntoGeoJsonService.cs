using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LandValueScraper.Models;

namespace LandValueScraper.Services;

public static class SerializeRecordsIntoGeoJsonService
{
    public static string Serialize(List<LandValueDataDTO> landValueDTOs) =>
        JsonConvert.SerializeObject(landValueDTOs);

    public static async Task WriteAndDownloadGeoJson(string geoJsonString, string filePath)
    {
        await File.WriteAllTextAsync(filePath, geoJsonString, Encoding.UTF8);
    }
}
