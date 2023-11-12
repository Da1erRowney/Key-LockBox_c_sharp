using Plugin.Fingerprint.Abstractions;

namespace MauiApp2;

public partial class ConfirmationPinCode : ContentPage
{
    private DatabaseServiceUser _databaseService;
    private readonly IFingerprint fingerprint;

    public ConfirmationPinCode(IFingerprint fingerprint)
    {
        InitializeComponent();
        this.fingerprint = fingerprint;
    }
    [Obsolete]
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Отложить изменение свойства IsAnimationPlaying через 3 секунды
        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                // Изменить свойство IsAnimationPlaying на True
                gif.IsAnimationPlaying = true;
            });

            return false; // Остановить таймер после одного выполнения
        });
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
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            _databaseService = new DatabaseServiceUser(databasePath);
            User user = _databaseService.GetUserByEmail(App.CurrentUserEmail);

            SingUp.CurrentUserEmail = App.CurrentUserEmail;
            SingUp.CurrentUserPassword = App.CurrentUserPassword;
            Setting.statusSort = user.StatusSort;
            await Navigation.PushModalAsync(new BasicsPage());

        }
        else
        {
            await DisplayAlert("Ошибка", "Не правильно введен PIN-код", "OK");
            return;
        }
    }

    private async void OnBiometricClicked(object sender, EventArgs e)
    {
        if (fingerprint != null)
        {
            var request = new AuthenticationRequestConfiguration("Подвердите отпечаток пальца", "Или войдите используя PIN-код.");
            var result = await fingerprint.AuthenticateAsync(request);
            if (result.Authenticated)
            {
                string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
                _databaseService = new DatabaseServiceUser(databasePath);
                User user = _databaseService.GetUserByEmail(App.CurrentUserEmail);

                SingUp.CurrentUserEmail = App.CurrentUserEmail;
                SingUp.CurrentUserPassword = App.CurrentUserPassword;
                Setting.statusSort = user.StatusSort;
                await Navigation.PushModalAsync(new BasicsPage());
            }
            else
            {
                await DisplayAlert("Ошибка", "Отпечаток не распознан", "OK");
            }
        }
        else
        {
            // Обработка, если fingerprint равен null
            await DisplayAlert("Error", "Fingerprint not initialized", "OK");
        }
    }
}