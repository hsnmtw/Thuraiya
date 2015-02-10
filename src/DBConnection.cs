using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Thuraiya{
	public class DBConnection{

		private static readonly string CS = @"server=localhost;userid=root;password=r00t;database=thrdb";

		private static DBConnection instance = null;

		private MySqlConnection conn;
		private DBConnection(){
			conn = new MySqlConnection (CS);
			conn.Open ();
			Console.WriteLine("MySQL version : {0}", conn.ServerVersion);
		}

		public void Close(){
			conn.Close();
		}

		private static int _counter = 0;
		public static DBConnection GetInstance(){
			if(instance==null && _counter++==0) instance = new DBConnection();
			return instance;
		}

		public DataTable GetDataTable(string sql){
			DataTable dt = new DataTable ();
			MySqlCommand cmd = new MySqlCommand (sql, conn);
				using (MySqlDataAdapter a = new MySqlDataAdapter (cmd)) {
					a.Fill (dt);
				}

			return dt;
		}

		public void Execute(string sql){
			MySqlCommand cmd = new MySqlCommand (sql, conn);
			cmd.ExecuteNonQuery ();

		}
	}
}