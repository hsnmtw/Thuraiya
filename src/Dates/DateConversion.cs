using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Thuraiya
{

    public class DateConversion : RTLForm
    {
        public DateConversion() : base() {
            Text = ar("Date Conversion");
            Size = new Size(400,180);
            Initialize();
        }

        
        private void Initialize()
        {
            Image imgX = Image.FromFile(Config.of[@"delete.sign"]);
            Label lH = new Label() { Text = "Hijri", Location = new Point(10, 13), Size = new Size(80, 25) };
            TextBox tHd = new TextBox() { Name = "tHd", Location = new Point(120, 10), Size = new Size(30, 20) , TextAlign =HorizontalAlignment.Center};
            TextBox tHm = new TextBox() { Name = "tHm", Location = new Point(150, 10), Size = new Size(30, 20) , TextAlign =HorizontalAlignment.Center};
            TextBox tHy = new TextBox() { Name = "tHy", Location = new Point(180, 10), Size = new Size(50, 20) , TextAlign =HorizontalAlignment.Center};
            PictureBox hX = new PictureBox() { Name = "hX", Location = new Point(235, 10), Size = new Size(20, 20), Image = imgX, Cursor = Cursors.Hand, SizeMode = PictureBoxSizeMode.StretchImage };
            Button bH2G = new Button() { Name = "bH2G", Location = new Point(280, 10), Size = new Size(80, 20), Text = "To Greg" };

            Label lG = new Label() { Text = "Gregorian", Location = new Point(10, 43), Size = new Size(80, 20) };
            TextBox tGd = new TextBox() { Name = "tHd", Location = new Point(120, 40), Size = new Size(30, 20) , TextAlign =HorizontalAlignment.Center};
            TextBox tGm = new TextBox() { Name = "tHm", Location = new Point(150, 40), Size = new Size(30, 20) , TextAlign =HorizontalAlignment.Center};
            TextBox tGy = new TextBox() { Name = "tHy", Location = new Point(180, 40), Size = new Size(50, 20) , TextAlign =HorizontalAlignment.Center};
            PictureBox gX = new PictureBox() { Name = "gX", Location = new Point(235, 40), Size = new Size(20, 20), Image = imgX, Cursor = Cursors.Hand, SizeMode = PictureBoxSizeMode.StretchImage };
            Button bG2H = new Button() { Name = "bG2H", Location = new Point(280, 40), Size = new Size(80, 20), Text = "To Hijri" };
//LOG.info("LLL");
            object[] controls = new object[] { lH, lG, tHd, tHm, tHy, hX, bH2G, tGd, tGm, tGy, gX, bG2H };
            foreach(Control control in controls)
            { Controls.Add(control); }


            hX.Click +=  new EventHandler(delegate(object s, EventArgs ea) {
                tHd.Text = "";
                tHm.Text = "";
                tHy.Text = "";
            });
            gX.Click +=  new EventHandler(delegate(object s, EventArgs ea) {
                tGd.Text = "";
                tGm.Text = "";
                tGy.Text = "";
            });
            bH2G.Click += new EventHandler(delegate(object s, EventArgs ea) {
                string h = string.Format("{2}-{1,2}-{0,2}", tHd.Text, tHm.Text, tHy.Text).Replace(' ','0');
                string[] g = DateConverter.toGreg(h).Split(',');
                tGd.Text = g[2];
                tGm.Text = g[1];
                tGy.Text = g[0];
            });

            bG2H.Click += new EventHandler(delegate(object s, EventArgs ea){
                string g = string.Format("{2}-{1,2}-{0,2}", tGd.Text, tGm.Text, tGy.Text).Replace(' ','0');
                string[] h = DateConverter.toHijri(g).Split('-');
                tHd.Text = h[2];
                tHm.Text = h[1];
                tHy.Text = h[0];
            });
            DateTime date = DateTime.Now;
            tGd.Text = date.Day+"";
            tGm.Text = date.Month+"";
            tGy.Text = date.Year+"";
            //bG2H.PerformClick();
        }
    }
}