using System;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;


namespace MauiApp2
{
    public partial class SingUp : ContentPage
    {
        private DatabaseServiceUser _databaseService;
        public static string CurrentUserEmail { get; private set; }
        public static string CurrentUserPassword { get; private set; }
        public SingUp()
        {
            InitializeComponent();
            //string databasePath = @"C:\Users\Игорь Черненко\source\repos\MauiApp2\MauiApp2\user.db";
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            _databaseService = new DatabaseServiceUser(databasePath);
        }

        private async void OnComeClicked(object sender, EventArgs e)
        {
            string password1 = EntryPassword1.Text;
            string email = EntryMail.Text;
            email = email.ToLower();
            // Проверка на пустоту полей
            if (string.IsNullOrEmpty(password1) || string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Ошибка", "Не все поля заполнены", "OK");
                return;
            }

            // Проверка авторизации в базе данных
            bool isAuthenticated = AuthenticateUser(email, password1);
            if (isAuthenticated)
            {

                User user = _databaseService.GetUserByEmail(email);
                await DisplayAlert("Успех", "Вы авторизовались", "OK");
                BasicsPage basicsPage = new BasicsPage();
                user.StatusAccount = "On";
                _databaseService.UpdateUser(user); // Обновление записи пользователя в базе данных
                await Navigation.PushModalAsync(basicsPage);
                basicsPage.Unfocus();
            }
            else
            {
                await DisplayAlert("Ошибка", "Неправильный email или пароль", "OK");
            }
        }

        private bool AuthenticateUser(string email, string password)
        {
            User user = _databaseService.GetUserByEmail(email);
            if (user != null)
            {
                CurrentUserEmail = email;
                CurrentUserPassword = password;
                string hashedPassword = (password);
                return user.Password == hashedPassword;
            }
            return false;
        }

        //private string HashPassword(string password)
        //{
        //    using (SHA256 sha256Hash = SHA256.Create())
        //    {
        //        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
        //        StringBuilder builder = new StringBuilder();
        //        for (int i = 0; i < bytes.Length; i++)
        //        {
        //            builder.Append(bytes[i].ToString("x2"));
        //        }
        //        return builder.ToString();
        //    }
        //}

        private async void OnGoBackTapped(object sender, TappedEventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}