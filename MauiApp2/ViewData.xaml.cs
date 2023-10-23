using System.Xml;
using PersonalsData;

namespace MauiApp2
{
    public partial class ViewData : ContentPage
    {
        private PersonalData selectedData; // Объявление переменной класса

        public ViewData(PersonalData selectedData)
        {
            InitializeComponent();

            // Сохраняем выбранные данные в переменную класса
            this.selectedData = selectedData;

            Name.Text = "Название: " + selectedData.Name;
            Login.Text = "Логин: " + selectedData.Login;
            Email.Text = "Почта: " + selectedData.Email;
            Password.Text = "Пароль: " + selectedData.Password;
            OtherData.Text = "Прочие данные: " + selectedData.OtherData;
        }

        private async void OnGoBackTapped(object sender, TappedEventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void DeleteData(object sender, EventArgs e)
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "personalData.db");
           // string databasePath = @"C:\Users\Игорь Черненко\source\repos\MauiApp2\MauiApp2\personalData.db";
            DatabaseServicePersonalData databaseService = new DatabaseServicePersonalData(databasePath);

            // Удалите выбранные данные из базы данных
            databaseService.DeletePersonalData(selectedData);

            // Закройте соединение с базой данных
            databaseService.CloseConnection();

            // Вернитесь на предыдущую страницу
            Navigation.PushModalAsync(new BasicsPage());
        }



        private void RenamesData(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ChangeData(selectedData));
        }
    }
}