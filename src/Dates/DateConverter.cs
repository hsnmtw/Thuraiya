using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Thuraiya{
    public class DateConverter {
        public static string toHijri(string greg){
			var rows = DBConnection.GetInstance().GetDataTable("select hijri from dates where greg=@p0",greg).Rows;
			return rows.Count > 0 ? rows[0][0].ToString() : null;
		}
		public static string toGreg(string hijri){
			var rows = DBConnection.GetInstance().GetDataTable("select greg from dates where hijri=@p0",hijri).Rows;
			return rows.Count > 0 ? rows[0][0].ToString() : null;
		}
    }
}