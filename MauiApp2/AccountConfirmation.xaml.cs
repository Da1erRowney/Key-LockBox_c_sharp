using Microsoft.Maui.ApplicationModel.Communication;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using MauiApp2;
using SQLite;
using static System.Security.Cryptography.SHA256;


namespace MauiApp2;

public partial class AccountConfirmation : ContentPage
{
    public string CurrentUserEmail { get; set; }
    public string CurrentUserPassword { get; set; }
    public AccountConfirmation()
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
        var userEmail = !string.IsNullOrEmpty(CurrentUserEmail) ? CurrentUserEmail : "Unknown";
        var labelText = $"Пользователь {userEmail}";
        UserLabel.Text = labelText;
    }
    private async void OnGoBackTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Setting());
    }

    private async void Confirmation(object sender, EventArgs e)
    {
        string password = PasswordBtn.Text;
        string salt = CurrentUserEmail.Split('@')[0];
        string hashedPassword = HashPassword(password, salt);

        if (string.IsNullOrEmpty(password) )
        {
            await DisplayAlert("Ошибка", "Не все поля заполнены", "OK");
            return;
        }
        else if (hashedPassword == CurrentUserPassword) {
            ChangeAccountDetails changeAccountDetails = new ChangeAccountDetails();
            changeAccountDetails.CurrentUserEmail = CurrentUserEmail; // Передача значения CurrentUserEmail
            changeAccountDetails.CurrentUserPassword = password;
            await Navigation.PushModalAsync(changeAccountDetails);
        }
        else if (password != CurrentUserPassword)
        {
            await DisplayAlert("Ошибка", "Не правильно введен пароль", "OK");
            return;
        }
        
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
}