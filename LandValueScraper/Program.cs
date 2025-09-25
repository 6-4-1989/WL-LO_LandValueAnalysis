using LandValueScraper.Models;
using LandValueScraper.Services;
using System.Text.Json;

/*
Short Description:
    -Gets lot coverage, land value per acre,
    taxable value per acre, and polygon dimentions of Lake
    Oswego and West Linn via reverse geocoding and
    LightBox Vision API
    -Packages data into geojson
 */
namespace LandValueScraper;

public class Program
{
    public static async Task Main(string[] args)
    {
        Initialize.Init();

        try
        {
            await InSession();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public static async Task InSession()
    {
        List<LandValueDataDTO> landValueDataDTOs = new List<LandValueDataDTO>();
        List<DeserializedAddressGeoJsonDTO> addressDTOs = DeserializeAddressGeoJsonService.Deserialize();

        foreach (var addressDTO in addressDTOs)
        {
            string[] landValueData = await ScrapeLandValues.ScrapeObjects(addressDTO.geometry.coordinates);
            if (landValueData[0] == "{\"error\":{\"code\":\"404\", \"message\":\"Not Found\"}}") continue;
            LandValueDataDTO? landValueDTO =
                ParseLandValueDataService.ParseLandValueData(landValueData[0], landValueData[1], FormattingService.FormatAddress(addressDTO), addressDTO.geometry.coordinates);
            if (landValueDTO != null) landValueDataDTOs.Add(landValueDTO);
        }

        //turn the data into geojson
        string json = SerializeRecordsIntoGeoJsonService.Serialize(landValueDataDTOs);
        string filePath = FormattingService.FormatFilePathString("WL-LO_PropertyData.geojson");
        await SerializeRecordsIntoGeoJsonService.WriteAndDownloadGeoJson(json, filePath);
    }
}