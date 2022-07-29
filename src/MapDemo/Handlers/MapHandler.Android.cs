using Android;
using Android.Content.PM;
using Android.Gms.Maps;
using AndroidX.Core.Content;
using MapDemo.Controls;
using Microsoft.Maui.Handlers;
using Debug = System.Diagnostics.Debug;
using IMap = MapDemo.Controls.IMap;

namespace MapDemo.Handlers
{
    public partial class MapHandler : ViewHandler<IMap, MapView>
    {
        MapView? _mapView;
        MapCallbackHandler? _mapReady;
        
        public GoogleMap? Map { get; set; }

        protected override MapView CreatePlatformView()
        {
            _mapView = new MapView(Context);
            _mapView.OnCreate(null);
            _mapView.OnResume();
            return _mapView;
        }

        protected override void ConnectHandler(MapView platformView)
        {
            base.ConnectHandler(platformView);

            _mapReady = new MapCallbackHandler(this);
            platformView.GetMapAsync(_mapReady);
        }

        protected override void DisconnectHandler(MapView platformView)
        {
            _mapReady = null;
            _mapView?.Dispose();

            base.DisconnectHandler(platformView);
        }

        public static void MapMapType(IMapHandler handler, IMap map)
        {
            GoogleMap? googleMap = handler?.Map;
            if (googleMap == null)
                return;

            googleMap.MapType = map.MapType switch
            {
                MapType.Street => GoogleMap.MapTypeNormal,
                MapType.Satellite => GoogleMap.MapTypeSatellite,
                MapType.Hybrid => GoogleMap.MapTypeHybrid,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static void MapIsShowingUser(IMapHandler handler, IMap map)
        {
            GoogleMap? googleMap = handler?.Map;
            if (googleMap == null)
                return;

            if (handler?.MauiContext?.Context == null)
                return;

            if (map.IsShowingUser)
            {
                var coarseLocationPermission = ContextCompat.CheckSelfPermission(handler.MauiContext.Context, Manifest.Permission.AccessCoarseLocation);
                var fineLocationPermission = ContextCompat.CheckSelfPermission(handler.MauiContext.Context, Manifest.Permission.AccessFineLocation);

                if (coarseLocationPermission == Permission.Granted || fineLocationPermission == Permission.Granted)
                    googleMap.MyLocationEnabled = googleMap.UiSettings.MyLocationButtonEnabled = true;
                else
                {
                    Debug.WriteLine("Missing location permissions for IsShowingUser.");
                    googleMap.MyLocationEnabled = googleMap.UiSettings.MyLocationButtonEnabled = false;
                }
            }
            else
            {
                googleMap.MyLocationEnabled = googleMap.UiSettings.MyLocationButtonEnabled = false;
            }
        }

        public static void MapHasScrollEnabled(IMapHandler handler, IMap map)
        {
            GoogleMap? googleMap = handler?.Map;
            if (googleMap == null)
                return;

            googleMap.UiSettings.ScrollGesturesEnabled = map.HasScrollEnabled;
        }

        public static void MapHasTrafficEnabled(IMapHandler handler, IMap map)
        {
            GoogleMap? googleMap = handler?.Map;
            if (googleMap == null)
                return;

            googleMap.TrafficEnabled = map.HasTrafficEnabled;
        }

        public static void MapHasZoomEnabled(IMapHandler handler, IMap map)
        {
            GoogleMap? googleMap = handler?.Map;
            if (googleMap == null)
                return;

            googleMap.UiSettings.ZoomControlsEnabled = map.HasZoomEnabled;
            googleMap.UiSettings.ZoomGesturesEnabled = map.HasZoomEnabled;
        }

        internal void OnMapReady(GoogleMap map)
        {
            if (map == null)
                return;

            Map = map;
        }
    }

    class MapCallbackHandler : Java.Lang.Object, IOnMapReadyCallback
    {
        MapHandler _handler;
        GoogleMap? _googleMap;

        public MapCallbackHandler(MapHandler mapHandler)
        {
            _handler = mapHandler;
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _googleMap = googleMap;
            _handler.OnMapReady(googleMap);
        }
    }
}
