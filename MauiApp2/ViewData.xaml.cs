using System.Xml;
using PersonalsData;

namespace MauiApp2
{
    public partial class ViewData : ContentPage
    {
        private PersonalData selectedData; // Объявление переменной класса
        string CurrentUserEmail = SingUp.CurrentUserEmail;
        public ViewData(PersonalData selectedData)
        {
            InitializeComponent();
            CheckHintsBasics();
            // Сохраняем выбранные данные в переменную класса
            this.selectedData = selectedData;

            Name.Text = "Название: " + selectedData.Name;
            Login.Text = "Логин: " + selectedData.Login;
            Email.Text = "Почта: " + selectedData.Email;
            Password.Text = "Пароль: " + selectedData.Password;
            OtherData.Text = "Прочие данные: " + selectedData.OtherData;
            DataCreation.Text = "Дата создания: " + selectedData.DateCreation;
            DataModification.Text = "Дата последнего изменения: " + selectedData.LastModifiedDate;
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
            DatabaseServiceUser databaseService = new DatabaseServiceUser(databasePath);

            // Получите информацию о текущем пользователе
            User currentUser = databaseService.GetUserByEmail(CurrentUserEmail);

            if (currentUser.HintsData == "NoN")
            {
                // Всплывающее уведомление
                DisplayAlert("Подсказка", "Это ваш кейс с вашими данными. Мы используем одно из самых эффективных шифрований для защиты ваших данных. Вы можете изменить ваши данные или же фатального их удалить без возможности восстановления.", "OK");

                // Обновите значение поля HintsBasics в базе данных
                currentUser.HintsData = "Active";
                databaseService.UpdateUser(currentUser);
            }

            databaseService.CloseConnection();
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