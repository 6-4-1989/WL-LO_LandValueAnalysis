using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoJSON.Net;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;

namespace LandValueScraper.Models;

public record LandValueDataDTO(
    string Address,
    double? MarketTotalValue,
    double? LandValuePerAcre, //market total value divided by lot size in acres
    double? TaxableValue, //median property tax rate * market total value
    double? TaxableValuePerAcre, //taxable value divided by lot size
    double? LotSize,
    double? LotCoverage, //building footprint compared to lot size
    string? BuildingDescription,
    ParcelGeometry Geometry
    );

public record ParcelGeometry(
    FeatureCollection? ParcelPolygon,
    FeatureCollection Location
    );