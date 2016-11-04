using System;
using System.IO;
using SQLite;

namespace StudentDriver.Droid
{
	public class SQLite_Android : ISQLite
	{
		private string dbName = "StudentDriver.db3";

		public SQLiteAsyncConnection GetAsyncConnection ()
		{
			string docPath = System.Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var path = Path.Combine (docPath, dbName);
			var conn = new SQLite.SQLiteAsyncConnection (path);
			return conn;
		}

		[Obsolete ("Use GetAsyncConnection instead")]
		public SQLiteConnection GetConnection ()
		{

			string docPath = System.Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var path = Path.Combine (docPath, dbName);
			var conn = new SQLite.SQLiteConnection (path);
			return conn;
		}
	}
}
