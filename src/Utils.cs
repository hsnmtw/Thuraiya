using System;
using System.Drawing;
using System.Windows.Forms;

namespace Thuraiya{
	
	public enum LOGType{INFO,WARN,ERROR}
	public class LOG{
		public static string info(string msg,params object[]parr){
			return emmit(string.Format(msg,parr),LOGType.INFO);
		}
		public static string error(string msg,params object[]parr){
			return emmit(string.Format(msg,parr),LOGType.ERROR);
		}
		public static string warn(string msg,params object[]parr){
			return emmit(string.Format(msg,parr),LOGType.WARN);
		}
		public static string emmit(string msg, LOGType tp){
			var res = DateTime.Now.ToString("[yyyyMMddHHmmss]")+":"+tp+":"+msg;
			Console.WriteLine(res);	
			return res;
		}
	}
	
	public class DTPicker : DateTimePicker{
		public DTPicker() : base(){
			Format = DateTimePickerFormat.Custom;
			CustomFormat = "dddd, dd MMM yyyy";
		}
	}
	
	public class CustomDataGridView : DataGridView
	{
		public CustomDataGridView() : base()
		{
			AllowUserToAddRows = false;
			AllowUserToDeleteRows = false;
			AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
			AllowUserToOrderColumns = false;
			ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
			ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
			AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			CellBorderStyle = DataGridViewCellBorderStyle.Single;
			RowHeadersVisible = false;
			AllowUserToAddRows = false;
			MultiSelect = false;
			ReadOnly = true;
			SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			BorderStyle = BorderStyle.None;
		}
	}
	public class NumericTextBox : TextBox{
		
		public NumericTextBox() : base(){
			KeyPress+=new KeyPressEventHandler(onKeyPress);
		}
		
		private void onKeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
			{
					e.Handled = true;
			}

			// only allow one decimal point
			if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
			{
				e.Handled = true;
			}
		}
	}
	
}