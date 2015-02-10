using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Thuraiya{
    public class DateConverter {
        public static string toHijri(string greg){
			return DBConnection.GetInstance().GetDataTable("select hijri from dates where greg='"+ greg +"'").Rows[0][0].ToString();
		}
		public static string toGreg(string hijri){
			return DBConnection.GetInstance().GetDataTable("select greg from dates where hijri='"+ hijri +"'").Rows[0][0].ToString();
		}
    }
}