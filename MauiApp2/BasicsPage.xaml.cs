using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.ComponentModel;
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

        ///public ImageButton SettingsButton => SettingsBtn;

        public BasicsPage()
        {
            InitializeComponent();
            InitializePersonalDataList();
            BindingContext = this;
        }

        private void InitializePersonalDataList()
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "personalData.db");
            //string databasePath = @"C:\Users\Игорь Черненко\source\repos\MauiApp2\MauiApp2\personalData.db";
            DatabaseServicePersonalData databaseService = new DatabaseServicePersonalData(databasePath);

            // Получите все персональные данные из базы данных
            List<PersonalData> allPersonalData = databaseService.GetAllPersonalData();

            // Отфильтруйте данные, соответствующие текущему пользователю
            PersonalDataList = allPersonalData.Where(data => data.EmailUser == CurrentUserEmail).ToList();

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