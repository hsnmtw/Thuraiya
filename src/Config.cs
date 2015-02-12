using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Thuraiya{
  
  public class Config : Dictionary<string, string>{

      public static Config of = null;

      private Config() : base() {
        try{
          
			using(var dt = DBConnection.GetInstance().GetDataTable(@"select prp,val from thrdb.config")){
				for(int i=0;i<dt.Rows.Count;i++){
					Add(dt.Rows[i][@"prp"].ToString() , dt.Rows[i][@"val"].ToString());
				}
			}
			
        }catch(Exception ex){
          MessageBox.Show(LOG.error(ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          Environment.Exit(0);
        }
      }
	  
	  public static void Initialize(){
		  if(of==null) of = new Config();
	  }
  }

}