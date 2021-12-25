using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int count = 0;
        private void DrawHeatMap(double p, int l)
        {

            DGV.Rows.Clear();
            DGV.Columns.Clear();
            bool[,] data = Program.Percolate(l, p);
            var marks = Program.Marking(data);
            int maxRow = data.GetLength(0);
            int maxCol = data.GetLength(1);

            int rowHeight = DGV.ClientSize.Height / maxRow - 1;
            int colWidth = DGV.ClientSize.Width / maxCol - 1;
            for (int c = 0; c < maxRow+1; c++)
            {
                DGV.Columns.Add(c.ToString(), "");
                DGV.Columns[c.ToString()].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            for (int c = 0; c < maxRow; c++)
                DGV.Columns[c].Width = colWidth;

            DGV.Rows.Add(maxRow + 5);
            for (int r = 0; r < maxRow; r++) DGV.Rows[r].Height = rowHeight;

            for (int i = 0; i < maxRow; i++)
            {
                for (int j = 0; j < maxCol; j++)
                {

                    if (data[i, j])
                    {
                        DGV[j, i].Style.BackColor = Color.Orchid;
                        
                    }
                    else
                        DGV[j, i].Style.BackColor = Color.Gray;                }
            }
            for (int i = 0; i < maxRow; i++)
            {
                for (int j = 0; j < maxCol; j++)
                { DGV[j, i].Value = marks[i, j]; }
            }
                    DGV.DefaultCellStyle.SelectionBackColor = BackColor;
            var lst = Program.CountGiantClasters(marks);

            textBox3.Text = " Вероятность принадлежности к соединяющему кластеру "+ Program.BelongToGiantClasterProbability(marks).ToString();
            textBox5.Text = " Средний размер кластера " + Program.AvaregeClasterSize(marks).ToString();

            List<Color> baseColors = new List<Color>();
            baseColors.Add(Color.Orange);
            baseColors.Add(Color.Red);
            baseColors.Add(Color.LightSkyBlue);
            baseColors.Add(Color.LightGreen);
            baseColors.Add(Color.Yellow);
            
            baseColors.Add(Color.RoyalBlue);
            int count = 0;
            foreach (var item in lst)
            {
                for (int i = 0; i < maxRow; i++)
                {
                    for (int j = 0; j < maxCol; j++)
                    {
                        //var a = DGV[j - 1, i].Value;
                        if (marks[i, j] == item)
                        {
                            DGV[j, i].Style.BackColor = baseColors[count];
                        }
                    }
                }
                count++;
            }
            dataGridView1.Rows.Clear();
            var clasterSizes = Program.GetSizesbyCount(marks);
            var tempDict = new Dictionary<int, int>();
            dataGridView1.Rows.Add(clasterSizes.Count());
            int k = 0;

            foreach (var pair in clasterSizes)
            {

                dataGridView1[0, k].Value = pair.Key;
                dataGridView1[1, k].Value = pair.Value;
                k++;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Перколировать_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
            var l = int.Parse(textBox4.Text);
            var p = (double)((Button)sender).Tag;
          //  if (checkBox1.Checked)
              //  count += 1;
            p += count * 0.0025;

            DrawHeatMap(p, l);
            textBox2.Visible = true;
            textBox2.Text = "p = " + p.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            var p = double.Parse(textBox1.Text);
            button1.Tag = p;
            textBox1.TextAlign = HorizontalAlignment.Center;
            button1.Visible = true;

            count = 0;
            textBox2.Visible = true;
            textBox2.Text = "p = " + p.ToString();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Visible = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
