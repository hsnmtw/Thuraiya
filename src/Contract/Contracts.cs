using System;
using System.Data;
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
			Size = new Size(840,360);
			Initialize();
			new Thread(delegate() { ReloadData(); }).Start();
		}

		private void ReloadData() {
			var sql = @"select * from v_contracts;";
			var dt = DBConnection.GetInstance ().GetDataTable (sql);
			foreach(DataColumn col in dt.Columns){
				col.ColumnName = ar(col.ColumnName);
			}
			dg.DataSource = dt;
		}

		private void Initialize() {
			
			Panel side = new Panel (){Dock = DockStyle.Left, Width=100};// {Location=new Point( 10,10),Size=new Size(95,300)};
			Panel main = new Panel (){Dock = DockStyle.Fill};// {Location=new Point(110,10),Size=new Size(500,300)};			
			
			side.BorderStyle = BorderStyle.Fixed3D;
			main.BorderStyle = BorderStyle.Fixed3D;

			dg = new CustomDataGridView() { Dock = DockStyle.Fill };

			Button bAdd = new Button() { Name = "bAdd", Text = ar("Add"),   Size=new Size(75,25), Location = new Point(10, 5) };
			Button bDel = new Button() { Name = "bDel", Text = ar("Delete"),Size=new Size(75,25), Location = new Point(10, 40) };
			Button bEdt = new Button() { Name = "bEdt", Text = ar("Edit"),  Size=new Size(75,25), Location = new Point(10, 75) };
			Button bPrt = new Button() { Name = "bPrt", Text = ar("Print"), Size=new Size(75,25), Location = new Point(10, 110) };

			main.Controls.Add(dg);
			side.Controls.Add(bAdd);
			side.Controls.Add(bEdt);
			side.Controls.Add(bPrt);
			side.Controls.Add(bDel);

			Controls.Add(main);
			Controls.Add(side);
			
			Controls.Add(new Label(){
				Text = ar("Contracts"),
				Dock = DockStyle.Top,
				TextAlign = ContentAlignment.MiddleCenter
			});
			
			bAdd.Click += new EventHandler(AddNewRecordAction);
			bEdt.Click += new EventHandler(EditRecordAction);
			bDel.Click += new EventHandler(DeleteRecordAction);

		}
		public void AddNewRecordAction(object s, EventArgs ea)
		{
			if(new ContractDialog().ShowDialog()==DialogResult.OK){
				ReloadData();
			}
		}

		public void EditRecordAction(object s, EventArgs ea)
		{
			if(dg.SelectedRows.Count<1) return;
			if(new ContractDialog(dg.SelectedRows[0].Cells[0].Value.ToString()).ShowDialog()==DialogResult.OK){
				ReloadData();
			}
			
		}

		public void DeleteRecordAction(object s, EventArgs ea)
		{
			var confirm = MessageBox.Show(ar(@"Are you sure of deleting this record?"),ar(@"Warning"),MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
			if(confirm==DialogResult.Yes){
				var sql  = @"select count(1) from payslip where contract=@p0";
				var p0   = dg.SelectedRows[0].Cells[0].Value;
				var rows = int.Parse(con.GetDataTable(sql,p0).Rows[0][0].ToString());
				
				if(rows>0){ throw new Exception(@"Cannot delete a contract that has payslips:"+rows); }
				
				sql = @"delete from contract where id=@p0";
				if(DBConnection.GetInstance().Execute(sql,p0)){
					MessageBox.Show(ar(@"Your request has been successfully processed"),ar(@"Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
					ReloadData();
				}else{
					MessageBox.Show(ar(@"Failed to process your request"),ar(@"Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}
}
