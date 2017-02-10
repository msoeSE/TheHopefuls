using System;
using SQLite.Net;
using SQLite.Net.Async;

namespace StudentDriver
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection ();
		SQLiteAsyncConnection GetAsyncConnection ();
	    void DeleteDatabase();
    }
}
