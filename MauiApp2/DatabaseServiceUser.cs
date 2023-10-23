using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.Communication;
using SQLite;


public class DatabaseServiceUser
{
    private SQLiteConnection _connection;

    public DatabaseServiceUser(string databasePath)
    {
        _connection = new SQLiteConnection(databasePath);
        // Дополнительные настройки базы данных, если необходимо
    }

    public void CreateTables()
    {
        _connection.CreateTable<User>();
        // Создание других таблиц, если необходимо
    }

    public void InsertUser(User user)
    {
        _connection.Insert(user);
    }

    public User GetUserByEmail(string email)
    {
        return _connection.Table<User>().FirstOrDefault(u => u.Email == email);
    }

    public void UpdateUser(User user)
    {
        _connection.Update(user);
    }

    // Другие методы для работы с базой данных

    
    }

public class User
{
    [PrimaryKey]
    public string Email { get; set; }
    public string Password { get; set; }
}
