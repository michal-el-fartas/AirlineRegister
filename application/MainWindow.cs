using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace DbGui
{
    public partial class MainWindow : Form
    {
        Size formSize,viewSize;
        StartWindow parent;
        DataBase dataBase;
        public MainWindow(StartWindow launcher,DataBase db)
        {
            InitializeComponent();
            parent = launcher;
            dataBase = db;
            formSize = new Size(Size.Width, Size.Height);
            viewSize = new Size(CTgrid.Size.Width, CTgrid.Size.Height);
            dataBase.executeProcedure("SELECT USUN_STARE();");
            TBoxJump TJ;
            CellJump CJ;

            DataManager clientsManager = new DataManager(dataBase, new ClientsTable());
            clientsManager.dataGrid = CTgrid;
            clientsManager.reloadButton = CTreloadData;
            clientsManager.quickSearch = CTqsearch;
            clientsManager.refreshDisplay = CTrefreshView;
            clientsManager.advancedSearch = CTadvSearch;
            clientsManager.addQuickSearchField(CTtbox1, CTlabel1, "ID_KLIENTA");
            clientsManager.addQuickSearchField(CTtbox2, CTlabel2, "NAZWISKO");
            clientsManager.addQuickSearchField(CTtbox3, CTlabel3, "IMIE");
            clientsManager.initGUI();



            DataManager planesManager = new DataManager(dataBase, new PlanesTable());
            planesManager.dataGrid = PTgrid;
            planesManager.reloadButton = PTreloadData;
            planesManager.quickSearch = PTqsearch;
            planesManager.refreshDisplay = PTrefreshView;
            planesManager.advancedSearch = PTadvSearch;
            planesManager.addQuickSearchField(PTtbox1, PTlabel1, "ID_SAMOLOTU");
            planesManager.addQuickSearchField(PTtbox2, PTlabel2, "MARKA");
            planesManager.addQuickSearchField(PTtbox3, PTlabel3, "MODEL");
            planesManager.initGUI();



            DataManager airportsManager = new DataManager(dataBase, new AirportsTable());
            airportsManager.dataGrid = ATgrid;
            airportsManager.reloadButton = ATreloadData;
            airportsManager.quickSearch = ATqsearch;
            airportsManager.refreshDisplay = ATrefreshView;
            airportsManager.advancedSearch = ATadvSearch;
            airportsManager.addQuickSearchField(ATtbox1, ATlabel1, "ID_LOTNISKA");
            airportsManager.addQuickSearchField(ATtbox2, ATlabel2, "MIEJSCOWOSC");
            airportsManager.addQuickSearchField(ATtbox3, ATlabel3, "NAZWA");
            airportsManager.initGUI();


            DataManager flightsManager = new DataManager(dataBase, new FlightsTable());
            flightsManager.dataGrid = FTgrid;
            flightsManager.reloadButton = FTreloadData;
            flightsManager.quickSearch = FTqsearch;
            flightsManager.refreshDisplay = FTrefreshView;
            flightsManager.advancedSearch = FTadvSearch;
            flightsManager.addQuickSearchField(FTtbox1, FTlabel1, "DATA_WYLOTU");
            flightsManager.addQuickSearchField(FTtbox2, FTlabel2, "idWYLOT");
            flightsManager.addQuickSearchField(FTtbox3, FTlabel3, "idPRZYLOT");
            flightsManager.initGUI();
            flightsManager.dataGrid.Columns[3].Visible = false;
            flightsManager.dataGrid.Columns[5].Visible = false;
            flightsManager.dataGrid.Columns[7].Visible = false;
            flightsManager.removalTab=new RawFlightsTable();

            TJ = new TBoxJump(mainTab);
            TJ.initSrc(FTtbox2);
            TJ.initSrc(FTtbox3);
            TJ.initDest(ATgrid, 4);


            DataManager reservaionsManager = new DataManager(dataBase, new ResetvationsTable());
            reservaionsManager.dataGrid = RTgrid;
            reservaionsManager.reloadButton = RTreloadData;
            reservaionsManager.quickSearch = RTqsearch;
            reservaionsManager.refreshDisplay = RTrefreshView;
            reservaionsManager.advancedSearch = RTadvSearch;
            reservaionsManager.addQuickSearchField(RTtbox1, RTlabel1, "REZERWACJA");
            reservaionsManager.addQuickSearchField(RTtbox2, RTlabel2, "idKLIENT");
            reservaionsManager.addQuickSearchField(RTtbox3, RTlabel3, "LOT");
            reservaionsManager.initGUI();
            reservaionsManager.dataGrid.Columns[1].Visible = false;
            reservaionsManager.removalTab = new RawResetvationsTable();

            TJ = new TBoxJump(mainTab);
            TJ.initSrc(RTtbox2);
            TJ.initDest(CTgrid, 1);

            TJ = new TBoxJump(mainTab);
            TJ.initSrc(RTtbox3);
            TJ.initDest(FTgrid, 3);



            

            TJ = new TBoxJump(mainTab);
            TJ.initSrc(NRclient);
            TJ.initDest(CTgrid, 1);

            TJ = new TBoxJump(mainTab);
            TJ.initSrc(NRflight);
            TJ.initDest(FTgrid, 3);

            TJ = new TBoxJump(mainTab);
            TJ.initSrc(NFsrc);
            TJ.initSrc(NFdest);
            TJ.initDest(ATgrid, 4);

            TJ = new TBoxJump(mainTab);
            TJ.initSrc(NFplane);
            TJ.initDest(PTgrid, 5);

            


            CJ = new CellJump(mainTab);
            CJ.initSrc(reservaionsManager, 2);
            CJ.initDest(clientsManager,1);

            CJ = new CellJump(mainTab);
            CJ.initSrc(reservaionsManager,3);
            CJ.initDest(flightsManager, 3);

            CJ = new CellJump(mainTab);
            CJ.initSrc(flightsManager, 4);
            CJ.initDest(airportsManager, 4);

            CJ = new CellJump(mainTab);
            CJ.initSrc(flightsManager, 6);
            CJ.initDest(airportsManager, 4);

            CJ = new CellJump(mainTab);
            CJ.initSrc(flightsManager, 8);
            CJ.initDest(planesManager,5);

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            dataBase.close();
            parent.Show();
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            Size nxt = new Size(Size.Width, Size.Height);
            int h = nxt.Height - formSize.Height;
            int w = nxt.Width - formSize.Width;
            viewSize.Width += w;
            viewSize.Height += h;
            CTgrid.Size = viewSize;
            ATgrid.Size = viewSize;
            PTgrid.Size = viewSize;
            FTgrid.Size = viewSize;
            RTgrid.Size = viewSize;
            formSize = nxt;
        }

        private void NFdate_Click(object sender, EventArgs e)
        {
            DateDialog d = new DateDialog(true);
            if (d.ShowDialog() == DialogResult.OK)
            {
                TextBox t = (TextBox)(sender);
                t.Text = d.result;
            }
        }

        private void NF_TextChanged(object sender, EventArgs e)
        {
            int dist, time;
            if ((NFsrc.Text != "") && (NFdest.Text != ""))
            {
                string q="SELECT DYSTANS(";
                q += NFsrc.Text + ",";
                q += NFdest.Text + ");";
                dist=dataBase.executeFunction(q);
                NFdist.Text = "" + dist;

                if ((NFdist.Text != "") && (NFplane.Text != ""))
                {
                    q = "SELECT PREDKOSC FROM SAMOLOTY WHERE ID_SAMOLOTU=";
                    q += NFplane.Text + ";";
                    time = dataBase.executeFunction(q);
                    time = dist * 60 / time;
                    NFtime.Text = "" + time;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string q = "INSERT INTO LOTY(";
            q += "data_wylotu,";
            q += "czas_lotu,";
            q += "lotnisko_docelowe,";
            q += "lotnisko_zrodlowe,";
            q += "samolot) ";
            q += "VALUES(";
            q += "'" + NFdate.Text + "',";
            q += NFtime.Text + ",";
            q += NFdest.Text + ",";
            q += NFsrc.Text + ",";
            q += NFplane.Text + ");";
            CommonQuery cq = new CommonQuery(q);
            if (dataBase.executeInsert(cq))
            {
                MessageBox.Show("Lot został poprawnie dodany.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string q = "INSERT INTO REZERWACJE(";
            q += "ID_KLIENTA,";
            q += "ID_LOTU,";
            q += "KLASA) ";
            q += "VALUES(";
            q += NRclient.Text + ",";
            q += NRflight.Text + ",";
            q += "'" + NRclass.SelectedItem + "');";
            CommonQuery cq = new CommonQuery(q);
            if (dataBase.executeInsert(cq))
            {
                MessageBox.Show("Rezerwacja została poprawnie dodana.");
            }
        }

        
    }
}
