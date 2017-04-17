using System;
using System.IO;
using Xamarin.Forms;
using RACompanion;
using SQLite.Net.Platform.XamarinAndroid;
using SQLite.Net.Async;
using StudentDriver;
using SQLiteConnectionString = SQLite.Net.SQLiteConnectionString;
using SQLiteConnectionWithLock = SQLite.Net.SQLiteConnectionWithLock;

[assembly: Dependency(typeof(SQLite_Android))]

namespace RACompanion
{

    public class SQLite_Android : ISQLite
    {
        private SQLiteConnectionWithLock _conn;
        public SQLite_Android()
        {
        }

        private static string GetDatabasePath()
        {
            var fileName = "studentdriver.db3";
#if DEBUG
            //var path = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            //var documentsPath = Path.Combine(path, Android.OS.Environment.DirectoryDownloads);
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#else
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#endif

            return Path.Combine(documentsPath, fileName);

        }
        [Obsolete("use GetAsyncConnection instead")]
        public SQLite.Net.SQLiteConnection GetConnection()
        {
            var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            var connection = new SQLite.Net.SQLiteConnection(platform, GetDatabasePath());

            return connection;
        }
        public void DeleteDatabase()
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

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var dbPath = GetDatabasePath();
            var platform = new SQLitePlatformAndroid();
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