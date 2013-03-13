using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DbGui
{
    public partial class DateDialog : Form
    {
        int date_y, date_m, date_d;
        public string time,date,result;
        public DateDialog(bool timecheck=true)
        {
            InitializeComponent();
            textBox1.Enabled = timecheck;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            time = textBox1.Text;
            date_y = monthCalendar1.SelectionStart.Year;
            date_m = monthCalendar1.SelectionStart.Month;
            date_d = monthCalendar1.SelectionStart.Day;
            date=""+date_y+"-"+date_m+"-"+date_d;
            result = date + " " + time;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            String str = textBox1.Text;
            if (str.Length == 5)
            {
                if (str[2] == ':')
                {
                    int h=0, m=0;
                    bool valid = true;
                    try
                    {
                        h = Int32.Parse(str.Substring(0, 2));
                        m = Int32.Parse(str.Substring(3, 2));
                    }
                    catch(System.FormatException)
                    {
                        valid = false;
                    }
                    if (valid)
                    {
                        if ((h >= 0) && (h < 24))
                        {
                            if ((m >= 0) && (m < 60))
                            {
                                button1.Enabled = true;
                                textBox1.ForeColor = Color.Black;
                                return;
                            }
                        }
                    }
                }
            }
            button1.Enabled = false;
            textBox1.ForeColor = Color.Red;
        }
    }
}
