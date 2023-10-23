using Microsoft.Maui.ApplicationModel.Communication;

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
        await Navigation.PopModalAsync();
    }

    private async void Confirmation(object sender, EventArgs e)
    {
        string password = PasswordBtn.Text;
        if (string.IsNullOrEmpty(password) )
        {
            await DisplayAlert("Ошибка", "Не все поля заполнены", "OK");
            return;
        }
        else if (password == CurrentUserPassword) {
            ChangeAccountDetails changeAccountDetails = new ChangeAccountDetails();
            changeAccountDetails.CurrentUserEmail = CurrentUserEmail; // Передача значения CurrentUserEmail
            changeAccountDetails.CurrentUserPassword = CurrentUserPassword;
            await Navigation.PushModalAsync(changeAccountDetails);
        }
        else if (password != CurrentUserPassword)
        {
            await DisplayAlert("Ошибка", "Не правильно введен пароль", "OK");
            return;
        }
        
    }
}