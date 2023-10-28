using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.Communication;
using SQLite;

namespace MauiApp2
{
    public class DatabaseServiceUser
    {
        private SQLiteConnection _connection;

        public DatabaseServiceUser(string databasePath)
        {
            _connection = new SQLiteConnection(databasePath);
        }

        public void CreateTables()
        {
            _connection.CreateTable<User>();
        }

        public void InsertUser(User user)
        {
            _connection.Insert(user);
        }

        public void CloseConnection()
        {
            _connection?.Close();
        }

        public User GetUserByEmail(string email)
        {
            return _connection.Table<User>().FirstOrDefault(u => u.Email == email);
        }

        public User GetUserByStatusAccount(string statusAccount)
        {
            return _connection.Table<User>().FirstOrDefault(u => u.StatusAccount == statusAccount);
        }

        public void UpdateUser(User user)
        {
            _connection.Update(user);
        }

    }

    public class User
    {
        [PrimaryKey]
        public string Email { get; set; }
        public string Password { get; set; }
        public string HintsBasics { get; set; }
        public string HintsSetting { get; set; }
        public string HintsData { get; set; }
        public string PinCode { get; set; }
        public string StatusAccount { get; set; }
    }
}