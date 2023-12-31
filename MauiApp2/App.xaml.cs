﻿using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using SQLite;

namespace MauiApp2;

public partial class App : Application
{
    public static string CurrentUserEmail { get; set; }
    public static string CurrentUserPassword { get; set; }
    public static string CurrentUserPinCode { get; set; }
    private readonly IFingerprint fingerprint;
    private DatabaseServiceUser _databaseService;
    public SQLiteConnection CreateDatabase(string databasePath)
    {
        SQLiteConnection connection = new SQLiteConnection(databasePath);
        connection.CreateTable<User>();
        return connection;
    }
    public App()
    {
        InitializeComponent();

        string databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db");
        SQLiteConnection connection = CreateDatabase(databasePath);
        _databaseService = new DatabaseServiceUser(databasePath);

        User user = _databaseService.GetUserByStatusAccount("On");
        if (user != null && user.PinCode != "NoN")
        {

            string userEmail = user.Email;
            string userPassword = user.Password;
            string userPinCode = user.PinCode;

            CurrentUserEmail = userEmail;
            CurrentUserPassword = userPassword;
            CurrentUserPinCode = userPinCode;
            if (user.ThemeApplication == "Dark")
            {
                Application.Current.UserAppTheme = AppTheme.Dark;
            }
            else if (user.ThemeApplication == "Light")
            {
                Application.Current.UserAppTheme = AppTheme.Light;
            }
            fingerprint = CrossFingerprint.Current;

            MainPage = new ConfirmationPinCode(fingerprint);
        }
        else
        {
            MainPage = new MainPage();
        }
    }
}