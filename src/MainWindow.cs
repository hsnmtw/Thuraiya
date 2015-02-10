using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

//using System.Globalization;
using System.Threading;

namespace Thuraiya
{

	public class MainWindow : RTLForm
	{
		
		private readonly MainWindow _this;
		
		public MainWindow () : base ()
		{
			_this = this;
			IsMdiContainer = true;
			Size = new Size (1100, 500);
			BuildMenu ();
		}

		private void BuildMenu ()
		{
			var ms = new MenuStrip ();
			ToolStripMenuItem tmsi, cmi;
			//Dictionary<string, List<string>> ms;
			Config config = Config.of ["main"];
			string[] menus = config.Get ("m_Menu").Split (',');
			foreach (string menu in menus) {
				tmsi = new ToolStripMenuItem ();
				tmsi.Text = ar(menu.Substring (2));
				ms.Items.Add (tmsi);
				string[] cmenus = config.Get (menu).Split (',');
				foreach (string cmenu in cmenus) {
					if (cmenu.Trim ().Length == 0)
						continue;
					if (cmenu.Equals ("-")) {
						tmsi.DropDownItems.Add (new ToolStripSeparator ());
						continue;
					}
					cmi = new ToolStripMenuItem ();
					cmi.Text = ar(cmenu);
					cmi.Tag = cmenu;
					tmsi.DropDownItems.Add (cmi);

					if (config.Has (cmenu + ".icon"))
						cmi.Image = Image.FromFile (config.Get (cmenu + ".icon"));

					cmi.Click += new EventHandler (delegate(object s, EventArgs e) {
                      
						_this.GetType ().GetMethod (((ToolStripMenuItem)s).Tag.ToString () + "Action").Invoke (_this, null);
						try { 
						} catch (Exception ex) {
							Console.WriteLine (DateTime.Now.ToString ("yyyyMMddHHmmss") + ":ERROR:" + ex.Message);
						}
					});
				}
			}

			Controls.Add (ms);
		}

		public void ExitAction ()
		{
			DBConnection.GetInstance().Close();
			this.Close();
			Application.Exit();
			Environment.Exit(0);
		}

		public void AboutAction ()
		{
			MessageBox.Show (
				"Programmed By: Hussain Al-Mutawa\n+966 508456745\nhussain.mutawa@gmail.com\nIcons from: http://icons8.com/"
				, "About"
				, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public void CalendarAction ()
		{
			Calendar cal = new Calendar (2015);
			cal.MdiParent = _this;
			cal.Show ();
		}

		public void ContractsAction ()
		{
			Contracts con = new Contracts ();
			con.MdiParent = _this;
			con.Show ();
		}

		public void ClientsAction ()
		{
			Clients con = new Clients();
			con.MdiParent = _this;
			con.Show ();
		}

		public void Date_ConversionAction ()
		{
			DateConversion con = new DateConversion ();
			con.MdiParent = _this;
			con.Show ();
		}
	}

}