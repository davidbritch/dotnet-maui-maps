#if IOS || MACCATALYST
using PlatformView = MapKit.MKMapView;
#elif ANDROID
using Android.Gms.Maps;
using PlatformView = Android.Gms.Maps.MapView;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0 && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif
using Microsoft.Maui.Handlers;
using IMap = MapDemo.Controls.IMap;

namespace MapDemo.Handlers;

public partial class MapHandler : IMapHandler
{
    public static IPropertyMapper<IMap, IMapHandler> Mapper = new PropertyMapper<IMap, IMapHandler>(ViewHandler.ViewMapper)
    {
        [nameof(IMap.MapType)] = MapMapType,
        [nameof(IMap.IsShowingUser)] = MapIsShowingUser,
        [nameof(IMap.HasScrollEnabled)] = MapHasScrollEnabled,
        [nameof(IMap.HasTrafficEnabled)] = MapHasTrafficEnabled,
        [nameof(IMap.HasZoomEnabled)] = MapHasZoomEnabled
    };

    IMap IMapHandler.VirtualView => VirtualView;

    PlatformView IMapHandler.PlatformView => PlatformView;

    public MapHandler() : base(Mapper)
    {
    }
}