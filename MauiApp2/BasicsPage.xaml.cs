using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PersonalsData;
using SQLite;

namespace MauiApp2
{
    public partial class BasicsPage : ContentPage, INotifyPropertyChanged
    {
        private List<PersonalData> _personalDataList;
        string CurrentUserEmail = SingUp.CurrentUserEmail;
        public List<PersonalData> PersonalDataList
        {
            get { return _personalDataList; }
            set
            {
                _personalDataList = value;
                OnPropertyChanged(nameof(PersonalDataList));
            }
        }

        public BasicsPage()
        {
            InitializeComponent();
            InitializePersonalDataList();
            BindingContext = this;

            // Проверка значения поля HintsBasics в базе данных
            CheckHintsBasics();
        }

        private void InitializePersonalDataList()
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "personalData.db");
            DatabaseServicePersonalData databaseService = new DatabaseServicePersonalData(databasePath);

            // Получите все персональные данные из базы данных
            List<PersonalData> allPersonalData = databaseService.GetAllPersonalData();

            // Отфильтруйте данные, соответствующие текущему пользователю
            PersonalDataList = allPersonalData
                .Where(data => data.EmailUser == CurrentUserEmail)
                .OrderBy(data => data.Name)
                .ToList();

            databaseService.CloseConnection();
        }
        private void CheckHintsBasics()
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
            DatabaseServiceUser databaseService = new DatabaseServiceUser(databasePath);

            // Получите информацию о текущем пользователе
            User currentUser = databaseService.GetUserByEmail(CurrentUserEmail);

            if (currentUser.HintsBasics == "NoN")
            {
                // Всплывающее уведомление
                DisplayAlert("Подсказка", "На данной странице вы можете создать кейсы для хранения ваших данных и их последующий просмотр. Так же сверху слева есть вкладка настройки.", "OK");

                // Обновите значение поля HintsBasics в базе данных
                currentUser.HintsBasics = "Active";
                databaseService.UpdateUser(currentUser);
            }

            databaseService.CloseConnection();
        }
        private async void OnAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddPunct());
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            //var settingPage = new Setting(SettingsButton);
            await Navigation.PushModalAsync(new Setting());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is PersonalData tappedData)
            {
                await Navigation.PushModalAsync(new ViewData(tappedData));
            }

            // Сбросьте выделение элемента
            PersonalDataListView.SelectedItem = null;
        }
    }
}