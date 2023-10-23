namespace MauiApp2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SingIn());
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SingUp());
        }
    }
}