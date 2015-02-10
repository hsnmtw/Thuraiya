using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

//using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;

[assembly: AssemblyTitle("Thuraiya")]
[assembly: AssemblyDescription("Wedding Organizer")]
[assembly: AssemblyConfiguration("./config/config.txt")]
[assembly: AssemblyCompany("Hussain Al Mutawa")]
[assembly: AssemblyProduct("Thuraiya")]
[assembly: AssemblyCopyright("Copyright (c) Hussain Al Mutawa 2015")]
[assembly: AssemblyTrademark("hussain.mutawa@gmail.com")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en-NZ")]
[assembly: CLSCompliant(true)]

namespace Thuraiya
{

	public class Program : RTLForm
	{
		[STAThreadAttribute ()]
		public static void Main (string[]args){
			//new Thread (delegate() {}).Start();
			DBConnection.GetInstance();
			Application.EnableVisualStyles();
			Application.Run (new MainWindow ());
		}
	}

}