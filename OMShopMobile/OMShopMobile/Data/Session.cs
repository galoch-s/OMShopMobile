using System;

using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace OMShopMobile
{
	public class Session
	{
		[PrimaryKey]
		public int Id { get; set; }
		public int UserId { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string HashKey { get; set; }

		public static Session GetSession()
		{
			lock (SqlConnect.locker) {
				return SqlConnect.database.Table<Session> ().FirstOrDefault (x => x.Id == 1);
			}
		}

		public static int SaveUser (User user) 
		{
			Session session = new Session ();
			session.Id = 1;
			session.UserId = user.Id;
			session.Email = user.Email;
			session.Password = user.Password;
			session.HashKey = user.HashKey;

			lock (SqlConnect.locker) {
				if (Session.GetItem(session.Id) != null) {
					SqlConnect.database.Update(session);
					return session.Id;
				} else {
					return SqlConnect.database.Insert(session);
				}
			}
		}

		public static Session GetItem (int id) 
		{
			lock (SqlConnect.locker) {
				return SqlConnect.database.Table<Session>().FirstOrDefault(x => x.Id == id);
			}
		}

		public static IEnumerable<Session> GetItems ()
		{
			lock (SqlConnect.locker) {
				return (from i in SqlConnect.database.Table<Session>() select i).ToList();
			}
		}

		public static int DeleteItem(int id)
		{
			lock (SqlConnect.locker) {
				return SqlConnect.database.Delete<Session>(id);
			}
		}
	}
}