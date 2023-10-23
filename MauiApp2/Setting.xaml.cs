using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;



namespace MauiApp2
{
    public partial class Setting : ContentPage
    {
        public string CurrentUserEmail { get; set; }
        public string CurrentUserPassword { get; set; }
        //private ImageButton settingsButton; // Приватное поле для хранения ссылки на SettingsBtn
        //public Setting(ImageButton settingsBtn)
        //{
        //    InitializeComponent();

        //    settingsButton = settingsBtn;
        //}

        public Setting()
        {
            InitializeComponent();
        }

        private async void OnSelectingThemeIndexChanged(object sender, EventArgs e)
        {
            if ((string)SelectingThemePicker.SelectedItem == "Светлая")
            {
                // Применить светлую тему
                Application.Current.UserAppTheme = AppTheme.Light;

                // Изменить источник изображения для светлой темы
               // settingsButton.Source = "setting8.png"; // Используйте сохраненную ссылку на SettingsBtn
                await Navigation.PushModalAsync(new Setting());
            }
            else if ((string)SelectingThemePicker.SelectedItem == "Темная")
            {
                // Применить темную тему
                Application.Current.UserAppTheme = AppTheme.Dark;

                // Изменить источник изображения для темной темы
               // settingsButton.Source = "setting9.png"; // Используйте сохраненную ссылку на SettingsBtn
                await Navigation.PushModalAsync(new Setting());
            }
            else
            {
                // Применить системную тему
                Application.Current.UserAppTheme = AppTheme.Unspecified;
                await Navigation.PushModalAsync(new Setting());
            }
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