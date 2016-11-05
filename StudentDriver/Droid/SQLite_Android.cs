using System;
using System.IO;
using SQLite;

namespace StudentDriver.Droid
{
	public class SQLite_Android : ISQLite
	{

		private static string GetDatabasePath ()
		{
			string dbName = "StudentDriver.db3";

#if DEBUG
			var docPath = Path.Combine (Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
#else
			docPath = Envrionment.GetFolderPath(Environment.SpecialFolder.Personal);
#endif
			return Path.Combine (docPath, dbName);
		}

		public SQLiteAsyncConnection GetAsyncConnection ()
		{
			var conn = new SQLite.SQLiteAsyncConnection (GetDatabasePath ());
			return conn;
		}

		[Obsolete ("Use GetAsyncConnection instead")]
		public SQLiteConnection GetConnection ()
		{

			var conn = new SQLite.SQLiteConnection (GetDatabasePath ());
			return conn;
		}
	}
}
