using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Thuraiya
{
	public class Calendar : RTLForm{
		
		public Calendar() : base(){
			Text = ar("Calendar");
			WindowState = FormWindowState.Maximized;
			TableLayoutPanel p = new TableLayoutPanel(){
				RowCount = 3,
				ColumnCount = 4,
				Dock = DockStyle.Fill,
				Size = new Size(218*4,182*3)
			};
			for(int i=1;i<13;i++){
				p.Controls.Add(new MonthView(2015,i));
			}
			DockPadding.All = 10;
			Controls.Add(p);
			Controls.Add(new Label(){
				Font = new Font("Arial", 20),
				TextAlign = ContentAlignment.MiddleCenter,
				Text = "2015",
				Height = 50,
				Dock = DockStyle.Top,
				AutoSize = false
			});
		}
		
	}
	
	public class MonthView : RTLPanel{
		public MonthView(int year,int month) : base(){
			var days = Config.of["Days"].Split(',');
			var dg = new CustomDataGridView();
			
			dg.SelectionMode = DataGridViewSelectionMode.CellSelect;
			dg.DefaultCellStyle.SelectionBackColor = Color.White;
			dg.DefaultCellStyle.SelectionForeColor = Color.Black;
			
			dg.ColumnCount = 7;
			dg.RowCount = 6;
			
			var p0 = new DateTime(year,month,1);
			var p1 = p0.AddMonths(1).AddDays(-1);
			var dt = DBConnection.GetInstance().GetDataTable("select on_date from v_calendar where on_date between @p0 and @p1",p0,p1);
			
			HashSet<int> hset = new HashSet<int>();
			for(int i=0;i<dt.Rows.Count;i++){
				hset.Add( ((DateTime)dt.Rows[i][0]).Day );
			}
			
			
			for(int i=0;i<days.Length;i++) dg.Columns[i].Name = days[i];
			
			string firstDay = string.Format("{0:ddd}", new DateTime(year, month, 1)).Substring(0, 2).ToUpper();
			int lastDayOfMonth = new DateTime(year, month, 1).AddMonths(1).AddDays(-1).Day;
			int d = 1;
			object o="";
			for (int i = 0; i < 31; i++)
			{
				if ((d == 1 && !firstDay.Equals(days[i % 7])) || d > lastDayOfMonth) o = ""; else o = d++;
				var cell = dg.Rows[i/7].Cells[i%7];
				cell.Value = o.ToString();
				if(hset.Contains(d-1)){
					cell.Style.BackColor = Color.LightPink;
					cell.Style.ForeColor = Color.Red;
					cell.Style.Font = new Font("Arial",11,FontStyle.Bold);
				}
			}
			
			dg.CellClick +=new DataGridViewCellEventHandler(delegate(object s,DataGridViewCellEventArgs e){
				var cell = (DataGridViewTextBoxCell)dg.Rows[e.RowIndex].Cells[e.ColumnIndex];
				dg.CurrentCell = dg.Rows[dg.Rows.Count-1].Cells[dg.Columns.Count-1];
				if(cell.Style.BackColor==Color.LightPink)
				{
					var par = new DateTime(year,month, int.Parse(cell.Value.ToString())).ToString("yyyy-MM-dd");
					var sql = "select id from contract where on_date='"+par+"'";
					LOG.info(sql);
					var dt0 = DBConnection.GetInstance().GetDataTable(sql,par);
					var id = dt0.Rows[0][0].ToString();
					LOG.info("id:{0}",id);
					new ContractDialog(id).Show();
				}
			});
			
			dg.Dock = DockStyle.Fill;
			Controls.Add(dg);
			Label lbl = new Label(){AutoSize=false};
			lbl.Text = new DateTime(year, month, 1).ToString("MMMM");
			lbl.Dock = DockStyle.Top;
			lbl.TextAlign = ContentAlignment.MiddleCenter;
			Controls.Add(lbl);
			Height = 180;
			dg.ScrollBars = ScrollBars.None;
		}
	}
}