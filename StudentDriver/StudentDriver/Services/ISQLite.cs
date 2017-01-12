using System;
namespace StudentDriver
{
	public interface ISQLite
	{
		SQLite.SQLiteConnection GetConnection ();
		SQLite.SQLiteAsyncConnection GetAsyncConnection ();
	}
}
