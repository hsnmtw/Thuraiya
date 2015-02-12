using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Thuraiya
{
    public class Calendar : RTLForm
    {
        public Calendar(int year) : base()
        {
            Text = ar("Calendar");
            WindowState = FormWindowState.Maximized;
            YearView viewer = new YearView(year);
            YearSelector selector = new YearSelector(viewer);
            Controls.Add(viewer);
            Controls.Add(selector);
        }
    }
    public class YearSelector : Panel {
        private YearView _viewer;
        public YearSelector(YearView viewer) : base(){
            _viewer = viewer;
            BorderStyle = BorderStyle.Fixed3D;
            Left = 10; Top = 68; Width = 1040; Height = 30;
            Initialize();
        }
        private void Initialize()
        {
            Controls.Add(new Label() { Text = "Year:", Top = 6, Left = 5, Width = 50, RightToLeft = RightToLeft.No });
            ComboBox combo = new ComboBox()
            {
                Left = 60, Top = 3, RightToLeft = RightToLeft.No
            };
            Controls.Add(combo);
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.Items.Add("2015");
            combo.Items.Add("2016");
            combo.Text = "2015";
            combo.SelectedIndexChanged += new EventHandler(delegate(object s,EventArgs ea) {
                _viewer.Refill( int.Parse(combo.Text) );
            });
        }
    }
    public class YearView : Panel {
        public readonly Dictionary<int, Dictionary<int, Label>> labels;
        public YearView(int year) : base(){
            labels = new Dictionary<int, Dictionary<int, Label>>();
            BorderStyle = BorderStyle.Fixed3D;
            Left = 10; Top = 100; Width = 1040; Height = 230;
            Initialize();
            Refill(year);
        }

        private void Initialize()
        {
            string[] Days = Config.of["Days"].Split(',');
            for (int i = 0; i < 37; i++)
            {
                Controls.Add(new CalLabel(Days[i % 7], 100 + i * 25, 10, 23, Color.Gray) { ForeColor = Color.White });
            }

            
            for (int m = 1; m < 13; m++)
            {
                labels[m] = new Dictionary<int, Label>();
                Controls.Add(new CalLabel(Config.of["MONTH."+m], 10, 10 + (16 * m), 88, m % 2 == 0 ? Color.WhiteSmoke : Color.White));
                for (int i = 0; i < 37; i++)
                {
                    labels[m][i] = new CalLabel("", 100 + i * 25, 10 + (16 * m), 23, m % 2 == 0 ? Color.WhiteSmoke : Color.White);
                    Controls.Add(labels[m][i]);
                }

            }
            
        }

        public void Refill(int year)
        {
            string[] Days = Config.of["Days"].Split(',');
            

            for (int m = 1; m < 13; m++)
            {

                int d = 1;
                string firstDay = string.Format("{0:ddd}", new DateTime(year, m, 1)).Substring(0, 2).ToUpper();
                int lastDayOfMonth = new DateTime(year, m, 1).AddMonths(1).AddDays(-1).Day;
                Console.WriteLine(year + ":" + m + ":" + firstDay);
                object o = "";
                for (int i = 0; i < 37; i++)
                {
                    if ((d == 1 && !firstDay.Equals(Days[i % 7])) || d > lastDayOfMonth) o = ""; else o = d++;
                    labels[m][i].Text = o.ToString();
                }

            }
        }

        public class CalLabel : Label
        {
            public CalLabel(object wd,int left, int top,int width, Color col) : base() {
                TextAlign = ContentAlignment.MiddleCenter;
                Text = wd.ToString();
                BackColor = col;
                Width = width;
                Height = 15;
                Left = left;
                Top = top;
            }
        }
    }
}