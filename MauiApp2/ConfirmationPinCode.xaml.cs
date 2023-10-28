namespace MauiApp2;

public partial class ConfirmationPinCode : ContentPage
{
    private DatabaseServiceUser _databaseService;
   
    public ConfirmationPinCode()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateUserLabel();
    }
    private void UpdateUserLabel()
    {
        
        var userEmail = App.CurrentUserEmail;
        var labelText = $"Пользователь {userEmail}";
        UserLabel.Text = labelText;
    }
    private async void OnGoBackTapped(object sender, TappedEventArgs e)
    {
        string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
        _databaseService = new DatabaseServiceUser(databasePath);
        User user = _databaseService.GetUserByEmail(App.CurrentUserEmail);
        user.StatusAccount = "Off";
        _databaseService.UpdateUser(user);
        await Navigation.PushModalAsync(new MainPage());

    }

    private async void Confirmation(object sender, EventArgs e)
    {
        string pincode = PinCodeBtn.Text;
        if (string.IsNullOrEmpty(pincode))
        {
            await DisplayAlert("Ошибка", "Не все поля заполнены", "OK");
            return;
        }
        if (pincode == App.CurrentUserPinCode)
        {
            
            SingUp.CurrentUserEmail   = App.CurrentUserEmail   ;
            SingUp.CurrentUserPassword = App.CurrentUserPassword ;
            await Navigation.PushModalAsync(new BasicsPage());

        }
        else
        {
            await DisplayAlert("Ошибка", "Не правильно введен PIN code", "OK");
            return;
        }
    }
}