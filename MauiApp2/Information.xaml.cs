namespace MauiApp2;

public partial class Information : ContentPage
{
    public Information()
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
    private async void OnGoBackTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Setting());
    }
}