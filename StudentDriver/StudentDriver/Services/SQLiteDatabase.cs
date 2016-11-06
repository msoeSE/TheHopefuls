using System;
using SQLite;
using StudentDriver.Models;
using Xamarin.Forms;

namespace StudentDriver
{
	public class SQLiteDatabase
	{
		static SQLiteAsyncConnection _database;
		public SQLiteDatabase ()
		{
			_database = DependencyService.Get<ISQLite> ().GetAsyncConnection ();
		    _database.CreateTableAsync<StateReqs>();
		    _database.CreateTableAsync<User>();
		    _database.CreateTableAsync<UserStats>();
		    _database.CreateTableAsync<UnsyncDrives>();
		    //TODO Create tables here
		}

	}
}
