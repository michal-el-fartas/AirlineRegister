using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DbGui
{
    public interface ExecutableQuery
    {
        string getQuery();
    }
    public class CommonQuery : ExecutableQuery
    {
        string q;
        public CommonQuery(string query)
        {
            q = query;
        }
        public string getQuery()
        {
            return q;
        }
    }
    public class SelectQuery : ExecutableQuery
    {
        string tableName;
        List<Field> fields;
        List<string> conditions;
        string order;
        public SelectQuery(string table)
        {
            tableName = table;
            conditions = new List<string>();
            fields = new List<Field>();
            order = "";
        }
        public SelectQuery(Table tab)
        {
            tableName = tab.tableName;
            conditions = new List<string>();
            fields = new List<Field>();
            order = "";
            for (int i = 0; i < tab.columns.Count; i++)
            {
                if (tab.columns[i].visible)
                {
                    fields.Add(tab.columns[i]);
                }
            }
        }
        public void addCondition(string cond)
        {
            conditions.Add(cond);
        }
        public void clearConditions()
        {
            conditions.Clear();
        }
        public void addField(Field field)
        {
            fields.Add(field);
        }
        public void clearFields()
        {
            fields.Clear();
        }
        public void initCheckRow(DataGridViewRow row, bool noID = false)
        {
            conditions.Clear();
            int i;
            if (noID) { i = 1; }
            else { i = 0; }

            for (; i < fields.Count; i++)
            {
                if (fields[i].type == Field.NUMERIC)
                {
                    string str;
                    str = Field.getCellValue(row.Cells[i]);
                    str = fields[i].getConstraint("=", str);
                    conditions.Add(str);
                }
                else
                {
                    string str;
                    str = Field.getCellValue(row.Cells[i], true);
                    str = fields[i].getConstraint("=", str);
                    conditions.Add(str);
                }
            }

            order = " ORDER BY ";
            order += fields[0].name;
            order += " DESC ";
        }
        public string getQuery()
        {
            string statement;
            statement = "SELECT ";
            if (fields.Count > 0)
            {
                int i;
                for (i = 0; i < fields.Count - 1; i++)
                {
                    statement += fields[i].name + ", ";
                }
                statement += fields[i].name;
            }
            else
            {
                statement += "*";
            }
            statement += " FROM " + tableName + " ";
            if (conditions.Count > 0)
            {
                statement += "WHERE ";
                int i;
                for (i = 0; i < conditions.Count - 1; i++)
                {
                    statement += conditions[i] + " AND ";
                }
                statement += conditions[i];
            }
            statement += order + ";";
            return statement;
        }
    }
    public class InsertQuery : ExecutableQuery
    {
        string tableName;
        List<Field> fields;
        List<List<string>> values;
        public InsertQuery(Table tab)
        {
            tableName = tab.tableName;
            fields = new List<Field>();
            values = new List<List<string>>();

            for (int i = 0; i < tab.columns.Count; i++)
            {
                if (tab.columns[i].visible)
                {
                    fields.Add(tab.columns[i]);
                }
            }
        }
        public void addValues(DataGridViewRow row)
        {
            List<string> val = new List<string>();
            for (int i = 1; i < fields.Count; i++)
            {
                if (fields[i].type == Field.NUMERIC)
                {
                    val.Add(Field.getCellValue(row.Cells[i]));
                }
                else
                {
                    val.Add(Field.getCellValue(row.Cells[i], true));
                }
            }
            values.Add(val);
        }
        public void addField(string field)
        {
            fields.Add(new Field(field, Field.TEXT));
        }
        public void clearFields()
        {
            fields.Clear();
        }
        public string getQuery()
        {
            string statement;
            statement = "INSERT INTO " + tableName;
            if (fields.Count > 0)
            {
                statement += "(";
                int i;
                for (i = 1; i < fields.Count - 1; i++)
                {
                    statement += fields[i].name + ",";
                }
                statement += fields[i].name;
                statement += ") ";
            }
            else
            {
                statement += " ";
            }
            statement += " VALUES ";
            int j, k;
            for (j = 0; j < values.Count; j++)
            {
                statement += "(";
                for (k = 0; k < values[j].Count - 1; k++)
                {
                    statement += values[j][k] + ",";
                }
                statement += values[j][k];
                if (j < values.Count - 1)
                { statement += "),"; }
                else
                { statement += ")"; }
            }
            statement += ";";
            return statement;
        }
    }
    public class UpdateQuery : ExecutableQuery
    {
        string tableName;
        List<Field> fields;
        List<string> values;
        public UpdateQuery(Table tab)
        {
            tableName = tab.tableName;
            fields = new List<Field>();

            for (int i = 0; i < tab.columns.Count; i++)
            {
                if (tab.columns[i].visible)
                {
                    fields.Add(tab.columns[i]);
                }
            }
        }
        public void setValues(DataGridViewRow row)
        {
            values = new List<string>();
            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].type == Field.NUMERIC)
                {
                    values.Add(Field.getCellValue(row.Cells[i]));
                }
                else
                {
                    values.Add(Field.getCellValue(row.Cells[i], true));
                }
            }
        }
        public void addField(string field)
        {
            fields.Add(new Field(field, Field.TEXT));
        }
        public void clearFields()
        {
            fields.Clear();
        }
        public string getQuery()
        {
            string statement;
            statement = "UPDATE " + tableName + " SET ";
            if (fields.Count > 0)
            {
                int i;
                for (i = 1; i < fields.Count - 1; i++)
                {
                    statement += fields[i].name + "=";
                    statement += values[i] + ", ";
                }
                statement += fields[i].name + "=";
                statement += values[i] + " ";
            }
            else
            {
                statement += " ";
            }
            statement += "WHERE ";
            statement += fields[0].name + "=";
            statement += values[0] + ";";
            return statement;
        }
    }
    public class DeleteQuery : ExecutableQuery
    {
        string tableName;
        List<Field> fields;
        List<string> values;
        public DeleteQuery(Table tab)
        {
            tableName = tab.tableName;
            fields = new List<Field>();

            if (tab.columns.Count > 0)
            {
                fields.Add(tab.columns[0]);
            }
        }
        public void setValues(DataGridViewRow row)
        {
            values = new List<string>();
            values.Add(Field.getCellValue(row.Cells[0]));
        }
        public void addField(string field)
        {
            fields.Add(new Field(field, Field.TEXT));
        }
        public void clearFields()
        {
            fields.Clear();
        }
        public string getQuery()
        {
            string statement;
            statement = "DELETE FROM " + tableName + " ";
            statement += "WHERE ";
            statement += fields[0].name + "=";
            statement += values[0] + ";";
            return statement;
        }
    }
    
}
