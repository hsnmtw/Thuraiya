using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Thuraiya
{
    public class Contracts : RTLForm
    {
        
		//private string[]columns;
		private DataGridView dg;
		public Contracts() : base() {
            Text = ar("Contracts");
			Size = new Size(640,360);
			Initialize();
			new Thread(delegate() { ReloadData(); }).Start();
		}

		private void ReloadData() {
			var sql = @"select a.id,b.name,a.on_date,a.price,a.occasion 
						  from thrdb.contract a 
						  join thrdb.client b 
			                on a.client=b.mobile 
						 order by 1";
			dg.DataSource = DBConnection.GetInstance ().GetDataTable (sql);
		}

		private void Initialize() {
			
			
			
			Panel side = new Panel (){Dock = DockStyle.Fill};// {Location=new Point( 10,10),Size=new Size(95,300)};
			Panel main = new Panel (){Dock = DockStyle.Fill};// {Location=new Point(110,10),Size=new Size(500,300)};

			SplitContainer sc = new SplitContainer ();
			sc.Panel1.Controls.Add(side);
			sc.Panel2.Controls.Add(main);
			sc.Dock = DockStyle.Fill;
			sc.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			sc.SplitterDistance  = 100;
			
			
			side.BorderStyle = BorderStyle.Fixed3D;
			main.BorderStyle = BorderStyle.Fixed3D;

			dg = new DataGridView()
			{
				Name = "dgMain",
				Dock = DockStyle.Fill,
				//RightToLeft = RightToLeft.No,
				AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders,
				ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
				CellBorderStyle = DataGridViewCellBorderStyle.Single,
				RowHeadersVisible = false,
				//ColumnHeadersDefaultCellStyle.BackColor = Color.Navy,
				//ColumnHeadersDefaultCellStyle.ForeColor = Color.White,
				AllowUserToAddRows = false,
				MultiSelect = false,
				ReadOnly = true,
				SelectionMode = DataGridViewSelectionMode.FullRowSelect,
				BorderStyle = BorderStyle.None
			};

			Button bAdd = new Button() { Name = "bAdd", Text = ar("Add"),   Size=new Size(75,25), Location = new Point(10, 5) };
			Button bDel = new Button() { Name = "bDel", Text = ar("Delete"),Size=new Size(75,25), Location = new Point(10, 40) };
			Button bEdt = new Button() { Name = "bEdt", Text = ar("Edit"),  Size=new Size(75,25), Location = new Point(10, 75) };
			Button bPrt = new Button() { Name = "bPrt", Text = ar("Print"), Size=new Size(75,25), Location = new Point(10, 110) };

			main.Controls.Add(dg);
			side.Controls.Add(bAdd);
			side.Controls.Add(bEdt);
			side.Controls.Add(bPrt);
			side.Controls.Add(bDel);

			Controls.Add(sc);
			Controls.Add(new Label(){
				Text = ar("Contracts"),
				Dock = DockStyle.Top,
				TextAlign = ContentAlignment.MiddleCenter
			});
			
			
			DataTable dt = new DataTable();
			dg.DataSource = dt;

			bAdd.Click += new EventHandler(AddNewRecordAction);
			bEdt.Click += new EventHandler(EditRecordAction);
			bDel.Click += new EventHandler(DeleteRecordAction);

		}
		public void AddNewRecordAction(object s, EventArgs ea)
		{
			BuildInputForm (-1);
		}

		public void EditRecordAction(object s, EventArgs ea)
		{
			if(dg.SelectedRows.Count<1) return;
			ContractDialog dialog = new ContractDialog();
			dialog.ShowDialog ();//(dg.SelectedRows[0].Index);
		}

		public void DeleteRecordAction(object s, EventArgs ea)
		{

		}
		public void BuildInputForm(int rowNum){
			ContractDialog dialog = new ContractDialog();
			dialog.ShowDialog ();
		}
	}
}
