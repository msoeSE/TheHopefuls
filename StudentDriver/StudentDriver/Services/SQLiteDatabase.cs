using System;
using SQLite;
using Xamarin.Forms;

namespace StudentDriver
{
	public class SQLiteDatabase
	{
		static SQLiteAsyncConnection _database;
		public SQLiteDatabase ()
		{
			_database = DependencyService.Get<ISQLite> ().GetAsyncConnection ();
			//TODO Create tables here
		}

	}
}
