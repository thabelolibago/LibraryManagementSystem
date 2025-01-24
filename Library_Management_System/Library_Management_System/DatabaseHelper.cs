using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace Library_Management_System
{
	public class DatabaseHelper
	{
		public static string GetConnectionString()
		{
			string connectionString = Environment.GetEnvironmentVariable("LibraryDB_ConnectionString");
			if (string.IsNullOrEmpty(connectionString))
			{
				connectionString = ConfigurationManager.ConnectionStrings["LibraryDB"].ConnectionString;
			}
			return connectionString;
		}
	}
}
