using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;



namespace MauiApp2
{
    public partial class Setting : ContentPage
    {
        string CurrentUserEmail = SingUp.CurrentUserEmail;
        string CurrentUserPassword = SingUp.CurrentUserPassword;

        //private ImageButton settingsButton; // Приватное поле для хранения ссылки на SettingsBtn
        //public Setting(ImageButton settingsBtn)
        //{
        //    InitializeComponent();

        //    settingsButton = settingsBtn;
        //}

        public Setting()
        {
            InitializeComponent();
            CheckHintsBasics();
        }
        private void CheckHintsBasics()
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            DatabaseServiceUser databaseService = new DatabaseServiceUser(databasePath);

            // Получите информацию о текущем пользователе
            User currentUser = databaseService.GetUserByEmail(CurrentUserEmail);

            if (currentUser.HintsSetting == "NoN")
            {
                // Всплывающее уведомление
                DisplayAlert("Подсказка", "Здесь вы можете изменить данные своей учетной записи, сменить пользовательскую тему и выйти из аккаунта.", "OK");

                // Обновите значение поля HintsBasics в базе данных
                currentUser.HintsSetting = "Active";
                databaseService.UpdateUser(currentUser);
            }

            databaseService.CloseConnection();
        }
        private async void OnThemeToggled(object sender, ToggledEventArgs e)
        {
            bool isDarkTheme = e.Value;

            if (isDarkTheme)
            {
                // Применить темную тему
                Application.Current.UserAppTheme = AppTheme.Dark;
            }
            else
            {
                // Применить светлую тему
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

           // await Navigation.PushModalAsync(new AccountConfirmation());
        
        
        }

        private async void ExitAccount(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new MainPage());
        }

        private async void OnGoBackTapped(object sender, EventArgs e)
        {
            var basicsPage = new BasicsPage();
            await Navigation.PushModalAsync(basicsPage);
        }

       
    }
}