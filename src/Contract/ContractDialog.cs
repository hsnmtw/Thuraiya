using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace Thuraiya
{	
	public class ContractDialog : RTLForm{
		
		private int id;
		private string action = "add";
		private ComboBox client;
		private DateTimePicker on_date;
		private TextBox hijri,occasion,price,paied,balance;
		private Label lclient,lon_date,loccasion,lprice,lpaied,lbalance,lpaySlips;
		private DataGridView paySlips;
		
		public ContractDialog() : base(){
			Text = ar("Contract Form");
			Size = new Size(420,400);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Initialize();
			ReloadClients();
			NewID();
		}
		
		public ContractDialog(string uid) : this(){
			id = int.Parse(uid);
			this.action = "update";
			DataTable dt = con.GetDataTable(@"select * from thrdb.contract where ID=@p0",id);
			client.SelectedValue = dt.Rows[0]["client"].ToString();
			price.Text = dt.Rows[0]["price"].ToString();
			
			on_date.Value = (DateTime)dt.Rows[0]["on_date"];
			occasion.Text = dt.Rows[0]["occasion"].ToString();
			ConvertDateAction(null,null);
			ReloadPaySlips();
			ReCalculateBalance();
		}
		
		private void ReCalculateBalance(){
			paied.Text = con.GetDataTable(@"select sum(x.paied) 
											  from (select a.paied 
													  from thrdb.payslip a 
													 where a.contract=@p0 
													 union 
													select 0) x;",id).Rows[0][0].ToString();
			
			balance.Text = (double.Parse(price.Text)-double.Parse(paied.Text)).ToString("0.00");			
		}
				
		private void NewID(){
			var query = @"select max(b.id)+1 from (select 0 as `id` union select max(a.id) from thrdb.contract a) b;";
			id = int.Parse(con.GetDataTable(query).Rows[0][0].ToString());
		}
		
		private void ReloadPaySlips(){
			var sql = @"select on_date 'On Date',id `ID`,paied `Paied` from thrdb.payslip where contract=@p0";
			var dt = con.GetDataTable(sql, id);
			foreach(DataColumn col in dt.Columns){
				col.ColumnName = ar(col.ColumnName);
			}
			paySlips.DataSource = dt;
		}

		private void PaySlipsAction(object s,EventArgs ea){
			new PaySlipDialog(id,client.Text).ShowDialog();
			ReCalculateBalance();
			ReloadPaySlips();
		}
		
		private void ConvertDateAction(object s,EventArgs ea){
			hijri.Text = DateConverter.toHijri(on_date.Value.ToString("yyyy-MM-dd"));
		}
		
		private void NewClientAction(object s,EventArgs ea){
			if(new ClientDialog().ShowDialog()==DialogResult.OK){
				ReloadClients();
			}
		}
		
		public void AddAction(object s,EventArgs ea){
			var sql = @"insert into thrdb.contract (id,client,on_date,price,occasion) values (@p0,@p1,@p2,@p3,@p4)";
			if("update".Equals(action)){
				sql = @"update thrdb.contract
						   set client  = @p1
							 , on_date = @p2
							 , price   = @p3
							 , occasion= @p4
						 where id = @p0";
			}
			var prm = new object[]{id,client.SelectedValue,on_date.Value.ToString(@"yyyy-MM-dd"),price.Text,occasion.Text};
			if(con.Execute(sql,prm)){
				DialogResult = DialogResult.OK;
				MessageBox.Show(ar(@"Your request has been successfully processed"),ar(@"Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Close();
			}else{
				MessageBox.Show(ar(@"Failed to process your request"),ar(@"Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		public void ReloadClients(){
			client.DataSource = con.GetDataTable(@"select mobile,name from client order by 2");
		}
		
		private void Initialize(){
			
			lclient  = new Label		 (){Location=new Point(5, 8),Width = 50,Text = ar("Client")};
			client   = new ComboBox		 (){Location=new Point(90,5),Width = 290};
			
			lon_date = new Label		 (){Location=new Point( 5,38),Width = 50,Text = ar("On Date")};
			on_date  = new DTPicker		 (){Location=new Point(90,35),Width = 200,Format = DateTimePickerFormat.Custom, CustomFormat = "dddd, dd MMM yyyy" };
			
			hijri    = new TextBox		 (){Location=new Point(300,35),Width = 80,ReadOnly = true };
			
			lprice   = new Label		 (){Location=new Point(90,63),Width = 60,Text = ar("Price")};
			price    = new NumericTextBox(){Location=new Point(90,80),Width = 60};
				
			lpaied   = new Label		 (){Location=new Point(200,63),Width = 60,Text = ar("Paied")};
			paied    = new NumericTextBox(){Location=new Point(200,80),Width = 60,ReadOnly = true};
			
			lbalance = new Label		 (){Location=new Point(310,63),Width = 60,Text = ar("Balance")};
			balance  = new NumericTextBox(){Location=new Point(310,80),Width = 60,ReadOnly = true};
			
			loccasion= new Label		 (){Location=new Point( 5,110),Width = 50,Text = ar("Occasion")};
			occasion = new TextBox		 (){Location=new Point(90,110),MaxLength=200,Size=new Size(290,50), Multiline = true, ScrollBars = ScrollBars.Vertical, AcceptsReturn = true, WordWrap = true };
			
			lpaySlips= new Label		 (){Location=new Point( 5,170),Text = ar("Pay Slips")};				
			paySlips = new CustomDataGridView	 (){Location=new Point(90,170), Size=new Size(290,140)};
				
			var no = new Button(){ Text = ar(@"Cancel")     , Location = new Point(290,10), Size=new Size(60,25) };
			var ok = new Button(){ Text = ar(@"Save")       , Location = new Point(200,10), Size=new Size(80,25) };
			var nc = new Button(){ Text = ar(@"New Client") , Location = new Point(115,10) , Size=new Size(80,25) };
			var ps = new Button(){ Text = ar(@"New Payment") , Location  = new Point(8,10) , Size=new Size(100,25) };
			
			var bPanel = new RTLPanel() {Dock = DockStyle.Bottom, Height = 35};
			var mPanel = new RTLPanel() {Dock = DockStyle.Fill};
			
			foreach(var con in new Button[]{nc,no,ok,ps}){
				bPanel.Controls.Add(con);
			}
			
			var cntrls = new object[]{
				client,lclient,
				on_date,lon_date,
				hijri,//lhijri,
				price,lprice,
				paied,lpaied,
				balance,lbalance,
				occasion,loccasion,
				paySlips,lpaySlips
			};
			
			foreach(var con in cntrls){
				mPanel.Controls.Add((Control)con);
			}
			
			CancelButton = no;
			AcceptButton = ok;
			
			DockPadding.All = 10;
			
			client.ValueMember = "mobile";
			client.DisplayMember = "name";
			client.BindingContext = this.BindingContext;
			client.DropDownStyle = ComboBoxStyle.DropDownList;
			
			on_date.ValueChanged+=new EventHandler(ConvertDateAction);
			
			ConvertDateAction(null,null);
			
			nc.Click+=new EventHandler(NewClientAction);
			ok.Click+=new EventHandler(AddAction);
			ps.Click+=new EventHandler(PaySlipsAction);
			
			Controls.Add(bPanel);
			Controls.Add(mPanel);			
		}		
	}
}
