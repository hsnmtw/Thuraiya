using System;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Thuraiya
{

    public class Dates : RTLForm
    {
        public Dates() : base() {
            Text = ar("Dates");
            Size = new Size(400,180);
            Initialize();
        }

        
        private void Initialize()
        {
            var cgv = new CustomDataGridView(){
				DataSource = con.GetDataTable(@"select hijri as `Hijri`,greg as `Gregorian` from dates"),
				Dock = DockStyle.Fill
			};
			var dt = (DataTable)cgv.DataSource;
			foreach(DataColumn col in dt.Columns){
				col.ColumnName = ar(col.ColumnName);
			}
	
			Controls.Add(cgv);
        }
    }
}