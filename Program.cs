using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab8
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public static bool[,] Percolate(int l, double p)
        {
            Random rnd = new Random();
            var area = new bool[l, l];
            for (int i = 0; i < area.GetLength(0); i++)
            {
                for (int j = 0; j < area.GetLength(1); j++)
                {
                    if (rnd.NextDouble() >= 1 - p)
                        area[i, j] = true;
                }
            }
              var marks = Marking(area);
              for (int i = 0; i < area.GetLength(0); i++)
              {
                  for (int j = 0; j < area.GetLength(1); j++)
                  {
                      Console.Write("{0}  ",area[i, j]);
                  }
                  Console.WriteLine();
              }
              
            return area;


        }

        public static int[,] Marking(bool[,] area)
        {
            int lbl = 0;
            var l = area.GetLength(0);
            var m = area.GetLength(1);
            var marks = new int[l + 1, m + 1];  //Тут пришлость написать ужасный костыль - добавить ряд нулей снизу и слева, чтобы одним циклом без кучи if реализовать Хошена-Копельмана
            var reversedNp = new int[l * m];
            var np = new int[l * m];
            for (int i = 0; i<np.Length; i++)
                np[i] = i;
            for (int i = l - 1; i >= 0; i--)
            {
                for (int j = 0; j < m; j++)
                {
                    if (area[i, j])
                    {
                        int left = marks[i, j];
                        int down = marks[i + 1, j + 1];
                        if (left == 0 && down == 0)
                        {
                            marks[i, j + 1] = ++lbl;
                        }
                        else if (left != 0 && down == 0)
                        {
                            marks[i, j + 1] = Find(np, left);
                        }
                        else if (left == 0 && down != 0)
                        {
                            marks[i, j + 1] = Find(np, down);
                        }
                        else if (left != 0 && down != 0)
                        {
                            int a = Find(np, Math.Max(left, down));
                            int b = Find(np, Math.Min(left, down));
                          
                            for (int k = 0; k<np.Length; k++)
                            {
                               if (np[k] == a)
                                    np[k] = b;
                            }
                            marks[i, j + 1] = Find(np, left);
                        }
                    }

                }
            }
              
                for (int i = l - 1; i >= 0; i--)
                {
                    for (int j = 0; j < m; j++)
                    {
                        marks[i, j + 1] = np[marks[i, j + 1]];
                    }
                }
            var Healthymarks = new int[l, m];
            for (int i = 0; i < l; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    Healthymarks[i, j - 1] = marks[i, j];
                }
            }

                    return Healthymarks;
            }

        private static int Find(int[] np, int item)
        {
            var item2 = item;
            while (np[item2] != item2)
                item2 = np[item2];
            while (np[item] != item)
            {
                int current = np[item];
                np[item] = item2;
                item = current;
            }
            return item2;

        }

        public static List<int> CountGiantClasters (int [,] marks)
        {
            var giantClasters = new List<int>();
            var n = marks.GetLength(0);
            var m = marks.GetLength(1);
            var pastMarks = new List<int>();
            for (int i = 0; i < n; i++)
            {
                int a = pastMarks.IndexOf(marks[n-1,i]);
                int b = marks[n - 1, i];
                if (b != 0 && a == -1)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (marks[n - 1, i] == marks[0, j])
                        {
                            giantClasters.Add(marks[n - 1, i]);
                            break;
                        }
                    }
                    pastMarks.Add(marks[n-1, i]);
                }
            }
            return giantClasters;
        }


        private static Dictionary<int, int> GetSizesbyMark(int[,] marks)
        {
            var sizeByMark = new Dictionary<int, int>();
            foreach (var mark in marks)
            {
                if (mark != 0)
                {
                    if (sizeByMark.ContainsKey(mark))
                        sizeByMark[mark]++;
                    else
                        sizeByMark.Add(mark, 1);
                }
            }
            return sizeByMark;
        }

        public static Dictionary<int, int> GetSizesbyCount(int[,] marks)
        {
            var sizeByMark = GetSizesbyMark(marks);
            var ClasterSizes = new Dictionary<int, int>();
            foreach (var pair in sizeByMark)
            {
                if ((ClasterSizes.ContainsKey(pair.Value)))
                    ClasterSizes[pair.Value]+=1;
                else
                    ClasterSizes.Add(pair.Value, 1);
            }

            return ClasterSizes;
        }


        public static double BelongToGiantClasterProbability (int[,] marks)
        {
            var giantClastersMarks = CountGiantClasters(marks);
            double p = 0;
            double count = 0;
            var ClasterSizes = GetSizesbyCount(marks);
            var sizeByMark = GetSizesbyMark(marks);
            foreach (var pair in ClasterSizes)
                count += pair.Key * pair.Value;
            foreach (var giantClasterMark in giantClastersMarks)
                p += sizeByMark[giantClasterMark] / (count);
            return p;
        }

        public static double AvaregeClasterSize (int[,] marks)
        {
            var count = 0;
            var ClasterSizes = GetSizesbyCount(marks);
            var sizeByMark = GetSizesbyMark(marks);
            foreach (var pair in ClasterSizes)
                count += pair.Key * pair.Value;
            return count / sizeByMark.Count();
        }
    }

}
