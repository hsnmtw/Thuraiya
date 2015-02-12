using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace Thuraiya
{	
	public class NationalityDialog : RTLForm{
		
		private int id;
		private string action = "add";
		private TextBox country;
		private Label lcountry;
		
		public NationalityDialog() : base(){
			Text = ar("Nationality Form");
			Size = new Size(420,400);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Initialize();
			NewID();
		}
		
		public NationalityDialog(string uid) : this(){
			id = int.Parse(uid);
			this.action = "update";
			DataTable dt = con.GetDataTable(@"select country from thrdb.nationality where ID=@p0",id);
			country.Text = dt.Rows[0]["country"].ToString();
		}
		
				
		private void NewID(){
			var query = @"select max(b.id)+1 from (select 0 as `id` union select max(a.id) from thrdb.nationality a) b;";
			id = int.Parse(con.GetDataTable(query).Rows[0][0].ToString());
		}
		
		
		public void AddAction(object s,EventArgs ea){
			var sql = @"insert into thrdb.nationality (id,country) values (@p0,@p1)";
			if("update".Equals(action)){
				sql = @"update thrdb.nationality
						   set country = @p1
						 where id = @p0";
			}
			var prm = new object[]{id,country.Text};
			if(con.Execute(sql,prm)){
				DialogResult = DialogResult.OK;
				MessageBox.Show(ar(@"Your request has been successfully processed"),ar(@"Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Close();
			}else{
				MessageBox.Show(ar(@"Failed to process your request"),ar(@"Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		private void Initialize(){
			
			lcountry  = new Label		 (){Location=new Point(5, 8),Width = 50,Text = ar("Country")};
			country   = new TextBox		 (){Location=new Point(90,5),Width = 290};
			
				
			var no = new Button(){ Text = ar(@"Cancel")     , Location = new Point(290,10), Size=new Size(60,25) };
			var ok = new Button(){ Text = ar(@"Save")       , Location = new Point(200,10), Size=new Size(80,25) };
			
			var bPanel = new RTLPanel() {Dock = DockStyle.Bottom, Height = 35};
			var mPanel = new RTLPanel() {Dock = DockStyle.Fill};
			
			foreach(var con in new Button[]{no,ok}){
				bPanel.Controls.Add(con);
			}
			
			var cntrls = new object[]{
				country,lcountry
			};
			
			foreach(var con in cntrls){
				mPanel.Controls.Add((Control)con);
			}
			
			CancelButton = no;
			AcceptButton = ok;
			
			DockPadding.All = 10;
			
			ok.Click+=new EventHandler(AddAction);
			
			Controls.Add(bPanel);
			Controls.Add(mPanel);			
		}		
	}
}
