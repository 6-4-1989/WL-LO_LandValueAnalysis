using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Location;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Security;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks;
using Esri.ArcGISRuntime.UI;
using LandValueAnalysis.Services;

namespace LandValueAnalysis.ViewModels
{
    public class InSession : BaseNotificationClass
    {
        private Map _theMap;
        public Map TheMap
        {
            get => _theMap;
            private set
            {
                _theMap = value;
                OnPropertyChanged();
            }
        }

        public InSession()
        {
            InitializeMap();
        }

        private void InitializeMap()
        {
            Map map = new Map(BasemapStyle.ArcGISTopographic);

            //set initial view centered around West Linn

            MapPoint initialMapView = new MapPoint(-122.630083, 45.366194, SpatialReferences.Wgs84);
            map.InitialViewpoint = new Viewpoint(initialMapView, 100000);

            TheMap = map;
        }
    }
}
