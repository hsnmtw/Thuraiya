using System;
using System.Data;
using System.Data.SQLite;

namespace Thuraiya{
	public class SQLiteDBConnection : DBConnection {
		
		private static readonly string CS = @"Data Source=db.sqllitep;";
		
		private SQLiteConnection conn;
		
		public SQLiteDBConnection(){
			try{
				conn = new SQLiteConnection (CS);
				conn.Open ();
				
				LOG.info("SQLite version : {0}", conn.ServerVersion);
			}catch(Exception ex){
				LOG.error(ex.Message);
				Environment.Exit(-1);
			}finally{
				
			}
		}
		


		public override void Close(){
			if(conn != null && conn.State == ConnectionState.Closed) conn.Close();
		}
		
		private SQLiteCommand BuildCommand(string sql,params object[]prm){
			var cmd = new SQLiteCommand(sql, conn);
			for(int i=0;i<prm.Length;i++){
				cmd.Parameters.AddWithValue(string.Format(@"@p{0}",i),prm[i]);
			}
			if(prm.Length>0) {cmd.Prepare();}
			return cmd;
		}
		
		public  override DataTable GetDataTable(string sql,params object[]prm){
			var dt = new DataTable ();
			try{
				using (SQLiteDataAdapter a = new SQLiteDataAdapter (BuildCommand(sql,prm))) {
					a.Fill (dt);
				}
			}catch(Exception ex){
				LOG.error(sql);
				LOG.error(ex.Message);
				Environment.Exit(-1);
			}
			return dt;
		}
		
		public override bool Execute(string sql,params object[]prm){
			try{
				return BuildCommand(sql,prm).ExecuteNonQuery () > 0;
			}catch(Exception ex){
				LOG.error(sql);
				LOG.error(ex.Message);
				Environment.Exit(-1);
			}
			return false;
		}
	}
}