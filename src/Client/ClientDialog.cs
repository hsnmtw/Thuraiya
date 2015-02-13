using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace Thuraiya
{	
	public class ClientDialog : RTLForm{
		
		private string action = "add";
		private ComboBox nationality;
		private DateTimePicker govid_doi;
		private TextBox hijri,mobile,name,govid,govid_poi,address,tel1,tel2;
		private Label lgovid_doi,lmobile,lname,lgovid,lgovid_poi,laddress,ltel1,ltel2,lcontracts,lnationality;
		private DataGridView contracts;
		
		public ClientDialog() : base(){
			Text = ar("Client Form");
			Size = new Size(500,400);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Initialize();
			ReloadNationalities();
		}
		
		public ClientDialog(string uid) : this(){
			
			this.action = "update";
			DataTable dt = con.GetDataTable(@"select mobile,name,nationality,govid,govid_doi,govid_poi,address,tel1,tel2 
											    from thrdb.client where mobile=@p0",uid);
			var row = dt.Rows[0];
			nationality.SelectedValue = (int)row["nationality"];
			
			mobile.Text = row["mobile"].ToString();
			tel1.Text = row["tel1"].ToString();
			tel2.Text = row["tel2"].ToString();
			name.Text = row["name"].ToString();
			address.Text = row["address"].ToString();
			govid_poi.Text = row["govid_poi"].ToString();
			//LOG.info("{0}",row["govid_doi"]);
			govid_doi.Value = "".Equals(row["govid_doi"]) || row["govid_doi"]==DBNull.Value?DateTime.Now:(DateTime)row["govid_doi"];
			govid.Text = row["govid"].ToString();
			
			ConvertDateAction(null,null);
			ReloadContracts();
			mobile.ReadOnly = true;
		}
		
		private void ReloadContracts(){
			var sql = @"select a.`ID`,a.`On Date`,a.`Price`,a.`Paied`,a.`Balance` 
						  from thrdb.v_contracts a 
			              join thrdb.contract    b
			                on a.ID=b.id 
			             where b.client=(@p0)";
			var dt = con.GetDataTable(sql, mobile.Text);
			foreach(DataColumn col in dt.Columns){
				col.ColumnName = ar(col.ColumnName);
			}
			contracts.DataSource = dt;
		}

		
		private void ConvertDateAction(object s,EventArgs ea){
			hijri.Text = DateConverter.toHijri(govid_doi.Value.ToString("yyyy-MM-dd"));
		}
		
		
		public void AddAction(object s,EventArgs ea){
			var sql = @"insert into thrdb.client (mobile,name,nationality,govid,govid_poi,govid_doi,address,tel1,tel2) 
										  values (@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)";
			if("update".Equals(action)){
				sql = @"update thrdb.client
						   set name		   = @p1
							 , nationality = @p2
				             , govid	   = @p3
				             , govid_poi   = @p4
				             , govid_doi   = @p5
							 , address	   = @p6
				             , tel1		   = @p7
				             , tel2		   = @p8
						 where mobile      = @p0";
			}
			var prm = new object[]{mobile.Text,name.Text,nationality.SelectedValue,govid.Text,govid_poi.Text,govid_doi.Value.ToString("yyyy-MM-dd"),address.Text,tel1.Text,tel2.Text};
			if(con.Execute(sql,prm)){
				DialogResult = DialogResult.OK;
				MessageBox.Show(ar(@"Your request has been successfully processed"),ar(@"Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Close();
			}else{
				MessageBox.Show(ar(@"Failed to process your request"),ar(@"Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		public void ReloadNationalities(){
			nationality.DataSource = con.GetDataTable(@"select id,country from nationality order by 2 asc");
		}
		
		private void Initialize(){
			//mobile,name,nationality,govid,govid_poi,govid_doi,address,tel1,tel2
			
			lmobile  = new Label		 (){Text = ar("Mobile")};
			mobile   = new TextBox		 (){};
			
			lname = new Label		 	 (){Text = ar("Name")};
			name  = new TextBox			 (){};
			
			lnationality   = new Label	 (){Text = ar("Nationality")};
			nationality    = new ComboBox(){};
				
			lgovid   = new Label		 (){Text = ar("NID")};
			govid    = new TextBox		 (){};
			
			lgovid_poi = new Label		 (){Text = ar("NID POI")};
			govid_poi  = new TextBox	 (){};
			
			lgovid_doi = new Label		 (){Text = ar("NID DOI")};
			govid_doi  = new DTPicker	 (){CustomFormat = "dd/MM/yyyy"};
			hijri    = new TextBox		 (){ReadOnly=true};

			laddress = new Label		 (){Text = ar("Address")};
			address  = new TextBox	 	 (){};

			ltel1 = new Label		     (){Text = ar("Tel 1")};
			tel1  = new TextBox	 	     (){};

			ltel2 = new Label		     (){Text = ar("Tel 2")};
			tel2  = new TextBox	 	     (){};
			

			
			lcontracts= new Label		 (){Text = ar("Contracts")};				
			contracts = new CustomDataGridView	 (){Location=new Point(90,170), Size=new Size(290,140)};
				
			var no = new Button(){ Text = ar(@"Cancel")     , Location = new Point(290,10), Size=new Size(60,25) };
			var ok = new Button(){ Text = ar(@"Save")       , Location = new Point(200,10), Size=new Size(80,25) };
			
			var bPanel = new RTLPanel() {Dock = DockStyle.Bottom, Height = 35};
			var mPanel = new RTLPanel() {Dock = DockStyle.Fill};
			
			foreach(var con in new Button[]{no,ok}){
				bPanel.Controls.Add(con);
			}
			
			object[] cntrls = {
				lname,name,
				lmobile,mobile,lnationality,nationality,
				lgovid,govid,lgovid_poi,govid_poi,
				lgovid_doi,govid_doi,hijri,
				ltel1,tel1,ltel2,tel2,
				laddress,address,
				lcontracts,contracts
			};
			
			
			int[,] bounds = {
				{ 10, 10+3, 70, 20},{ 90, 10,350, 20},
				{ 10, 40+3, 70, 20},{ 90, 40,110, 20},{210, 40+3, 70, 20},{290, 40,150, 20},
				{ 10, 70+3, 70, 20},{ 90, 70,110, 20},{210, 70+3, 70, 20},{290, 70,150, 20},
				{ 10,100+3, 70, 20},{ 90,100,110, 20},{210,100+0, 70, 20},
				{ 10,130+3, 70, 20},{ 90,130,110, 20},{210,130+3, 70, 20},{290,130,150, 20},
				{ 10,160+3, 70, 20},{ 90,160,350, 20},
				{ 10,190+3, 70, 20},{ 90,190,350,120},
			};
			
						//cntrls.Length
			for(int c=0;c<cntrls.Length;c++){
				var con = (Control)cntrls[c];
				mPanel.Controls.Add(con);
				con.SetBounds(bounds[c,0],bounds[c,1],bounds[c,2],bounds[c,3]);
			}
			
			
			
			CancelButton = no;
			AcceptButton = ok;
			
			DockPadding.All = 10;
			
			nationality.ValueMember = "id";
			nationality.DisplayMember = "country";
			nationality.BindingContext = this.BindingContext;
			nationality.DropDownStyle = ComboBoxStyle.DropDownList;
			
			govid_doi.ValueChanged+=new EventHandler(ConvertDateAction);
			
			ConvertDateAction(null,null);
			
			ok.Click+=new EventHandler(AddAction);
			
			Controls.Add(bPanel);
			Controls.Add(mPanel);
			
			
			
			
		}		
	}
}