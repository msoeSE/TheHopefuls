using System;
using System.IO;
using System.Runtime.CompilerServices;
using RACompanion;
using Xamarin.Forms;
using SQLite.Net.Async;
using SQLite.Net.Platform.XamarinIOS;
using StudentDriver;
using SQLiteConnectionString = SQLite.Net.SQLiteConnectionString;
using SQLiteConnectionWithLock = SQLite.Net.SQLiteConnectionWithLock;


[assembly: Xamarin.Forms.Dependency(typeof(SQLite_iOS))]

namespace RACompanion
{
    public class SQLite_iOS : ISQLite
    {
        private SQLiteConnectionWithLock _conn;

        private static string GetDatabasePath()
        {

            var fileName = "StudentDriver.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //You need to place things inside of the library path on iOS.
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            return Path.Combine(libraryPath, fileName);

        }
        [Obsolete("use GetAsyncConnection instead")]
        public SQLite.Net.SQLiteConnection GetConnection()
        {

            var platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
            var connection = new SQLite.Net.SQLiteConnection(platform, GetDatabasePath());

            return connection;
        }

        public void DeleteDatabase()
        {
            try
            {
                var path = GetDatabasePath();

                try
                {
                    if (_conn != null)
                    {
                        _conn.Close();

                    }
                }
                catch { }

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                _conn = null;

            }
            catch
            {
                throw;
            }
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var dbPath = GetDatabasePath();
            var platform = new SQLitePlatformIOS();
            var connectionFactory = new Func<SQLiteConnectionWithLock>(
                () => {
                    if (_conn == null)
                    {
                        _conn = new SQLiteConnectionWithLock(platform, new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: true));
                    }
                    return _conn;
                });
            return new SQLiteAsyncConnection(connectionFactory);
        }

    }
}