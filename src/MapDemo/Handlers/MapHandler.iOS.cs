using CoreLocation;
using MapDemo.Controls;
using MapKit;
using Microsoft.Maui.Handlers;
using IMap = MapDemo.Controls.IMap;

namespace MapDemo.Handlers
{
    public partial class MapHandler : ViewHandler<IMap, MKMapView>
    {
        MKMapView? _mkMapView;
        CLLocationManager? _locationManager;

        protected override MKMapView CreatePlatformView()
        {
            _mkMapView = new MKMapView();
            return _mkMapView;
        }

        protected override void ConnectHandler(MKMapView platformView)
        {
            base.ConnectHandler(platformView);
            _locationManager = new CLLocationManager();
        }

        protected override void DisconnectHandler(MKMapView platformView)
        {
            _mkMapView?.Dispose();

            base.DisconnectHandler(platformView);
        }

        public static void MapMapType(IMapHandler handler, IMap map)
        {
            switch (map.MapType)
            {
                case MapType.Street:
                    handler.PlatformView.MapType = MKMapType.Standard;
                    break;
                case MapType.Satellite:
                    handler.PlatformView.MapType = MKMapType.Satellite;
                    break;
                case MapType.Hybrid:
                    handler.PlatformView.MapType = MKMapType.Hybrid;
                    break;
            }
        }

        public static void MapIsShowingUser(IMapHandler handler, IMap map)
        {
#if !MACCATALYST
            if (map.IsShowingUser)
            {
                MapHandler? mapHandler = handler as MapHandler;
                mapHandler?._locationManager?.RequestWhenInUseAuthorization();
            }
#endif
            handler.PlatformView.ShowsUserLocation = map.IsShowingUser;
        }

        public static void MapHasScrollEnabled(IMapHandler handler, IMap map)
        {
            handler.PlatformView.ScrollEnabled = map.HasScrollEnabled;
        }

        public static void MapHasTrafficEnabled(IMapHandler handler, IMap map)
        {
            handler.PlatformView.ShowsTraffic = map.HasTrafficEnabled;
        }

        public static void MapHasZoomEnabled(IMapHandler handler, IMap map)
        {
            handler.PlatformView.ZoomEnabled = map.HasZoomEnabled;
        }
    }
}
