namespace MauiApp2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

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