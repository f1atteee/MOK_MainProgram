﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MOK_MainInterface.Cypher;
using System.Drawing.Printing;
using static MOK_MainInterface.Cypher.BruteForceAlgo;

namespace MOK_MainInterface.Views
{

    public partial class MainForm : Form
    {

        public int StepCrp { get; set; }

        public bool Crypto { get; set; } // true розшифрувати; false зашифрувати
        public bool ChangeCR { get; set; }
        public bool ChangeLan { get; set; }
        public bool Language { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("ТОЧНО", "Ви впевнені??", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
            
        }

        private void проСистемуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutSystem ab = new AboutSystem();
            ab.ShowDialog(this);

        }

        private void відкритиToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dialog.InitialDirectory = "C:\\";
            dialog.Title = "Select a text file";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string fname = dialog.FileName;
                richTextBox1.Text = File.ReadAllText(fname, Encoding.UTF8);


            }
        }

        private void trackBar1_Scroll_1(object sender, EventArgs e)
        {
            label4.Text = trackBar1.Value.ToString();
            StepCrp = trackBar1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(ChangeCR == false && ChangeLan == false)
            {
                MessageBox.Show("Будь - ласка оберіть Тип шифрування або Мову шифру");
            }
            else
            {
                if (richTextBox1.Text == "")
                {
                    MessageBox.Show("Введіть текст для шифрування!!!");
                }
                else
                {
                    CaesarCipher ab = new CaesarCipher();
                    if (Language && Crypto) // укр мова розшифрувати
                    {
                        richTextBox2.Text = ab.UkrDecrypt(richTextBox1.Text, StepCrp);
                    }
                    else if (!Language && Crypto)// англ мова розшифрувати
                    {
                        richTextBox2.Text = ab.EngDecrypt(richTextBox1.Text, StepCrp);
                    }
                    else if (!Language && !Crypto)// англ мова зашифрувати
                    {
                        richTextBox2.Text = ab.EngEncrypt(richTextBox1.Text, StepCrp);
                    }
                    else if (Language && !Crypto)// укр мова зашифрувати
                    {
                        
                        richTextBox2.Text = ab.UkrEncrypt(richTextBox1.Text, StepCrp);
                    }
                }
                
            }

            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Crypto = true;
            ChangeCR = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Crypto = false;
            ChangeCR = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Language = true;
            ChangeLan = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Language = false;
            ChangeLan = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ChangeCR = false;
            ChangeLan = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox2.Text;
            richTextBox2.Text = "";
        }

        private void зберегтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog oSaveFileDialog = new SaveFileDialog();
            oSaveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (oSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = oSaveFileDialog.FileName;
                string extesion = Path.GetExtension(fileName);
                string fullPath = Path.GetFullPath(fileName);
                switch (extesion)
                {
                    case ".txt":
                        File.WriteAllText(fullPath, richTextBox1.Text);
                        break;
                    default://do something here
                        break;
                }
            }
        }

        private void друкToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox1.Text, new Font("Times New Roman", 14), Brushes.Black, new PointF(100, 100));
        }

        private void методГрубоїСилиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DelBruteForceAlgo testCallback = delegate (ref char[] inputs)
            {
                var str = new string(inputs);
                return (str == richTextBox2.Text);
            };


            int result = BruteForce(richTextBox1.Text, 1, richTextBox1.Text.Length, testCallback);
            int myRes = MyBruteForce(richTextBox1.Text, testCallback);
            MessageBox.Show("Result - " + result + "\n\r" + "My result - " + myRes);
        }
    }
}
