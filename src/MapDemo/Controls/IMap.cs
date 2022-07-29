namespace MapDemo.Controls
{
    public interface IMap : IView
    {
        MapType MapType { get; }
        bool IsShowingUser { get; }
        bool HasScrollEnabled { get; }
        bool HasZoomEnabled { get; }
        bool HasTrafficEnabled { get; }
    }
}
