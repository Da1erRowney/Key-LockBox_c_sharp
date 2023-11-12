
using SQLite;

namespace PersonalsData
{
    public class DatabaseServicePersonalData
    {
        private SQLiteConnection _connection;

        public DatabaseServicePersonalData(string databasePath)
        {
            _connection = new SQLiteConnection(databasePath);
            _connection.CreateTable<PersonalData>();
        }

        public void InsertPersonalData(PersonalData personalData)
        {
            _connection.Insert(personalData);
        }

        public PersonalData GetPersonalDataByEmail(string email)
        {
            return _connection.Table<PersonalData>().FirstOrDefault(u => u.EmailUser == email);
        }

        public void UpdatePersonalData(PersonalData personalData)
        {
            _connection.Update(personalData);
        }

        public void DeletePersonalData(PersonalData personalData)
        {
            _connection.Delete(personalData);
        }

        public List<PersonalData> GetAllPersonalData()
        {
            return _connection.Table<PersonalData>().ToList();
        }
        public void CloseConnection()
        {
            _connection?.Close();
        }
    }

    public class PersonalData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string EmailUser { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string OtherData { get; set; }
        public string DateCreation { get; set; }
        public string LastModifiedDate { get; set; }

        public string IconUrl { get; set; }

    }
}