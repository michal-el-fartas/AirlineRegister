using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DbGui
{
    public class Field
    {
        public string name;
        public string label;
        public int type;
        public bool visible;
        public bool hasDefValue;
        public bool readOnly;
        public List<string> comboValues=new List<string>();

        public static int ID = 6;
        public static int REF = 7;
        public static int LABEL = 8;
        public static int NUMERIC = 1;
        public static int TEXT = 2;
        public static int DATE = 3;
        public static int DATETIME = 4;
        public static int SELECT = 5;

        public Field(string name, int type)
        {
            this.name = name;
            this.type = type;
            this.label = name;
            visible = true;
            readOnly = ((type == ID) || (type == REF) || (type == LABEL));
            hasDefValue = false;
        }
        public Field(string name, int type, string label): this(name,type)
        {
            this.label = label;
        }
        public List<string> getOperators()
        {
            List<string> tab = new List<string>();
            tab.Add("=");
            tab.Add("!=");
            if ((type == ID) || (type == NUMERIC) || (type == DATE) || (type == DATETIME))
            {
                tab.Add(">");
                tab.Add("<");
                tab.Add(">=");
                tab.Add("<=");
            }
            return tab;
        }
        public string getConstraint(string oper, string value)
        {
            return ("(" + name + oper + value + ")");
        }
        static public string getCellValue(DataGridViewCell cell, bool text = false)
        {
            string str = "";
            str += cell.Value;
            if (text)
            {
                str = "'" + str + "'";
            }
            return str;
        }
        public string wrapValue(string value)
        {
            string str = value;
            if ((type != NUMERIC) && (type != ID) && (type != REF))
            {
                str = "'" + str + "'";
            }
            return str;
        }
        public DataGridViewColumn getGridCollumn()
        {
            DataGridViewColumn dgvc;
            if (type == SELECT)
            {
                DataGridViewComboBoxColumn dgvcbc = new DataGridViewComboBoxColumn();
                for (int i = 0; i < comboValues.Count; i++)
                {
                    dgvcbc.Items.Add(comboValues[i]);
                }
                dgvc = dgvcbc;
            }
            else
            {
                dgvc = new DataGridViewTextBoxColumn();
            }
            dgvc.ReadOnly = readOnly;
            dgvc.Visible = visible;
            dgvc.HeaderText = label;
            return dgvc;
        }
    }


    public class QuickSearchField
    {
        TextBox textBox;
        Label label;
        Field field;
        public QuickSearchField(TextBox textBox, Label label, Field field)
        {
            this.textBox = textBox;
            this.label = label;
            this.field = field;
            textBox.Text = "";
            label.Text = field.label;
        }
        public string getCondition()
        {
            if (textBox.Text == "")
            {
                return null;
            }
            else
            {
                return field.getConstraint("=", field.wrapValue(textBox.Text));
            }
        }
    }
}
