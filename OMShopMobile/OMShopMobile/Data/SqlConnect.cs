using System;
using SQLite;
using System.IO;

namespace OMShopMobile
{
	public static class SqlConnect
	{
		public static object locker = new object ();

		public static SQLiteConnection database;

		static string DatabasePath {
			get { 
				var sqliteFilename = "TodoSQLite.db3";
				#if __IOS__
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
				var path = Path.Combine(libraryPath, sqliteFilename);
				#else
				#if __ANDROID__
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				var path = Path.Combine(documentsPath, sqliteFilename);
				#else
				// WinPhone
				var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);;
				#endif
				#endif
				return path;
			}
		}

		static SqlConnect()
		{
			database = new SQLiteConnection (DatabasePath);
			// create the tables
			database.CreateTable<Session>();
			database.CreateTable<BasketDB>();
		}
	}
}