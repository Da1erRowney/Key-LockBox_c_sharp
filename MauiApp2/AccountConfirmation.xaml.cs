using System.Security.Cryptography;
using System.Text;


namespace MauiApp2;

public partial class AccountConfirmation : ContentPage
{
    public string CurrentUserEmail { get; set; }
    public string CurrentUserPassword { get; set; }
    public AccountConfirmation()
    {
        InitializeComponent();
    }
    [Obsolete]
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // �������� ��������� �������� IsAnimationPlaying ����� 3 �������
        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                // �������� �������� IsAnimationPlaying �� True
                gif.IsAnimationPlaying = true;
            });

            return false; // ���������� ������ ����� ������ ����������
        });
        UpdateUserLabel();

    }
    private void UpdateUserLabel()
    {
        var userEmail = !string.IsNullOrEmpty(CurrentUserEmail) ? CurrentUserEmail : "Unknown";
        var labelText = $"������������ {userEmail}";
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

        if (string.IsNullOrEmpty(password))
        {
            await DisplayAlert("������", "�� ��� ���� ���������", "OK");
            return;
        }
        else if (hashedPassword == CurrentUserPassword)
        {
            ChangeAccountDetails changeAccountDetails = new ChangeAccountDetails();
            changeAccountDetails.CurrentUserEmail = CurrentUserEmail; // �������� �������� CurrentUserEmail
            changeAccountDetails.CurrentUserPassword = password;
            await Navigation.PushModalAsync(changeAccountDetails);
        }
        else if (password != CurrentUserPassword)
        {
            await DisplayAlert("������", "�� ��������� ������ ������", "OK");
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