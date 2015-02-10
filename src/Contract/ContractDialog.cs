using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Thuraiya
{	
	public class ContractDialog : RTLForm{
		public ContractDialog() : base(){
			Text = "Contract";
			Size = new Size(350,300);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			
			TableLayoutPanel tlp = new TableLayoutPanel(){Dock=DockStyle.Fill};
			tlp.ColumnCount = 2;
			
			tlp.Controls.Add(new Label(){Text = ar("Client") , TextAlign = ContentAlignment.BottomLeft});
			tlp.Controls.Add(new ComboBox(){ Name = "Client" , Width = 200});
			
			tlp.Controls.Add(new Label(){Text = ar("On Date") , TextAlign = ContentAlignment.BottomLeft});
			tlp.Controls.Add(new DateTimePicker(){ Name = "On_Date", Format = DateTimePickerFormat.Custom, CustomFormat = "dddd, dd MMM yyyy" });

			tlp.Controls.Add(new Label(){Text = ar("Hijri") , TextAlign = ContentAlignment.BottomLeft});
			tlp.Controls.Add(new TextBox(){ Name = "Hijri", ReadOnly = true });

			
			
			tlp.Controls.Add(new Label(){Text = ar("Price") , TextAlign = ContentAlignment.BottomLeft});
			tlp.Controls.Add(new NumericTextBox(){ Name = "Price" });
			
			tlp.Controls.Add(new Label(){Text = ar("Occasion") , TextAlign = ContentAlignment.BottomLeft});
			tlp.Controls.Add(new TextBox(){ Name = "Occasion", MaxLength=200,Size=new Size(200,100), Multiline = true, ScrollBars = ScrollBars.Vertical, AcceptsReturn = true, WordWrap = true });
			
			
			Button ok = new Button(){ Text = ar("OK")     , Location = new Point(150,10) , Size=new Size(80,25) };
			Button no = new Button(){ Text = ar("Cancel") , Location = new Point(240,10) , Size=new Size(80,25) };
			Button nc = new Button(){ Text = ar("New Client") , Location = new Point(60,10) , Size=new Size(80,25) };
			
			Panel bPanel = new Panel() {Dock = DockStyle.Bottom, Height = 35};
			
			bPanel.Controls.Add (nc);
			bPanel.Controls.Add (no);
			bPanel.Controls.Add (ok);
			
			CancelButton = no;
			AcceptButton = ok;
			
			Controls.Add(tlp);
			Controls.Add(bPanel);
			DockPadding.All = 10;
			ComboBox cb = (ComboBox)tlp.Controls["Client"];
			cb.DataSource = DBConnection.GetInstance().GetDataTable("select mobile,name from client order by 2");
			cb.DisplayMember = "name";
			cb.BindingContext = this.BindingContext;
			cb.DropDownStyle = ComboBoxStyle.DropDownList;
			
			var on_date = ((DateTimePicker)tlp.Controls["On_Date"]);
			on_date.ValueChanged+=new EventHandler(delegate(object s,EventArgs ea){
				((TextBox)tlp.Controls["Hijri"]).Text = DateConverter.toHijri(on_date.Value.ToString("yyyy-MM-dd"));
			});
			on_date.Value = DateTime.Now;
		}
	}
}
