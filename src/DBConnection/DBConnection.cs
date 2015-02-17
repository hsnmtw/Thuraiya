using System;
using System.Data;

namespace Thuraiya{
	public abstract class DBConnection {
		public abstract void Close();
		public abstract DataTable GetDataTable(string sql,params object[]prm);
		public abstract bool Execute(string sql,params object[]prm);
		
		private static DBConnection instance = null;
		private static int _counter = 0;
		
		[STAThreadAttribute ()]
		public static DBConnection GetInstance(){
			if(instance==null && _counter++==0){
				instance = new SQLiteDBConnection();
			}
			return instance;
		}


		public static void Initialize(){
			initsql(@"..\\data\\database.sql");
			initsql(@"..\\data\\dates.sql");
			initsql(@"..\\data\\data.sql");
		}
		
		private static void initsql(string file){
			string[] sqls = System.IO.File.ReadAllText(file).Split(';');
			foreach(var statement in sqls) {
				var sql = statement.Replace('\n',' ').Replace('\r',' ').Trim();
				if("exit".Equals(sql)) break;
				//if(@"..\\data\\database.sql".Equals(file) || sql.Trim().EndsWith("-01')")){
					LOG.info(sql);
					instance.Execute(sql);
				//}
			}			
		}
	}
}