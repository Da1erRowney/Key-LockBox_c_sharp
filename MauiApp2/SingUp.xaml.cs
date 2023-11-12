using System.Security.Cryptography;
using System.Text;


namespace MauiApp2
{
    public partial class SingUp : ContentPage
    {
        private DatabaseServiceUser _databaseService;
        public static string CurrentUserEmail { get; set; }
        public static string CurrentUserPassword { get; set; }

        public SingUp()
        {
            InitializeComponent();
            //string databasePath = @"C:\Users\Игорь Черненко\source\repos\MauiApp2\MauiApp2\user.db";
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
        private async void OnComeClicked(object sender, EventArgs e)
        {
            string password1 = EntryPassword1.Text;
            string email = EntryMail.Text;
            // Проверка на пустоту полей
            if (string.IsNullOrEmpty(password1) || string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Ошибка", "Не все поля заполнены", "OK");
                return;
            }

            email = email.ToLower();
            // Проверка авторизации в базе данных
            bool isAuthenticated = AuthenticateUser(email, password1);
            if (isAuthenticated)
            {

                User user = _databaseService.GetUserByEmail(email);
                Setting.statusSort = user.StatusSort;
               
                BasicsPage basicsPage = new BasicsPage();
                user.StatusAccount = "On";
                if (user.ThemeApplication == "Dark")
                {
                    Application.Current.UserAppTheme = AppTheme.Dark;
                }
                else if (user.ThemeApplication == "Light")
                {
                    Application.Current.UserAppTheme = AppTheme.Light;
                }
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
                string salt = email.Split('@')[0];
                string hashedPassword = HashPassword(password, salt);

                CurrentUserEmail = email;
                CurrentUserPassword = hashedPassword;
                return user.Password == hashedPassword;
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

        private async void OnRecoveryKey(object sender, TappedEventArgs e)
        {
            await Navigation.PushModalAsync( new RecoveryAccount());
        }
    }
}