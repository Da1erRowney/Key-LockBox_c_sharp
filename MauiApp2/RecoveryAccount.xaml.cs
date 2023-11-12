using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MauiApp2;

public partial class RecoveryAccount : ContentPage
{
    private DatabaseServiceUser _databaseService;

    public RecoveryAccount()
	{
		InitializeComponent();
        string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
        _databaseService = new DatabaseServiceUser(databasePath);
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
    }

    private async void OnRecovery(object sender, EventArgs e)
    {
        string password1 = EntryPassword1.Text;
        string email = EntryMail.Text;
        string saveKey = EntrySaveKey.Text;

        if (string.IsNullOrEmpty(password1) || string.IsNullOrEmpty(email)|| string.IsNullOrEmpty(saveKey))
        {
            await DisplayAlert("Ошибка", "Не все поля заполнены", "OK");
            return;
        }

        if (password1.Length < 8 || !HasLetterAndDigit(password1))
        {
            await DisplayAlert("Ошибка", "Пароль должен содержать не менее 8 символов и включать и буквы, и цифры", "Ок");
            return;
        }
        email = email.ToLower();

        bool isAuthenticated = AuthenticateUser(email, saveKey);
        if (isAuthenticated)
        {
            string salt = email.Split('@')[0];
            string hashedPassword = HashPassword(password1, salt);
            User user = _databaseService.GetUserByEmail(email);
            user.Password = hashedPassword;
            _databaseService.UpdateUser(user); // Обновление записи пользователя в базе данных

            await Navigation.PopModalAsync();
        }

        else
        {
            await DisplayAlert("Ошибка", "Неправильный email или код восстановления", "OK");
        }
    }
    private bool HasLetterAndDigit(string input)
    {
        return Regex.IsMatch(input, @"[a-zA-Z]") && Regex.IsMatch(input, @"\d");
    }
    private bool AuthenticateUser(string email, string saveKey)
    {
        User user = _databaseService.GetUserByEmail(email);
        if (user != null)
        {

            return user.SaveKey == saveKey;
        }
        return false;
    }
    private string HashPassword(string password, string salt)
    {
        string saltedPassword = password + salt;

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(saltedPassword);

            byte[] hashedBytes = sha256.ComputeHash(bytes);

            string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();


            return hashedPassword;
        }
    }

    private async void OnGoBackTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}