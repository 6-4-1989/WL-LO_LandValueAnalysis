using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LandValueScraper.Services;

//made this because my ocd was on fire
public static class LandValueQuickMathsService
{
    public static double? CalculateValuePerAcre(double? marketTotalValue, double? lotSize)
    {
        if (lotSize == null || lotSize == null) return null;
        return marketTotalValue / lotSize;
    }

    public static double? CalculateLotCoverage(double? buildingFootprint, double? lotSize)
    {
        if (buildingFootprint == null || lotSize == null) return null;
        return buildingFootprint / lotSize;
    }

    public static double? CalculateTaxableValue(double? marketTotalValue, string zipCode)
    {
        if (marketTotalValue == null) return null;
        if (zipCode == "97068") return marketTotalValue * 0.0106;
        return marketTotalValue * 0.0096;
    }

    public static double? CalculateTaxableValuePerAcre(double? marketTotalValue, string zipCode, double? lotSize)
    {
        if (marketTotalValue == null || lotSize == null) return null;
        return CalculateTaxableValue(marketTotalValue, zipCode) / lotSize;
    }

    //freedom units conversion for better understanding (bc American audience)
    public static double? ConvertMetersToAcres(double? meterValueIn)
    {
        if (meterValueIn == null) return null;
        return meterValueIn / 4047;
    }

}
