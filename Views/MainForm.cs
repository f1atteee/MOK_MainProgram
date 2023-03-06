using System;
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
using System.Drawing.Imaging;
using Newtonsoft.Json;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MOK_MainInterface.Views
{

    public partial class MainForm : Form
    {

        public int StepCrp { get; set; }

        public bool Crypto { get; set; } // true розшифрувати; false зашифрувати
        public bool ChangeCR { get; set; }
        public bool ChangeLan { get; set; }
        public bool Language { get; set; }

        public bool ImageTrue { get; set; }

        public MainForm()
        {
            InitializeComponent();
            ImageTrue = false;
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
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter =
                   "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG|Text Files (*.txt)|*.TXT";
                dialog.InitialDirectory = "C:\\";
                dialog.Title = "Select your file";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string fname = dialog.FileName;
                    if (fname.Contains(".txt"))
                    {
                        richTextBox1.Text = File.ReadAllText(fname, Encoding.UTF8);
                        MessageBox.Show("Ви відкрили текстовий файл.");
                    }
                    else
                    {
                        string imagePath = dialog.FileName;
                        byte[] imageBytes = File.ReadAllBytes(imagePath);
                        string base64String = Convert.ToBase64String(imageBytes);
                        richTextBox1.Text = base64String;
                        MessageBox.Show("Ви відкрили зображення.");
                        ImageTrue = true;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Помилка - " + ex.Message);
            }

        }

        private void trackBar1_Scroll_1(object sender, EventArgs e)
        {
            label4.Text = trackBar1.Value.ToString();
            StepCrp = trackBar1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ImageTrue = false;
            if (ChangeCR == false && ChangeLan == false)
            {
                MessageBox.Show("Будь - ласка оберіть Тип шифрування або Мову шифру");
            }
            else
            {
                if(ImageTrue)
                {
                    byte[] base64Bytes = Convert.FromBase64String(richTextBox1.Text);
                    string plaintext = Encoding.UTF8.GetString(base64Bytes);
                }


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

        private void друкToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                if (e.Graphics != null)
                    e.Graphics.DrawString(richTextBox1.Text, new Font("Times New Roman", 14), Brushes.Black, new PointF(100, 100));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка - " + ex.Message);
            }

        }

        private void методГрубоїСилиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            CaesarCipher ab = new CaesarCipher();

            string encryptedText = richTextBox2.Text;

            if (string.IsNullOrEmpty(encryptedText))
            {
                MessageBox.Show("Текстове поле для шифрування порожнє", "Помилка");
                return;
            }

            for (int step = 1; step <= 1000; step++)
            {
                string decryptedText = ab.EngDecrypt(encryptedText, step);
                if (decryptedText == richTextBox1.Text)
                {
                    stopwatch.Stop();
                    MessageBox.Show($"Витрачений час: {stopwatch.ElapsedMilliseconds} мс, Ключ: {step}", "Атака 'грубою силою'");

                    richTextBox2.Text = decryptedText;
                    return;
                }
            }

            MessageBox.Show("Ключ не знайдено", "Помилка");
        }

        private void побудуватиЧастотніТаблиціToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form tableForm = new Form();
            tableForm.Text = "Частотна таблиця";
            tableForm.Size = new Size(500, 700);

            string fileName = "Dict.json";
            string directoryPath = @"E:\C# програмування\MOK_y2023\MOK_MainInterface\Files\";
            string filePath = Path.Combine(directoryPath, fileName);
            string json = File.ReadAllText(filePath);
            Dictionary<string, int> dict = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
            SortedDictionary<string, int> sortedDict = new SortedDictionary<string, int>(dict, new KeyComparer());

            DataGridView enTable = new DataGridView();
            DataTable table = new DataTable();

            table.Columns.Add("Ключ", typeof(string));
            table.Columns.Add("Частота", typeof(int));

            foreach (var item in sortedDict)
            {
                table.Rows.Add(item.Key, item.Value);
            }

            enTable.DataSource = table;
            enTable.Size = new Size(500, 700);
            tableForm.Controls.Add(enTable);
            tableForm.ShowDialog();
        }

        private void зображенняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Pictures (*.png)|*.png|Images (*.jpeg)|*.jpeg|All files (*.*)|*.*",
                Title = "Save image file"
            };
            try
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    byte[] ib = Convert.FromBase64String(richTextBox2.Text);
                    File.WriteAllBytes(sfd.FileName, ib);
                    MessageBox.Show("Зображення успішно збережено.", "Успіх");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Щось пішло не так. \n\nПовідомлення про помилку: {ex.Message}", "Помилка");
            }

        }

        private void текстToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog oSaveFileDialog = new SaveFileDialog();
                oSaveFileDialog.Filter = "TXT files (*.txt)|*.txt|All files (*.*)|*.*";
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
                        default:
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Помилка - " + ex.Message);
            }
        }

        public void WriteJsonFile(Dictionary<string, int> keyValuePairs, string filePath)
        {
            string json = JsonConvert.SerializeObject(keyValuePairs, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        class KeyComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return x.CompareTo(y);
            }
        }


        public static Dictionary<string, int> CountLetters(string input)
        {
            Dictionary<string, int> letterCounts = new Dictionary<string, int>();
            foreach (char c in input)
            {
                if (Char.IsLetter(c))
                {
                    if (letterCounts.ContainsKey(c.ToString()))
                    {
                        letterCounts[c.ToString()]++;
                    }
                    else
                    {
                        letterCounts.Add(c.ToString(), 1);
                    }
                }
            }
            return letterCounts;
        }


        private void атакаНаШифрToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
