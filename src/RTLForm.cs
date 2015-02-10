using System.Windows.Forms;
//using System.Data;
namespace Thuraiya{
	public class RTLForm : Form{
		public readonly bool RTL;
		public RTLForm() : base(){
			RTL = Config.of ["main"].Get ("RTL").Equals ("true");
			if (RTL) {
				this.RightToLeft = RightToLeft.Yes;
			}
		}
		public string ar(string en){
			if(!RTL) return en;
			var dt =  DBConnection.GetInstance().GetDataTable(@"select arabic from lang where english='"+ en +"'");
			return dt.Rows.Count==1?dt.Rows[0]["arabic"].ToString():en;
		}
	}
}