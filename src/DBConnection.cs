using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Thuraiya{
	public class DBConnection{

		private static readonly string CS = @"server=localhost;userid=root;password=r00t;database=thrdb";

		private static DBConnection instance = null;

		private MySqlConnection conn;
		private DBConnection(){
			try{
				conn = new MySqlConnection (CS);
				conn.Open ();
				LOG.info("MySQL version : {0}", conn.ServerVersion);
			}catch(Exception ex){
				LOG.error(ex.Message);
				Environment.Exit(-1);
			}finally{
				
			}
		}

		public void Close(){
			if(conn != null && conn.State == ConnectionState.Closed) conn.Close();
		}

		private static int _counter = 0;
		public static DBConnection GetInstance(){
			if(instance==null && _counter++==0) instance = new DBConnection();
			return instance;
		}
		
		public DataTable GetDataTable(string sql,params object[]prm){
			//LOG.warn(sql);
			var dt = new DataTable ();
			try{
				using (MySqlDataAdapter a = new MySqlDataAdapter (BuildCommand(sql,prm))) {
					a.Fill (dt);
				}
			}catch(Exception ex){
				LOG.error(ex.Message);
			}
			return dt;
		}
		
		private MySqlCommand BuildCommand(string sql,params object[]prm){
			var cmd = new MySqlCommand (sql, conn);
			for(int i=0;i<prm.Length;i++){
				cmd.Parameters.AddWithValue(string.Format(@"@p{0}",i),prm[i]);
			}
			if(prm.Length>0)cmd.Prepare();
			return cmd;
		}
		
		public bool Execute(string sql,params object[]prm){
			try{
				return BuildCommand(sql,prm).ExecuteNonQuery () > 0;
			}catch(Exception ex){
				LOG.error(ex.Message);
			}
			return false;
		}
	}
}