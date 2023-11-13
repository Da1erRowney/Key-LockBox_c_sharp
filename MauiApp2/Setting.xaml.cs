namespace MauiApp2
{
    public partial class Setting : ContentPage
    {
        private DatabaseServiceUser _databaseService;
        string CurrentUserEmail = SingUp.CurrentUserEmail;
        string CurrentUserPassword = SingUp.CurrentUserPassword;
        public static string statusSort;

        private DatabaseServiceUser databaseService; // Объявление переменной как поле класса

        public Setting()
        {
            InitializeComponent();
            sort.SelectedItem = statusSort;
            CheckHintsBasics();


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
        private void CheckHintsBasics()
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            databaseService = new DatabaseServiceUser(databasePath); // Инициализация переменной

            // Получите информацию о текущем пользователе
            User currentUser = databaseService.GetUserByEmail(CurrentUserEmail);

            if (currentUser.HintsSetting == "NoN")
            {
                // Всплывающее уведомление
                DisplayAlert("Подсказка", "Принудительно вас просим создать собственный PIN-код. Для этого нажмите Изменить данные учетной записи -> подтвердите свои данные и введите пинкод. ", "OK");
                DisplayAlert("Подсказка", "Здесь вы можете изменить данные своей учетной записи, добавить PIN-код для дальнейшего входа, сменить пользовательскую тему и выйти из аккаунта.", "OK");
                // Обновите значение поля HintsBasics в базе данных
                currentUser.HintsSetting = "Active";
                databaseService.UpdateUser(currentUser);
            }

            databaseService.CloseConnection();
        }
        private async void OnThemeToggled(object sender, ToggledEventArgs e)
        {
            bool isDarkTheme = e.Value;
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            DatabaseServiceUser databaseService = new DatabaseServiceUser(databasePath);
            User currentUser = databaseService.GetUserByEmail(CurrentUserEmail);

            if (isDarkTheme)
            {
                currentUser.ThemeApplication = "Dark";

                databaseService.UpdateUser(currentUser);
                // Применить темную тему
                Application.Current.UserAppTheme = AppTheme.Dark;
            }
            else
            {
                // Применить светлую тему
                currentUser.ThemeApplication = "Light";

                databaseService.UpdateUser(currentUser);
                Application.Current.UserAppTheme = AppTheme.Light;
            }

            await Navigation.PushAsync(new Setting());
            Navigation.RemovePage(this);
        }
        private async void ChangeAccountDetails(object sender, EventArgs e)
        {
            AccountConfirmation accountConfirmation = new AccountConfirmation();
            accountConfirmation.CurrentUserEmail = SingUp.CurrentUserEmail; // Передача значения CurrentUserEmail
            accountConfirmation.CurrentUserPassword = SingUp.CurrentUserPassword; // Передача значения CurrentUserPassword
            await Navigation.PushModalAsync(accountConfirmation);// Используйте changeAccountDetails для навигации
        }

        private async void ExitAccount(object sender, EventArgs e)
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            _databaseService = new DatabaseServiceUser(databasePath);
            User user = _databaseService.GetUserByEmail(CurrentUserEmail);
            user.StatusAccount = "Off";
            _databaseService.UpdateUser(user);
            await Navigation.PushModalAsync(new MainPage());

        }

        private async void OnGoBackTapped(object sender, EventArgs e)
        {
           var basicsPage = new BasicsPage();
            await Navigation.PushModalAsync(basicsPage);
        }

        private async void DeleteAccount(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Удаление", "Вы уверены что хотите удалить ваш аккаунт?", "Да", "Нет");

            if (result)
            {
                string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
                _databaseService = new DatabaseServiceUser(databasePath);
                _databaseService.DeleteUserByEmail(CurrentUserEmail);
                await Navigation.PushModalAsync(new MainPage());
            }
        }

        private async Task<bool> alert()
        {
            return await DisplayAlert("Удаление", "Вы уверены что хотите удалить ваш аккаунт?", "Да", "Нет");
        }


        private async void informationPage(object sender, TappedEventArgs e)
        {
            await Navigation.PushModalAsync(new Information());
        }

        private void OnSortPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            DatabaseServiceUser databaseService = new DatabaseServiceUser(databasePath);
            User currentUser = databaseService.GetUserByEmail(CurrentUserEmail);

            statusSort = (string)sort.SelectedItem;
            currentUser.StatusSort = statusSort;
            databaseService.UpdateUser(currentUser);

        }

        
    }
}