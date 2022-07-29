using System.Diagnostics;
using MapDemo.Controls;

namespace MapDemo;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

        PermissionStatus result = await CheckAndRequestLocationPermission();
        Debug.WriteLine("Location permissions: " + result.ToString());
	}

    async Task<PermissionStatus> CheckAndRequestLocationPermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status == PermissionStatus.Granted)
            return status;

        if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
        {
            // Prompt the user to turn on in settings
            // On iOS once a permission has been denied it may not be requested again from the application
            return status;
        }

        if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
        {
            // Prompt the user with additional information as to why the permission is needed
        }

        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        return status;
    }

    void OnMapTypePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        Picker picker = (Picker)sender;

        switch (picker.SelectedItem.ToString())
        {
            default:
            case "Street":
                map.MapType = MapType.Street;
                break;
            case "Satellite":
                map.MapType = MapType.Satellite;
                break;        
            case "Hybrid":
                map.MapType = MapType.Hybrid;
                break;
        }
    }

    void OnContentPageUnloaded(object sender, EventArgs e)
    {
        map.Handler?.DisconnectHandler();
    }
}
