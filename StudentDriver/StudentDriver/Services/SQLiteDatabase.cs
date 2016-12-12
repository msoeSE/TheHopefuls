using System;
using SQLite;
using StudentDriver.Models;
using Xamarin.Forms;

namespace StudentDriver
{
	public class SQLiteDatabase
	{
		static SQLiteAsyncConnection _database;
		private SQLiteDatabase _sqLiteDatabaseInstance;

		public SQLiteDatabase GetInstance ()
		{
			return _sqLiteDatabaseInstance ?? (_sqLiteDatabaseInstance = new SQLiteDatabase ());
		}

		private SQLiteDatabase ()
		{
			_database = DependencyService.Get<ISQLite> ().GetAsyncConnection ();
			_database.CreateTableAsync<StateReqs> ();
			_database.CreateTableAsync<User> ();
			_database.CreateTableAsync<UserStats> ();
			_database.CreateTableAsync<DrivePoints> ();
			_database.CreateTableAsync<UnsyncDrives> ();
		}

	}
}
