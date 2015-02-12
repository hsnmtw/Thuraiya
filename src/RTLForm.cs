using System;
using System.Windows.Forms;
using System.Collections.Generic;
namespace Thuraiya{
	public class RTLForm : Form{
		public DBConnection con {get{return DBConnection.GetInstance();}}
		public static bool RTL;
		private readonly Dictionary<string,string> lang;
		public RTLForm() : base(){
			StartPosition = FormStartPosition.CenterScreen;
			//CenterToScreen();
			LOG.info(Config.of["RTL"]);
			RTL = "true".Equals(Config.of["RTL"]);
			if (RTL) {
				this.RightToLeft = RightToLeft.Yes;
			}
			lang = new Dictionary<string,string>();
		}
		public string ar(string en){
			if(!RTL) return en;
			var dt =  con.GetDataTable("select arabic from thrdb.lang where english = @p0;",en);
			if(dt.Rows.Count==0) {return en;} else { lang[en] = dt.Rows[0]["arabic"].ToString(); }
			return lang[en];
		}
		
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				if (RTL)
				{
					createParams.ExStyle |= 0x500000; // WS_EX_LAYOUTRTL | WS_EX_NOINHERITLAYOUT
					createParams.ExStyle &= ~0x7000; // WS_EX_RIGHT | WS_EX_RTLREADING | WS_EX_LEFTSCROLLBAR
				}
				return createParams;
			}
		}

		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			RecreateHandle();
		}
	}
	
	public class RTLPanel : Panel{
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				if (RTLForm.RTL)
				{
					createParams.ExStyle |= 0x500000; // WS_EX_LAYOUTRTL | WS_EX_NOINHERITLAYOUT
					createParams.ExStyle &= ~0x7000; // WS_EX_RIGHT | WS_EX_RTLREADING | WS_EX_LEFTSCROLLBAR
				}
				return createParams;
			}
		}

		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			RecreateHandle();
		}		
	}
}