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
    public partial class SearchDialog : Form
    {
        Table src;
        public List<string> constraints;
        Field curr;
        public SearchDialog(Table tab)
        {
            src=tab;
            constraints = new List<string>();
            InitializeComponent();
            for (int i = 0; i < src.columns.Count; i++)
            {
                comboBox1.Items.Add(src.columns[i].label);
            }
            curr = null;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            curr = src.columns[comboBox1.SelectedIndex];
            List<string> operators;
            operators = curr.getOperators();
            comboBox2.Items.Clear();
            for (int i = 0; i < operators.Count; i++)
            {
                comboBox2.Items.Add(operators[i]);
            }
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (curr == null) { return; }
            string str="";
            if (curr.type == Field.NUMERIC)
            {
                str=textBox1.Text;
            }
            else
            {
                str="'"+textBox1.Text+"'";
            }
            str=curr.getConstraint(comboBox2.SelectedItem.ToString(), str);
            constraints.Add(str);
            str = "";
            str += curr.label + " ";
            str += comboBox2.SelectedItem.ToString() + " ";
            str += textBox1.Text;
            listBox1.Items.Add(str);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (curr == null) { return; }
            if ((curr.type == Field.DATE) || (curr.type == Field.DATETIME))
            {
                DateDialog d = new DateDialog((curr.type == Field.DATETIME));
                if (d.ShowDialog() == DialogResult.OK)
                {
                    if (curr.type == Field.DATETIME)
                    {
                        textBox1.Text = d.result;
                    }
                    else
                    {
                        textBox1.Text = d.date;
                    }
                }
            }
        }
    }
}
