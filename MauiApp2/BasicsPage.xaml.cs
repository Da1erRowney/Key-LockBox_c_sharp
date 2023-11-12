using PersonalsData;
using System.ComponentModel;
using System.Text.RegularExpressions;



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
        private async void InitializePersonalDataList()
        {
            string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "personalData.db");
            DatabaseServicePersonalData databaseService = new DatabaseServicePersonalData(databasePath);


            List<PersonalData> allPersonalData = databaseService.GetAllPersonalData();

            foreach (var data in allPersonalData)
            {
                string nameicon = data.Name;
                nameicon = nameicon.ToLower();
                nameicon = Regex.Replace(nameicon, @"[\p{P}-[.]]+", "");
                nameicon = Regex.Replace(nameicon, " ", "");
                string nameicontranc = string.Empty;
                // Replace "кс" with "x"
                if (nameicon == "майкрософт")
                {
                    nameicon = "microsoft";
                }

                if (nameicon == "хром")
                {
                    nameicon = "chrome";
                }

                if (nameicon == "tictok")
                {
                    nameicon = "tiktok";
                }

                if (nameicon == "стим")
                {
                    nameicon = "steam";
                }

                if (nameicon == "юбисофт")
                {
                    nameicon = "ubisoft";
                }

                if (nameicon == "вк" || nameicon == "вконтакте")
                {
                    nameicon = "vk";
                }

                if (nameicon == "ютуб")
                {
                    nameicon = "youtube";
                }

                if (nameicon == "али" || nameicon == "алиэкспрэс" || nameicon == "алиекспрес")
                {
                    nameicon = "aliexpress";
                }


                nameicon = Regex.Replace(nameicon, @"кс", "x");

                // Replace "ай" in the middle with "i"
                nameicon = Regex.Replace(nameicon, @"(?<=\p{IsCyrillic})ай(?=\p{IsCyrillic})", "i");

                // Replace "ай" at the end with "y"
                nameicon = Regex.Replace(nameicon, @"(?<=\p{IsCyrillic})ай$", "y");
                nameicon = Regex.Replace(nameicon, @"(?<=\p{IsCyrillic})эй$", "ay");
                nameicon = Regex.Replace(nameicon, @"(?<=\p{IsCyrillic})ей$", "ay");

                foreach (char c in nameicon)
                {
                    nameicontranc += TransliterateCharacter(c);
                }

                string TransliterateCharacter(char c)
                {
                    switch (c)
                    {
                        case 'а': return "a";
                        case 'б': return "b";
                        case 'в': return "v";
                        case 'г': return "g";
                        case 'д': return "d";
                        case 'е': return "e";
                        case 'ё': return "yo";
                        case 'ж': return "zh";
                        case 'з': return "z";
                        case 'и': return "i";
                        case 'й': return "y";
                        case 'к': return "k";
                        case 'л': return "l";
                        case 'м': return "m";
                        case 'н': return "n";
                        case 'о': return "o";
                        case 'п': return "p";
                        case 'р': return "r";
                        case 'с': return "s";
                        case 'т': return "t";
                        case 'у': return "u";
                        case 'ф': return "f";
                        case 'х': return "kh";
                        case 'ц': return "ts";
                        case 'ч': return "ch";
                        case 'ш': return "sh";
                        case 'щ': return "sch";
                        case 'ъ': return "'";
                        case 'ы': return "y";
                        case 'ь': return "'";
                        case 'э': return "e";
                        case 'ю': return "yu";
                        case 'я': return "ya";
                        default: return c.ToString();
                    }
                }


                var icon = new string[]
                {
                    "dota2",
                    "google",
                    "googleplay",
                    "instagram",
                    "microsoft",
                    "nvidia",
                    "odnoklassniki",
                    "opera",
                    "pinterest",
                    "spotify",
                    "steam",
                    "telegram",
                    "tiktok",
                    "viber",
                    "visa",
                    "vk",
                    "whatsapp",
                    "youtube",
                    "discord",
                    "twitter",
                    "pornhub",
                    "mobilelegends",
                    "genshinimpact",
                    "github",
                    "drweb",
                    "delphi",
                    "chrome",
                    "blender",
                    "css",
                    "javascript",
                    "ea",
                    "onedrive",
                    "edge",
                    "roblox",
                    "totalcommander",
                    "shazam",
                    "appleappstore",
                    "snapchat",
                    "facebook",
                    "kfc",
                    "lamoda",
                    "oz",
                    "onliner",
                    "kufar",
                    "burgerking",
                    "epicgames",
                    "faceit",
                    "blizzard",
                    "soundcloud",
                    "ubisoft",
                    "rockstargames",
                    "aliexpress",
                    "yandex",
                    "oplati"
                };

                if (icon.Contains(nameicontranc))
                {
                    data.IconUrl = $"{nameicontranc}.png";
                }
                else
                {


                    data.IconUrl = "noticon.png";
                }
            }

            if (Setting.statusSort == "По названию")
            {
                PersonalDataList = allPersonalData
                    .Where(data => data.EmailUser == CurrentUserEmail)
                    .OrderBy(data => data.Name)
                    .ToList();
            }
            else if (Setting.statusSort == "По логину")
            {
                PersonalDataList = allPersonalData
                    .Where(data => data.EmailUser == CurrentUserEmail)
                    .OrderBy(data => data.Login)
                    .ToList();
            }
            else if (Setting.statusSort == "По дате создания")
            {
                PersonalDataList = allPersonalData
                    .Where(data => data.EmailUser == CurrentUserEmail)
                    .OrderBy(data => data.DateCreation)
                    .ToList();
            }
            else
            {
                PersonalDataList = allPersonalData
                    .Where(data => data.EmailUser == CurrentUserEmail)
                    .OrderBy(data => data.Name)
                    .ToList();
            }
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


            PersonalDataListView.SelectedItem = null;
        }

        private void OnSettingsClicked(object sender, TappedEventArgs e)
        {

        }
    }


}