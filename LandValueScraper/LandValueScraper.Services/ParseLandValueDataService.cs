using LandValueScraper.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LandValueScraper.Services
{
    public static class ParseLandValueDataService
    {
        public static LandValueDataDTO? ParseLandValueData(string landValueJson, string buildingFootprintJson, string address, double[] coordPair)
        {
            JObject parsedLandValueJson = JObject.Parse(landValueJson);
            string? zipCode = IsWithinBounds(parsedLandValueJson);
            if (zipCode == null) return null;

            JObject parsedBuildingFootprintJson = JObject.Parse(buildingFootprintJson);

            //parse out required parameters. Some are nullable due to empty no situs sites
            double? marketTotalValue = ParseOutTotalValue(parsedLandValueJson);
            double? lotSize = ParseOutLotSize(parsedLandValueJson);
            double? buildingFootprint = ParseOutBuildingFootprint(parsedBuildingFootprintJson);
            string? landUseDescription = ParseOutPropertyDescription(parsedLandValueJson);
            string? parcelWKTString = ParseOutParcelWKTPolygonString(parsedLandValueJson);

            ParcelGeometry geometry = new ParcelGeometry(
                FormattingService.ParcelWKTToObject(parcelWKTString),
                FormattingService.CoordPairToObject(coordPair)
                );

            return new LandValueDataDTO(
                address,
                marketTotalValue,
                LandValueQuickMathsService.CalculateValuePerAcre(marketTotalValue, lotSize),
                LandValueQuickMathsService.CalculateTaxableValue(marketTotalValue, zipCode),
                LandValueQuickMathsService.CalculateTaxableValuePerAcre(marketTotalValue, zipCode, lotSize),
                lotSize,
                LandValueQuickMathsService.CalculateLotCoverage(buildingFootprint, lotSize),
                landUseDescription,
                geometry
                );
        }
        //checks whether the property is in LO or WL
        private static string? IsWithinBounds(JObject parsedLandData)
        {
            string? postalCode = (string?)parsedLandData.SelectToken("parcels[0].location.postalCode");
            if (postalCode == "97034" || postalCode == "97068") return postalCode;
            return null;
        }

        private static double? ParseOutTotalValue(JObject parsedLandData)
        {
            string? parsedLandDataString = (string?)parsedLandData.SelectToken("parcels[0].assessment.marketValue.total");
            if (parsedLandDataString == null) return null;
            return double.Parse(parsedLandDataString);
        }

        private static double? ParseOutLotSize(JObject parsedLandData)
        {
            string? lotSizeInM = (string?)parsedLandData.SelectToken("parcels[0].derived.calculatedLotArea");
            if (lotSizeInM == null) return null;
            return LandValueQuickMathsService.ConvertMetersToAcres(double.Parse(lotSizeInM));
        }

        private static double? ParseOutBuildingFootprint(JObject parsedBuildingData)
        {
            string? buildingFootprintStr = (string?)parsedBuildingData?.SelectToken("structures[0].physicalFeatures.area.footprintArea");
            if (buildingFootprintStr == null) return null;
            return LandValueQuickMathsService.ConvertMetersToAcres(double.Parse(buildingFootprintStr));
        }

        private static string? ParseOutPropertyDescription(JObject parsedLandData) =>
            (string?)parsedLandData.SelectToken("parcels[0].landUse.normalized.description");

        private static string? ParseOutParcelWKTPolygonString(JObject parsedLandData) =>
            (string?)parsedLandData.SelectToken("parcels[0].location.geometry.wkt");
    }
}
