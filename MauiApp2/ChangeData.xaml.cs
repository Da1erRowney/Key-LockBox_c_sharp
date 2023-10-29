namespace MauiApp2;
using PersonalsData;
using SQLite;

public partial class ChangeData : ContentPage
{
    private PersonalData selectedData;
    public ChangeData(PersonalData selectedData)
	{
		InitializeComponent();
        this.selectedData = selectedData;

        EntryName.Text =selectedData.Name;
        EntryLogin.Text =selectedData.Login;
        EntryEmail.Text =selectedData.Email;
        EntryPassword.Text =selectedData.Password;
        EntryOtherData.Text = selectedData.OtherData;
    }

    private async void OnGoBackTapped(object sender, TappedEventArgs e)
    {
       await Navigation.PopModalAsync();
    }

    private void RenameData(object sender, EventArgs e)
    {
        DateTime currentDate = DateTime.UtcNow;
        DateTime newDate = currentDate.AddHours(+3);
        string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "personalData.db");
        //string databasePath = @"C:\Users\Игорь Черненко\source\repos\MauiApp2\MauiApp2\personalData.db";
        DatabaseServicePersonalData databaseService = new DatabaseServicePersonalData(databasePath);

        // Обновите данные выбранного элемента
        selectedData.Name = EntryName.Text;
        selectedData.Login = EntryLogin.Text;
        selectedData.Email = EntryEmail.Text;
        selectedData.Password = EntryPassword.Text;
        selectedData.OtherData = EntryOtherData.Text;
        selectedData.LastModifiedDate = newDate.ToString("yyyy-MM-dd HH:mm:ss");

        // Вызовите метод обновления данных в базе данных
        databaseService.UpdatePersonalData(selectedData);

        // Закройте соединение с базой данных
        databaseService.CloseConnection();

        // Вернитесь на предыдущую страницу
       
        Navigation.PushModalAsync(new BasicsPage());
    }

   
}