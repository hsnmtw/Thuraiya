using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace Thuraiya
{	
	public class PaySlipDialog : RTLForm{
		
		private int id;
		private string action = "add";
		private DateTimePicker on_date;
		private TextBox pid,hijri,paied,notes,client,contract;
		private Label lpid,lclient,lon_date,lcontract,lpaied,lnotes;
		
		public PaySlipDialog() : base(){
			Text = ar("Contract Form");
			Size = new Size(500,400);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Initialize();
			NewID();
		}
		
		public PaySlipDialog(string uid) : this(){
			id = int.Parse(uid);
			this.action = "update";
			DataTable dt = con.GetDataTable(@"select * from v_payslips where ID=@p0",id);
			pid.Text = uid;
			client.Text = dt.Rows[0]["Client"].ToString();
			contract.Text = dt.Rows[0]["Contract"].ToString();
			paied.Text = dt.Rows[0]["Paied"].ToString();
			on_date.Value = (DateTime)dt.Rows[0]["On Date"];
			notes.Text = dt.Rows[0]["Notes"].ToString();
			
			ConvertDateAction(null,null);
		}
		
		public PaySlipDialog(int contractID, string clientName) : this(){
			this.action = "add";
			NewID();
			pid.Text = id.ToString();
			client.Text = clientName;
			contract.Text = contractID.ToString();
			paied.Text = "";
			on_date.Value = DateTime.Now;
			notes.Text = "";
			
			ConvertDateAction(null,null);
		}
	
		private void NewID(){
			var query = @"select max(b.id)+1 from (select 0 as `id` union select max(a.id) from payslip a) b;";
			id = int.Parse(con.GetDataTable(query).Rows[0][0].ToString());
		}
		
		private void ConvertDateAction(object s,EventArgs ea){
			hijri.Text = DateConverter.toHijri(on_date.Value.ToString("yyyy-MM-dd"));
		}
		
		public void AddAction(object s,EventArgs ea){
			var sql = @"insert into payslip (id,contract,on_date,paied,notes) values (@p0,@p1,@p2,@p3,@p4)";
			if("update".Equals(action)){
				sql = @"update payslip
						   set contract = @p1
							 , on_date  = @p2
							 , paied    = @p3
							 , notes    = @p4
						 where id = @p0";
			}
			var prm = new object[]{id,contract.Text,on_date.Value.ToString(@"yyyy-MM-dd"),paied.Text,notes.Text};
			if(con.Execute(sql,prm)){
				DialogResult = DialogResult.OK;
				MessageBox.Show(ar(@"Your request has been successfully processed"),ar(@"Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Close();
			}else{
				MessageBox.Show(ar(@"Failed to process your request"),ar(@"Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		
		private void Initialize(){
			
			lpid  = new Label		 (){Text = ar("ID")};
			pid   = new TextBox		 (){Width = 290,ReadOnly=true};
			
			lclient  = new Label		 (){Text = ar("Client")};
			client   = new TextBox		 (){Width = 290,ReadOnly=true};
			
			lcontract  = new Label		 (){Text = ar("Contract")};
			contract   = new TextBox		 (){Width = 290,ReadOnly=true};
			
			lon_date = new Label		 (){Text = ar("On Date")};
			on_date  = new DTPicker		 (){Value=DateTime.Now,Format = DateTimePickerFormat.Custom, CustomFormat = "dddd, dd MMM yyyy" };
			
			hijri    = new TextBox		 (){ReadOnly = true };
			
			lpaied   = new Label		 	(){Text = ar("Paied")};
			paied    = new NumericTextBox(){};
							
			lnotes	 = new Label		 (){Text = ar("Notes")};
			notes 	 = new TextBox		 (){Multiline=true,Size=new Size(290,150), AcceptsReturn = true, WordWrap = true,MaxLength=200, ScrollBars = ScrollBars.Vertical };
				
			var no = new Button(){ Text = ar(@"Cancel")     , Location = new Point(290,10), Size=new Size(60,25) };
			var ok = new Button(){ Text = ar(@"Save")       , Location = new Point(200,10), Size=new Size(80,25) };
			
			var bPanel = new RTLPanel() {Dock = DockStyle.Bottom, Height = 35};
			var mPanel = new TableLayoutPanel() {Dock = DockStyle.Fill, ColumnCount=2};
			
			foreach(var con in new Button[]{no,ok}){
				bPanel.Controls.Add(con);
			}
			
			var pondate = new FlowLayoutPanel(){WrapContents=false, Dock = DockStyle.Top,Height=27};
			pondate.DockPadding.All=0;
			pondate.Controls.Add(on_date);
			pondate.Controls.Add(hijri);
			
			var cntrls = new object[]{
				lpid,pid,
				lclient,client,
				lcontract,contract,
				lon_date,pondate,
				lpaied,paied,
				lnotes,notes
			};
			
			foreach(var con in cntrls){
				mPanel.Controls.Add((Control)con);
			}
			
			CancelButton = no;
			AcceptButton = ok;
			
			DockPadding.All = 10;
						
			on_date.ValueChanged+=new EventHandler(ConvertDateAction);
			
			ConvertDateAction(null,null);
			
			ok.Click+=new EventHandler(AddAction);
			
			Controls.Add(bPanel);
			Controls.Add(mPanel);			
		}		
	}
}
