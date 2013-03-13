using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DbGui
{
    public class TBoxJump
    {
        TabControl cTab;
        int srcTab,destTab;
        String content;
        TextBox source;
        DataGridView dgv;

        public TBoxJump(TabControl cTab)
        {
            this.cTab = cTab;
        }

        public void initDest(DataGridView dgv, int destTab)
        {
            this.destTab = destTab;
            this.dgv = dgv;
            dgv.CellDoubleClick += cellDoubleClick;
        }

        public void initSrc(TextBox source)
        {
            //this.source = source;
            //this.srcTab = srcTab;
            source.DoubleClick += tboxDoubleClick;
        }

        private void cellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (source == null) { return; }
            if (e.ColumnIndex < 1)
            {
                content = Field.getCellValue(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex]);
                source.Text = content;
                cTab.SelectedIndex = srcTab;
                source = null;
            }
        }

        private void tboxDoubleClick(object sender, EventArgs e)
        {
            source = (TextBox)sender;
            srcTab = cTab.SelectedIndex;
            cTab.SelectedIndex = destTab;
        }
    }

    public class CellJump
    {
        TabControl cTab;
        int srcTab, destTab;
        String content;
        DataManager srcDM,destDM;
        DataGridView srcGrid,destGrid;
        DataGridViewCell source;
        int col;

        public CellJump(TabControl cTab)
        {
            this.cTab = cTab;
        }

        public void initSrc(DataManager dm,int col)
        {
            srcDM = dm;
            srcGrid = dm.dataGrid;
            srcGrid.CellDoubleClick += srcDoubleClick;
            dm.editRef.ItemClicked += editMenu_Clicked;
            this.col = col;
        }

        public void initDest(DataManager dm, int destTab)
        {
            destDM = dm;
            this.destTab = destTab;
            destGrid = dm.dataGrid;
            destGrid.CellDoubleClick += destDoubleClick;
        }

        private void srcDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == col)
            {
                //if (e.RowIndex >= srcGrid.Rows.Count - 1) { return; }
                if (srcDM.getRowState(srcGrid.Rows[e.RowIndex]) == "Nowy")
                    { return; }

                int c = e.ColumnIndex;
                if (srcDM.table.columns[c].type == Field.LABEL) { c--; }
                source = srcGrid.Rows[e.RowIndex].Cells[c];
                destDM.showRowByID(Field.getCellValue(source));
                source = null;

                srcTab = cTab.SelectedIndex;
                cTab.SelectedIndex = destTab;
            }
        }

        private void editMenu_Clicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //srcGrid.SelectedColumns[0].Index
            source = srcGrid.SelectedCells[0];
            srcTab = cTab.SelectedIndex;
            cTab.SelectedIndex = destTab;
        }

        private void destDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (source == null) { return; }
            if (e.ColumnIndex < 1)
            {
                content = Field.getCellValue(destGrid.Rows[e.RowIndex].Cells[e.ColumnIndex]);
                source.Value = content;
                cTab.SelectedIndex = srcTab;
                source = null;
            }
        }
    }
}
