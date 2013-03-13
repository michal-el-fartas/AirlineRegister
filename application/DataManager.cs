using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace DbGui
{
    public class DataManager
    {
        // Core
        DataBase dataBase;
        public Table table, removalTab;
        public DataManager(DataBase db, Table tab)
        {
            dataBase = db;
            table = tab;
            removalTab = tab;

            editRef = new System.Windows.Forms.ContextMenuStrip();
            ToolStripMenuItem toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            editRef.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem1 });
            editRef.Name = "editMenuStrip";
            editRef.Size = new System.Drawing.Size(153, 48);
            toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            toolStripMenuItem1.Text = "Edytuj";
        }
        
        //GUI elements
        public int tabIndex;
        public DataGridView dataGrid;
        public Button reloadButton;
        public Button quickSearch;
        public Button advancedSearch;
        public Button refreshDisplay;
        public ContextMenuStrip editRef;
        List<QuickSearchField> qSearchFields = new List<QuickSearchField>();
        public void addQuickSearchField(TextBox textBox, Label label, string fieldName)
        {
            Field field = table.getColumnByName(fieldName);
            if (field != null)
            {
                qSearchFields.Add(new QuickSearchField(textBox, label, field));
            }
            else
            {
                textBox.Text = "qSearch name error";
                label.Text = "qSearch name error";
            }
        }


        //GUI initialization
        public void initGUI()
        {
            table.initDataGrid(dataGrid);
            reloadButton.Click += new System.EventHandler(reload_Click);
            quickSearch.Click += new System.EventHandler(qsearch_Click);
            advancedSearch.Click += new System.EventHandler(advancedSearch_Click);
            refreshDisplay.Click += new System.EventHandler(refreshDisplay_Click);
            dataGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dataGrid_CellClick);
            dataGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(dataGrid_CellEndEdit);
            dataGrid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(dataGrid_CellEnter);
            dataGrid.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(dataGrid_CellLeave);
            dataGrid.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(dataGrid_UserDeletingRow);
            dataGrid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(dataGrid_CellBeginEdit);
        }

        //functions
        private void rowCommited(DataGridViewRow row)
        {
            int last = row.Cells.Count - 1;
            row.Cells[last].Value = "Aktualny";
            row.Cells[last].Style.SelectionBackColor = Color.Green;
            row.Cells[last].Style.BackColor = Color.Green;
        }
        private void rowChanged(DataGridViewRow row)
        {
            int last = row.Cells.Count - 1;
            row.Cells[last].Value = "Zmieniony";
            row.Cells[last].Style.SelectionBackColor = Color.Yellow;
            row.Cells[last].Style.BackColor = Color.Yellow;
        }
        private void rowAdded(DataGridViewRow row)
        {
            int last = row.Cells.Count - 1;
            row.Cells[last].Value = "Nowy";
            row.Cells[last].Style.SelectionBackColor = Color.Red;
            row.Cells[last].Style.BackColor = Color.Red;
        }
        public string getRowState(DataGridViewRow row)
        {
            int last = row.Cells.Count - 1;
            string str = "";
            str += row.Cells[last].Value;
            return str;
        }
        private string getCellValue(DataGridViewRow row, int index)
        {
            string str = "";
            if (row.Cells[index].Value != null)
            {
                str += row.Cells[index].Value;
            }
            return str;
        }

        public void reloadData(SelectQuery q)
        {
            NpgsqlDataReader dr = dataBase.executeQuery(q);
            if (dr == null) { return; }
            dataGrid.Rows.Clear();
            int last = dataGrid.Columns.Count - 1;
            int rowid;
            while (dr.Read())
            {
                rowid = dataGrid.Rows.Add();
                int i;
                for (i = 0; i < dr.FieldCount; i++)
                {
                    dataGrid.Rows[rowid].Cells[i].Value = dr[i];
                }
                rowCommited(dataGrid.Rows[rowid]);
                //dataGrid.Rows[rowid].ContextMenuStrip = contextMenuStrip1;
            }
            dr.Close();
        }
       
        public bool reloadRow(DataGridViewRow row,bool loadByID=true)
        {
            SelectQuery sq = table.getSelectAllQuery();
            if (loadByID)
            {
                string cond;
                cond=Field.getCellValue(row.Cells[0]);
                cond=table.columns[0].getConstraint("=",cond);
                sq.addCondition(cond);
            }
            else
            {
                sq.initCheckRow(row, true);
            }
            
            NpgsqlDataReader dr = dataBase.executeQuery(sq);
            if (dr == null) { return false; }
            bool status = dr.Read();
            if (status)
            {
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    row.Cells[i].Value = dr[i];
                }
                rowCommited(row);
            }
            dr.Close();
            return status;
        }

        private void reload_Click(object sender, EventArgs e)
        {
            SelectQuery q = table.getSelectAllQuery();
            reloadData(q);
        }

        private void refreshDisplay_Click(object sender, EventArgs e)
        {
            DataGridViewRow row;
            for (int i = 0; i < dataGrid.Rows.Count-1; i++)
            {
                row = dataGrid.Rows[i];
                if (!reloadRow(row))
                {
                    dataGrid.Rows.RemoveAt(i);
                    i--;
                }
            }
        }

        private void dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex < 0) || (e.RowIndex >= dataGrid.Rows.Count)) { return; }
            DataGridViewRow row = dataGrid.Rows[e.RowIndex];
            int last = row.Cells.Count - 1;

            if (e.ColumnIndex == last)
            {
                bool commited = false;

                if (getRowState(row) == "Nowy")
                {
                    InsertQuery iq = new InsertQuery(table);
                    iq.addValues(row);
                    commited = dataBase.executeInsert(iq);
                }
                else if (getRowState(row) == "Zmieniony")
                {
                    UpdateQuery uq = new UpdateQuery(table);
                    uq.setValues(row);
                    commited = dataBase.executeInsert(uq);
                }
                if (commited)
                {
                    reloadRow(row, false);
                }
            }
        }

        private void qsearch_Click(object sender, EventArgs e)
        {
            SelectQuery q = table.getSelectAllQuery();

            for (int i = 0; i < qSearchFields.Count; i++)
            {
                string str=qSearchFields[i].getCondition();
                if (str != null) { q.addCondition(str); }
            }

            reloadData(q);
        }

        private void dataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGrid.Rows[e.RowIndex];
            DataGridViewCell cell = row.Cells[e.ColumnIndex];
            int last = row.Cells.Count - 1;

            if (getRowState(row) == "Aktualny") { rowChanged(row); }
            else if (getRowState(row) == "Zmieniony") { }
            else if (getRowState(row) == "Nowy") { }
            else if (cell.Value != null) { rowAdded(row); }
        }

        private void dataGrid_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DataGridViewRow row = e.Row;
            if (getRowState(row) == "")
            {
                return;
            }
            e.Cancel = true;
            if (getRowState(row) != "Aktualny")
            {
                MessageBox.Show("Usuwanie nieaktualnych wierszy zabronione");
                e.Cancel = true;
                return;
            }
            DeleteDialog f = new DeleteDialog();
            f.ShowDialog();
            DialogResult result;
            result = f.DialogResult;
            if (result == DialogResult.Yes)
            {
                bool commited = false;
                DeleteQuery dq = new DeleteQuery(removalTab);
                dq.setValues(row);
                commited = dataBase.executeInsert(dq);
                e.Cancel = !commited;
            }
        }

        private void advancedSearch_Click(object sender, EventArgs e)
        {
            SearchDialog f = new SearchDialog(table);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                SelectQuery sq = table.getSelectAllQuery();
                for (int i = 0; i < f.constraints.Count; i++)
                {
                    sq.addCondition(f.constraints[i]);
                }
                reloadData(sq);
            }
        }

        private void dataGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int type=table.columns[e.ColumnIndex].type;
            if ((type == Field.DATE) || (type == Field.DATETIME))
            {
                e.Cancel = true;
                DateDialog d = new DateDialog((type == Field.DATETIME));
                if (d.ShowDialog() == DialogResult.OK)
                {
                    if (type == Field.DATETIME)
                    {
                        dataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = d.result;
                    }
                    else
                    {
                        dataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = d.date;
                    }
                    
                    dataGrid_CellEndEdit(sender, new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));
                }
            }
        }

        public void showRowByID(string id)
        {
            string cond = table.columns[0].getConstraint("=", id);
            SelectQuery sq=table.getSelectAllQuery();
            sq.addCondition(cond);
            reloadData(sq);
        }

        private void dataGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < table.columns.Count)
            {
                if (table.columns[e.ColumnIndex].type == Field.REF)
                {
                    dataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].ContextMenuStrip = editRef;
                }
            }
        }

        private void dataGrid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            dataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].ContextMenuStrip = null;
        }
    }
}
