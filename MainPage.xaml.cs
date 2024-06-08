namespace MAUI_Notification;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
		if (status != PermissionStatus.Granted)
		{
			status = await Permissions.RequestAsync<Permissions.PostNotifications>();
            if (status != PermissionStatus.Granted)
			{
                await DisplayAlert("Permission Denied", "To stay informed about important updates, please enable notification permissions in your settings.", "OK");
            }

        }
        base.OnAppearing();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
        var result = status == PermissionStatus.Granted ? "Granted" : "Not granted";
        await DisplayAlert("MAUI Notification", $"Notification permission: {result}","OK");
    }
}

