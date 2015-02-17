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
		
		public static MainWindow _this;
		private StatusBar mainStatusBar = new StatusBar();
		public MainWindow () : base ()
		{
			Text = ar(@"Thuraiya - Wedding Occations Program");
			_this = this;
			IsMdiContainer = true;
			Size = new Size (1100, 500);
			BuildMenu ();
			
			//new DateConversion().Show();
		}

		private void BuildMenu ()
		{
			var ms = new MenuStrip ();
			ToolStripMenuItem tmsi, cmi;
			//Dictionary<string, List<string>> ms;
			string[] menus = Config.of["m_Menu"].Split (',');
			foreach (string menu in menus) {
				tmsi = new ToolStripMenuItem ();
				tmsi.Text = ar(menu.Substring (2));
				ms.Items.Add (tmsi);
				string[] cmenus = Config.of[menu].Split (',');
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

					if (Config.of.ContainsKey (cmenu + ".icon"))
						cmi.Image = Image.FromFile (Config.of[cmenu + ".icon"]);
						cmi.Click += new EventHandler (delegate(object s, EventArgs e) {						
							try { 
								_this.GetType ().GetMethod (((ToolStripMenuItem)s).Tag.ToString () + "Action").Invoke (_this, null);
							} catch (Exception ex) {
								LOG.error(ex.Message);
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

		
		public void DatesAction ()
		{
			Dates cal = new Dates ();
			cal.MdiParent = _this;
			cal.Show ();
		}
		
		public void CalendarAction ()
		{
			Calendar cal = new Calendar ();
			cal.MdiParent = _this;
			cal.Show ();
		}

		public void Pay_SlipsAction ()
		{
			PaySlips psl = new PaySlips();
			psl.MdiParent = _this;
			psl.Show ();
		}
		
		
		public void NationalitiesAction ()
		{
			Nationalities nat = new Nationalities();
			nat.MdiParent = _this;
			nat.Show ();
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