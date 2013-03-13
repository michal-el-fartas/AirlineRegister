using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DbGui
{
    public class Table
    {
        public string tableName;
        public List<Field> columns;
        public Table()
        {
            columns = new List<Field>();
        }
        public SelectQuery getSelectAllQuery()
        {
            return new SelectQuery(this);
        }
        public void initDataGrid(DataGridView dgv)
        {
            dgv.Columns.Clear();
            for (int i = 0; i < columns.Count; i++)
            {
                //if (columns[i].visible)
                //{
                //    int x = dgv.Columns.Add(columns[i].name, columns[i].label);
                //    dgv.Columns[x].ReadOnly = columns[i].readOnly;
                //}
                dgv.Columns.Add(columns[i].getGridCollumn());
            }
            dgv.Columns.Add(new DataGridViewButtonColumn());
        }
        public Field getColumnByName(string name)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].name == name)
                {
                    return columns[i];
                }
            }
            return null;
        }
    }


    public class ClientsTable : Table
    {
        public ClientsTable()
            : base()
        {
            tableName = "KLIENCI";
            columns.Add(new Field("ID_KLIENTA", Field.ID, "ID"));
            columns.Add(new Field("NAZWISKO", Field.TEXT, "Nazwisko"));
            columns.Add(new Field("IMIE", Field.TEXT, "Imię"));
            columns.Add(new Field("DATA_URODZENIA", Field.DATE, "Data urodzenia"));
            columns.Add(new Field("ADRES", Field.TEXT, "Adres"));
            columns.Add(new Field("TELEFON", Field.TEXT, "Telefon"));
            columns.Add(new Field("BONUSOWE_MILE", Field.NUMERIC, "Mile"));
        }
    }



    public class AirportsTable : Table
    {
        public AirportsTable()
            : base()
        {
            tableName = "LOTNISKA";
            columns.Add(new Field("ID_LOTNISKA", Field.ID, "ID"));
            columns.Add(new Field("KRAJ", Field.TEXT, "Kraj"));
            columns.Add(new Field("MIEJSCOWOSC", Field.TEXT, "Miasto"));
            columns.Add(new Field("NAZWA", Field.TEXT, "Nazwa"));
            var cmbcol = new Field("TYP", Field.SELECT, "Typ");
            cmbcol.comboValues.Add("DUŻE");
            cmbcol.comboValues.Add("ŚREDNIE");
            cmbcol.comboValues.Add("MAŁE");
            columns.Add(cmbcol);
            //columns.Add(new Field("MAKS_ROZMIAR", Field.SELECT, "Pasażerów do przyjęcia"));
            cmbcol = new Field("MAKS_ROZMIAR", Field.SELECT, "Pasażerów do przyjęcia");
            cmbcol.comboValues.Add("MIEDZYNARODOWE");
            cmbcol.comboValues.Add("KRAJOWE");
            cmbcol.comboValues.Add("REGIONALNE");
            columns.Add(cmbcol);
            columns.Add(new Field("WSP_X", Field.NUMERIC, "Pozycja X"));
            columns.Add(new Field("WSP_Y", Field.NUMERIC, "Pozycja Y"));
        }
    }


    public class PlanesTable : Table
    {
        public PlanesTable()
            : base()
        {
            tableName = "SAMOLOTY";
            columns.Add(new Field("ID_SAMOLOTU", Field.ID, "ID"));
            columns.Add(new Field("MARKA", Field.TEXT, "Marka"));
            columns.Add(new Field("MODEL", Field.TEXT, "Model"));
            columns.Add(new Field("ZASIEG", Field.NUMERIC, "Zasięg"));
            columns.Add(new Field("MIEJSCA", Field.NUMERIC, "Ilość miejsc"));
            columns.Add(new Field("BIZNES_KLASA", Field.NUMERIC, "Miejsca w biznes klasie"));
            columns.Add(new Field("PREDKOSC", Field.NUMERIC, "Prędkość"));
            var cmbcol = new Field("TYP", Field.SELECT, "Typ");
            cmbcol.comboValues.Add("DUŻY");
            cmbcol.comboValues.Add("ŚREDNI");
            cmbcol.comboValues.Add("MAŁY");
            columns.Add(cmbcol);
        }
    }


    public class FlightsTable : Table
    {
        public FlightsTable()
            : base()
        {
            tableName = "LOTY_VIEW";
            columns.Add(new Field("ID_LOTU", Field.ID, "ID"));
            columns.Add(new Field("DATA_WYLOTU", Field.DATETIME, "Data"));
            columns.Add(new Field("CZAS_LOTU", Field.NUMERIC, "Czas lotu"));
            columns.Add(new Field("idWYLOT", Field.REF, "Wylot"));
            columns.Add(new Field("WYLOT", Field.LABEL, "Wylot"));
            columns.Add(new Field("idPRZYLOT", Field.REF, "Przylot"));
            columns.Add(new Field("PRZYLOT", Field.LABEL, "Przylot"));
            columns.Add(new Field("idSAMOLOT", Field.REF, "Samolot"));
            columns.Add(new Field("SAMOLOT", Field.LABEL, "Samolot"));
        }
    }


    public class RawFlightsTable : Table
    {
        public RawFlightsTable()
            : base()
        {
            tableName = "LOTY";
            columns.Add(new Field("ID_LOTU", Field.ID, "ID"));
        }
    }

    public class ResetvationsTable : Table
    {
        public ResetvationsTable()
            : base()
        {
            tableName = "REZERWACJE_VIEW";
            columns.Add(new Field("REZERWACJA", Field.ID, "Numer rezerwacji"));
            columns.Add(new Field("idKLIENT", Field.REF, "ID klienta"));
            columns.Add(new Field("KLIENT", Field.LABEL, "ID klienta"));
            columns.Add(new Field("LOT", Field.REF, "Numer lotu"));
            var f = new Field("KLASA", Field.SELECT, "Klasa");
            f.comboValues.Add("ECONOMIC");
            f.comboValues.Add("BUISNESS");
            columns.Add(f);
        }
    }

    public class RawResetvationsTable : Table
    {
        public RawResetvationsTable()
            : base()
        {
            tableName = "REZERWACJE";
            columns.Add(new Field("REZERWACJA", Field.ID, "Numer rezerwacji"));
        }
    }
}
