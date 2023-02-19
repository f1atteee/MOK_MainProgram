using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MOK_MainInterface.Cypher;

namespace MOK_MainInterface.Views
{
    public partial class FrequencyTable : Form
    {
        public string InputText { get; set; }
        public int KeyLenght { get; set; }


        public FrequencyTable()
        {
            InitializeComponent();
        }

        private void FrequencyTable_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Символ");
            dt.Columns.Add("Кількість");
            Dictionary<string, int> dic = new Dictionary<string, int>();
            BruteForceFrequencyTable bruteF = new BruteForceFrequencyTable();
            dic = bruteF.BuildBruteForceFrequencyTable(InputText, KeyLenght);
            foreach (var item in dic)
            {
                DataRow dr = dt.NewRow();
                dr["Символ"] = item.Key;
                dr["Кількість"] = item.Value;
                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;
        }
    }
}
