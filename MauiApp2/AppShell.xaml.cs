namespace MauiApp2;

public partial class AppShell : Shell
{
    private DatabaseServiceUser _databaseService;
    public string CurrentUserEmail { get; set; }
    public string CurrentUserPassword { get; set; }
    public string CurrentUserPinCode { get; set; }
    public AppShell()
    {
        InitializeComponent();
    }

}
