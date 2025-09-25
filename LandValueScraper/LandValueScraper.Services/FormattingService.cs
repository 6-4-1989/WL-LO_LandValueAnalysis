using LandValueScraper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using GeoJSON.Net;
using System.Reflection.Metadata.Ecma335;
using GeoJSON.Net.Feature;

namespace LandValueScraper.Services;

public static class FormattingService
{
    public static string FormatAddress(DeserializedAddressGeoJsonDTO addressDTO)
    {
        string partiallyFormattedAddress = $"{addressDTO.properties.number} " +
            $"{addressDTO.properties.street} " +
            $"{addressDTO.properties.city}, OR " +
            $"{addressDTO.properties.postcode}";
        return Regex.Replace(partiallyFormattedAddress, @"\s+", " ").Trim(' ');
    }

    public static FeatureCollection CoordPairToObject(double[] coordPair)
    {
        Point point = new Point(new Position(coordPair[1], coordPair[0]));
        return FormatIntoFeatureCollection(new Feature(point));
    }

    public static string FormatFilePathString(string fileName) =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);

    public static FeatureCollection? ParcelWKTToObject(string? parcelWKTString)
    {
        if (parcelWKTString == null) return null;

        if (parcelWKTString.Contains("MULTIPOLYGON"))
        {
            MultiPolygon multiPolygon = FormatMultiPolygonWKT(parcelWKTString);
            return FormatIntoFeatureCollection(new Feature(multiPolygon));
        }
        Polygon polygon = FormatPolygonWKT(parcelWKTString);
        return FormatIntoFeatureCollection(new Feature(polygon));
    }

    private static MultiPolygon FormatMultiPolygonWKT(string wktString)
    {
        MatchCollection matches = Regex.Matches(wktString, @"POLYGON\s*\(\([^)]+\)(?:\s*,\s*\([^)]+\))*\)|(?<=\()\([^)]+\)(?:\s*,\s*\([^)]+\))*(?=\))");
        List<string> polygonList = matches.Cast<Match>()
            .Select(m => m.Value)
            .ToList();
        List<Polygon> castedPolygons = new List<Polygon>();

        foreach (string polygonWKT in polygonList)
        {
            Polygon polygon = FormatPolygonWKT(polygonWKT);
            castedPolygons.Add(polygon);
        }
        return new MultiPolygon(castedPolygons);
    }

    private static Polygon FormatPolygonWKT(string wktString)
    {
        MatchCollection matches = Regex.Matches(wktString, @"-?\d+(?:\.\d+)?");
        List<double> coordList = matches.Cast<Match>()
            .Select(m => double.Parse(m.Value))
            .ToList();
        List<IPosition> polygonCoords = new List<IPosition>();

        for (int i = 0; i < coordList.Count - 1; i += 2)
        {
            polygonCoords.Add(new Position(coordList[i + 1], coordList[i]));
        }

        return new Polygon(new List<LineString>
        {
            new LineString(polygonCoords)
        });
    }

    private static FeatureCollection FormatIntoFeatureCollection(Feature myPolygon) =>
        new FeatureCollection(new List<Feature> { myPolygon });
}
