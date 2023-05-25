using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;
using System.Windows.Forms.DataVisualization.Charting;

namespace kursa4
{
    public partial class Form1 : Form
    {
        private SQLiteConnection SqlC;
        private DataTable dataTable;
        private DataTable dataTableDop;
        private DataTable dataTableStatic;
        public static double E = 0.0003;
        public static double A = 0.7;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button2.Enabled = false;
            button4.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            comboBox1.Enabled = false;
            checkedListBox1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            dataTable = new DataTable();
            fileDialog.Filter = "База данных (*.db;*.sqlite)|*.db;*.sqlite|Все файлы|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                SqlC = new SQLiteConnection($"Data Source={fileDialog.FileName}");
                SqlC.Open();
                LoadTable();
                DataTableDop();
                pictureBox1.Image = ReadPhoto();
                textBox1.Text = ReadT().ToString();
                textBox2.Text = ReadA().ToString();

                button3.Enabled = true;
                button2.Enabled = true;
                button4.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button8.Enabled = true;
                comboBox1.Enabled = true;
                checkedListBox1.Enabled = true;
                comboBox1.SelectedIndex = 0;
            }

        }

        private void LoadTable()
        {
            string SQLQuerry = "SELECT * FROM [Данные];";
            GetTable(SQLQuerry);
        }

        private void LoadTableStatic()
        {
            string SQLQuery2 = "Select * from [Точка]";
            GetTable(SQLQuery2);
        }

        private void LoadTableLine()
        {
            string SQLQuery3 = "select * from [Прямая]";
            GetTable(SQLQuery3);

        }

        private void LoadTableJump()
        {
            string SQLQuery4 = "select * from [Скачек]";
            GetTable(SQLQuery4);
        }

        private void LoadTableCyrcle()
        {
            string SQLQuery5 = "select * from [Цикл]";
            GetTable(SQLQuery5);
        }

        private void GetTable(string SQLQuerry) // заполнения таблицы по запросу
        {
            dataTable.Clear();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(SQLQuerry, SqlC);
            adapter.Fill(dataTable);

            baseTable.Columns.Clear();
            baseTable.Rows.Clear();

            string ColumnsName; //заполнения столбцов
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                ColumnsName = dataTable.Columns[i].ColumnName;
                baseTable.Columns.Add(ColumnsName, ColumnsName);
                baseTable.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            }

            for (int row = 0; row < dataTable.Rows.Count; row++) //заполнения строк
            {
                baseTable.Rows.Add(dataTable.Rows[row].ItemArray);
            }
        }

        private Image ReadPhoto()
        {
            byte[] PhotoByte;
            PhotoByte = (byte[]) dataTableDop.Rows[0].ItemArray[2];
            MemoryStream ms = new MemoryStream(PhotoByte);
            return Image.FromStream(ms);
        }

        public double ValueA;
        public double ValueT;

        private double ReadT()
        {
            ValueT = (double) dataTableDop.Rows[0].ItemArray[0];
            return ValueT;

        }

        private double ReadA()
        {

            ValueA = (double) dataTableDop.Rows[0].ItemArray[1];
            return ValueA;
        }


        private void DataTableDop()
        {
            string SQLQuerry = "SELECT * From [Доп];";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(SQLQuerry, SqlC);
            dataTableDop = new DataTable();
            adapter.Fill(dataTableDop);

        }

        private void ChangeT(double T)
        {
            string SQLQuerry = $"UPDATE [Доп] SET T = {T} WHERE T = {this.ReadT()};";
            SQLQuerry = SQLQuerry.Replace(',', '.');
            SQLiteCommand command = new SQLiteCommand(SQLQuerry, SqlC);
            command.ExecuteNonQuery();
            DataTableDop();
        }

        private void ChangeA(double T)
        {
            string SQLQuerry = $"UPDATE [Доп] SET T = {T} WHERE T = {this.ReadA()};";
            SQLQuerry = SQLQuerry.Replace(',', '.');
            SQLiteCommand command = new SQLiteCommand(SQLQuerry, SqlC);
            command.ExecuteNonQuery();
            DataTableDop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChangeT(Convert.ToDouble(textBox1.Text));
            try
            {
                ValueT = Convert.ToDouble(textBox1.Text);

                if (ValueT < 0 | ValueT > 1)
                {
                    MessageBox.Show("Число должно быть от нуля до еденицы!", "Ошибка", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Успешно добавлено!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Пожалуйста, введите корректные значения", "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex.Equals(-1))
            {
                
                MessageBox.Show("Выбирете элемент");
            }
            else if (comboBox1.SelectedIndex.Equals(0))
            {
                LoadTable();
                MessageBox.Show($"Вы выбрали элемент : {comboBox1.Items[comboBox1.SelectedIndex]}");
            }
            else if (comboBox1.SelectedIndex.Equals(1))
            {
                LoadTableStatic();
                MessageBox.Show($"Вы выбрали элемент : {comboBox1.Items[comboBox1.SelectedIndex]}");
            }
            else if (comboBox1.SelectedIndex.Equals(2))
            {
                LoadTableLine();
                MessageBox.Show($"Вы выбрали элемент : {comboBox1.Items[comboBox1.SelectedIndex]}");
            }
            else if (comboBox1.SelectedIndex.Equals(3))
            {
                LoadTableJump();
                MessageBox.Show($"Вы выбрали элемент : {comboBox1.Items[comboBox1.SelectedIndex]}");
            }
            else if (comboBox1.SelectedIndex.Equals(4))
            {
                LoadTableCyrcle();
                MessageBox.Show($"Вы выбрали элемент : {comboBox1.Items[comboBox1.SelectedIndex]}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ChangeA(Convert.ToDouble(textBox2.Text));
            try
            {
                ValueA = Convert.ToDouble(textBox2.Text);

                if (ValueA < 0 | ValueA > 1)
                {
                    MessageBox.Show("Число должно быть от нуля до еденицы!", "Ошибка", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Успешно добавлено!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Пожалуйста, введите корректные значения", "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }



        private DataTable Calc(DataTable dTable)
        {
            DataTable finalTable = new DataTable();

            finalTable.Columns.Add("Эпоха");
            finalTable.Columns.Add("M");
            finalTable.Columns.Add("Alpha");
            finalTable.Columns.Add("M пр.");
            finalTable.Columns.Add("Alpha пр.");
            finalTable.Columns.Add("M +");
            finalTable.Columns.Add("Alpha +");
            finalTable.Columns.Add("M -");
            finalTable.Columns.Add("Alpha -");
            finalTable.Columns.Add("Устойчивость");

            //Вычисление значений для графика фазовой траектории
           
            double _miZero = 0;
            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                finalTable.Rows.Add();
                finalTable.Rows[i][0] = dTable.Rows[i][0];

                double _mi = 0;
                for (int j = 1; j < dTable.Columns.Count; j++)
                {
                    _mi += Convert.ToDouble(dTable.Rows[i][j]) * Convert.ToDouble(dTable.Rows[i][j]);
                }

                _mi = Math.Sqrt(_mi);
                if (i == 0) _miZero = _mi;

                double _alfa = 0;
                for (int j = 1; j < dTable.Columns.Count; j++)
                {
                    _alfa += Convert.ToDouble(dTable.Rows[0][j]) * Convert.ToDouble(dTable.Rows[i][j]);
                }

                _alfa /= (_mi * _miZero);
                if (_alfa > 0.99999999999) _alfa = 1;

                _alfa = Math.Acos(_alfa);
                _alfa = _alfa * 206264.816;


                _mi = Math.Round(_mi, 6);
                _alfa = Math.Round(_alfa, 4);
                finalTable.Rows[i][1] = _mi;
                finalTable.Rows[i][2] = _alfa;
            }

            double _sumMi = 0;
            double _sumAlfa = 0;
            for (int i = 0; i < finalTable.Rows.Count; i++)
            {
                _sumMi += Convert.ToDouble(finalTable.Rows[i][1]);
                _sumAlfa += Convert.ToDouble(finalTable.Rows[i][2]);
            }

            double _miPredict = ValueA * _miZero + (1 - ValueA) * (_sumMi / finalTable.Rows.Count);
            double _alfaPredict = ValueA * Convert.ToDouble(finalTable.Rows[0][2]) +
                                  (1 - ValueA) * (_sumAlfa / finalTable.Rows.Count);
            finalTable.Rows[0][3] = Math.Round(_miPredict, 6);
            finalTable.Rows[0][4] = Math.Round(_alfaPredict, 4);

            for (int i = 1; i < finalTable.Rows.Count; i++)
            {
                _miPredict = ValueA * Convert.ToDouble(finalTable.Rows[i][1]) + (1 - ValueA) * _miPredict;
                finalTable.Rows[i][3] = Math.Round(_miPredict, 6);
                _alfaPredict = ValueA * Convert.ToDouble(finalTable.Rows[i][2]) + (1 - ValueA) * _alfaPredict;
                finalTable.Rows[i][4] = Math.Round(_alfaPredict, 4);
            }

            //вычисляем значения для верхнего предела
            double _miZeroTop = 0;
            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                double _miTop = 0;
                for (int j = 1; j < dTable.Columns.Count; j++)
                {
                    _miTop += Math.Pow(Convert.ToDouble(dTable.Rows[i][j]) + ValueT, 2);
                }

                _miTop = Math.Sqrt(_miTop);
                if (i == 0) _miZeroTop = _miTop;

                double _alTop = 0;
                for (int j = 1; j < dTable.Columns.Count; j++)
                {
                    _alTop += (Convert.ToDouble(dTable.Rows[0][j]) + ValueT) * (Convert.ToDouble(dTable.Rows[i][j]) + ValueT);
                }

                _alTop /= _miTop * _miZeroTop;
                if (_alTop > 0.99999999999) _alTop = 1;
                _alTop = Math.Acos(_alTop);
                _alTop = _alTop * 206264.816;

                _miTop = Math.Round(_miTop, 6);
                _alTop = Math.Round(_alTop, 4);
                finalTable.Rows[i][5] = _miTop;
                finalTable.Rows[i][6] = _alTop;
            }

            //вычисляем значения для нижнего предела
            double _miZeroBot = 0;
            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                double _miBot = 0;
                for (int j = 1; j < dTable.Columns.Count; j++)
                {
                    _miBot += Math.Pow(Convert.ToDouble(dTable.Rows[i][j]) - ValueT, 2);
                }

                _miBot = Math.Sqrt(_miBot);
                if (i == 0) _miZeroBot = _miBot;

                double _alBot = 0;
                for (int j = 1; j < dTable.Columns.Count; j++)
                {
                    _alBot += (Convert.ToDouble(dTable.Rows[0][j]) - ValueT) * (Convert.ToDouble(dTable.Rows[i][j]) - ValueT);
                }

                _alBot /= _miBot * _miZeroBot;
                if (_alBot > 0.99999999999) _alBot = 1;
                _alBot = Math.Acos(_alBot);
                _alBot = _alBot * 206264.816;


                _miBot = Math.Round(_miBot, 6);
                _alBot = Math.Round(_alBot, 4);
                finalTable.Rows[i][7] = _miBot;
                finalTable.Rows[i][8] = _alBot;
            }





            //определение устойчивости по эпохам
            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                double right =
                    Math.Abs(Convert.ToDouble(finalTable.Rows[i][5]) - Convert.ToDouble(finalTable.Rows[i][7])) / 2;
                double left =
                    Math.Abs(Convert.ToDouble(finalTable.Rows[i][1]) - Convert.ToDouble(finalTable.Rows[0][1]));

                if (left >= right) finalTable.Rows[i][9] = "Аварийное";
                else finalTable.Rows[i][9] = "Неаварийное";
            }

            return finalTable;
        }

        private void AddGraph(Chart chart, DataTable dTable, int col1, int col2, string name)
        {
            Series serie = new Series(name);

            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                serie.Points.AddXY(Convert.ToDouble(dTable.Rows[i][col1]), Convert.ToDouble(dTable.Rows[i][col2]));
                serie.Points[i].Label = $"{i}";
            }

            chart.Series.Add(serie);
        }

        private void ShowTable(DataTable DTable, DataGridView Table)
        {
            Table.Columns.Clear();
            Table.Rows.Clear();

            for (int col = 0; col < DTable.Columns.Count; col++)
            {
                string ColName = DTable.Columns[col].ColumnName;
                Table.Columns.Add(ColName, ColName);
                Table.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            
            for (int row = 0; row < DTable.Rows.Count; row++)
            {
                Table.Rows.Add(DTable.Rows[row].ItemArray);
            }

            
            foreach (DataGridViewColumn column in Table.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private DataTable Predict(DataTable data)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Эпоха");
            table.Columns.Add("M predict");
            table.Columns.Add("Alpha predict");
            table.Columns.Add("M+ pr.");
            table.Columns.Add("M- pr.");
            table.Columns.Add("Устойчивость");
            table.Rows.Add("r");



            double _miZero = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                table.Rows.Add();
                table.Rows[i][0] = data.Rows[i][0];

                double _mi = 0;
                for (int j = 1; j < data.Columns.Count; j++)
                {
                    _mi += Convert.ToDouble(data.Rows[i][j]) * Convert.ToDouble(data.Rows[i][j]);
                }

                _mi = Math.Sqrt(_mi);
                if (i == 0) _miZero = _mi;

                double _alfla = 0;
                for (int j = 1; j < data.Columns.Count; j++)
                {
                    _alfla += Convert.ToDouble(data.Rows[0][j]) * Convert.ToDouble(data.Rows[i][j]);
                }

                _alfla /= (_mi * _miZero);
                if (_alfla > 0.99999999999) _alfla = 1;

                _alfla = Math.Acos(_alfla);
                _alfla = _alfla * 206264.816;


                _mi = Math.Round(_mi, 6);
                _alfla = Math.Round(_alfla, 4);

            }

            double _sumMi = 0;
            double _sumAlfa = 0;
            for (int i = 0; i < table.Rows.Count - 1; i++)
            {
                _sumMi += Convert.ToDouble(Calc(dataTable).Rows[i][1]);
                _sumAlfa += Convert.ToDouble(Calc(dataTable).Rows[i][2]);
            }

            double _miPredict = ValueA * _miZero + (1 - A) * (_sumMi / data.Rows.Count);
            double _alfaPredict = ValueA * Convert.ToDouble(Calc(dataTable).Rows[0][2]) +
                                  (1 - ValueA) * (_sumAlfa / table.Rows.Count);
            table.Rows[0][1] = Math.Round(_miPredict, 6);
            table.Rows[0][2] = Math.Round(_alfaPredict, 4);

            for (int i = 1; i < table.Rows.Count - 1; i++)
            {
                _miPredict = A * Convert.ToDouble(Calc(dataTable).Rows[i][1]) + (1 - ValueA) * _miPredict;
                table.Rows[i][1] = Math.Round(_miPredict, 6);
                _alfaPredict = A * Convert.ToDouble(Calc(dataTable).Rows[i][2]) + (1 - ValueA) * _alfaPredict;
                table.Rows[i][2] = Math.Round(_alfaPredict, 4);
            }


            double _sumMiPredict = 0;
            for (int i = 0; i <= table.Rows.Count - 2; i++)
            {
                _sumMiPredict += Convert.ToDouble(table.Rows[i][1]);
            }



            //table.Rows[16][1] = (A * Convert.ToDouble(sumMiPr /table.Rows.Count)) + ((1 - A) * Convert.ToDouble(table.Rows[15][1]));

            var r1 = ValueA * Convert.ToDouble(_sumMiPredict / (table.Rows.Count - 1));
            var r2 = 1 - ValueA;
            var r3 = Convert.ToDouble(table.Rows[15][1]) * r2;
            var res = r1 + r3;

            table.Rows[16][1] = res;

            //mi top

            double _miZeroTop = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                double _miTop = 0;
                for (int j = 1; j < data.Columns.Count; j++)
                {
                    _miTop += Math.Pow(Convert.ToDouble(data.Rows[i][j]) + ValueT, 2);
                }

                _miTop = Math.Sqrt(_miTop);
                if (i == 0) _miZeroTop = _miTop;
            }


            double _sumMiTop = 0;
            for (int i = 0; i < table.Rows.Count - 1; i++)
            {
                _sumMiTop += Convert.ToDouble(Calc(dataTable).Rows[i][5]);
            }

            double _miTopPredict = ValueA * _miZeroTop + (1 - ValueA) * (_sumMiTop / data.Rows.Count);
            table.Rows[0][3] = Math.Round(_miTopPredict, 4);


            for (int i = 1; i < table.Rows.Count - 1; i++)
            {
                _miTopPredict = ValueA * Convert.ToDouble(Calc(dataTable).Rows[i][5]) + (1 - ValueA) * _miTopPredict;
                table.Rows[i][3] = Math.Round(_miTopPredict, 6);
            }

            double _sumMiTopPredict = 0;
            for (int i = 0; i <= table.Rows.Count - 2; i++)
            {
                _sumMiTopPredict += Convert.ToDouble(table.Rows[i][3]);
            }

            var e1 = ValueA * Convert.ToDouble(_sumMiTopPredict / (table.Rows.Count - 1));
            var e2 = 1 - ValueA;
            var e3 = Convert.ToDouble(table.Rows[15][3]) * e2;
            var resPlus = e1 + e3;
            resPlus = Math.Round(resPlus, 6);
            table.Rows[16][3] = resPlus;


            // mi bot
            double _miZeroBot = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                double _miBot = 0;
                for (int j = 1; j < data.Columns.Count; j++)
                {
                    _miBot += Math.Pow(Convert.ToDouble(data.Rows[i][j]) - ValueT, 2);
                }

                _miBot = Math.Sqrt(_miBot);
                if (i == 0) _miZeroBot = _miBot;
            }

            double _sumMiBot = 0;
            for (int i = 0; i < table.Rows.Count - 1; i++)
            {
                _sumMiBot += Convert.ToDouble(Calc(dataTable).Rows[i][7]);
            }

            double _miBotPredict = ValueA * _miZeroBot + (1 - ValueA) * (_sumMiBot / data.Rows.Count);
            table.Rows[0][4] = Math.Round(_miBotPredict, 4);


            for (int i = 1; i < table.Rows.Count - 1; i++)
            {
                _miBotPredict = ValueA * Convert.ToDouble(Calc(dataTable).Rows[i][7]) + (1 - ValueA) * _miBotPredict;
                table.Rows[i][4] = Math.Round(_miBotPredict, 6);
            }

            double _sumMiBotPredict = 0;
            for (int i = 0; i <= table.Rows.Count - 2; i++)
            {
                _sumMiBotPredict += Convert.ToDouble(table.Rows[i][4]);
            }

            var w1 = ValueA * Convert.ToDouble(_sumMiBotPredict / (table.Rows.Count - 1));
            var w2 = 1 - ValueA;
            var w3 = Convert.ToDouble(table.Rows[15][4]) * w2;
            var resMinus = w1 + w3;
            resMinus = Math.Round(resMinus, 6);
            table.Rows[16][4] = resMinus;
            table.Rows[16][0] = "Прогноз";



            for (int i = 0; i < data.Rows.Count+1; i++)
            {
                double right = Math.Abs(Convert.ToDouble(table.Rows[i][3]) - Convert.ToDouble(table.Rows[i][4])) / 2;
                double left = Math.Abs(Convert.ToDouble(table.Rows[i][1]) - Convert.ToDouble(table.Rows[0][1]));

                if (left >= right) table.Rows[i][5] = "Аварийно";
                else table.Rows[i][5] = "Неаварийно";
            }

            return table;
        }

        private void Decommposition(DataGridView DataGrid, Chart chart, DataTable dTable, CheckedListBox chBox,
            DataGridView dataGridPredict)
        {
            DataTable dataT = Calc(dTable);
            DataTable dataT2 = Predict(dTable);

            ShowTable(dataT2, dataGridPredict);
            ShowTable(dataT, DataGrid);

            chart.Series.Clear();
            chart.Legends.Clear();
            chBox.Items.Clear();

            chart.ChartAreas[0].AxisX.Title = "M";
            chart.ChartAreas[0].AxisY.Title = "A (сек)";
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:#.##}";
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "{0:#.####}";

            AddGraph(chart, dataT, 1, 2, "Фазовая траектория");
            AddGraph(chart, dataT, 3, 4, "Прогнозируемая траектория");
            AddGraph(chart, dataT, 5, 6, "Верхний предел");
            AddGraph(chart, dataT, 7, 8, "Нижний предел");

            //Выводим легенду, изменяем вид маркеров, добавляем серии в список
            foreach (Series serie in chart.Series)
            {
                serie.ChartType = SeriesChartType.Spline;
                chart.Legends.Add(serie.Name);
                chart.Legends[serie.Name].Docking = Docking.Bottom;
                chart.Series[serie.Name].MarkerBorderColor = System.Drawing.Color.Black;
                chart.Series[serie.Name].MarkerStyle = MarkerStyle.Circle;
                chart.Series[serie.Name].MarkerSize = 6;
                chart.Legends[serie.Name].Docking = Docking.Bottom;

                chBox.Items.Add(serie.Name);
            }

           

        }


        private void button6_Click(object sender, EventArgs e)
        {

            try
            {

                

                Decommposition(firstLevelTable, chart1, dataTable, checkedListBox1, predictTable);

                if(dataTable == null)
                {
                    MessageBox.Show("Пожалуйста, сначала подлючите таблицу", "Ошибка", MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                }

                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("Пожалуйста, сначала подлючите таблицу", "Ошибка", MessageBoxButtons.OK,
                      MessageBoxIcon.Error);

            }

                
                
                        
            

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                
                if (checkedListBox1.GetItemChecked(i))
                {
                    chart1.Series[Convert.ToString(checkedListBox1.Items[i])].Enabled = true;
                }
                else chart1.Series[Convert.ToString(checkedListBox1.Items[i])].Enabled = false;
            }

            if (checkedListBox1.CheckedItems.Count == 0)
            {
                chart1.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
                chart1.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }

        private void chart1_Click_1(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            chart2.Series.Clear();
            checkedListBox2.Items.Clear();

            chart2.ChartAreas[0].AxisX.Title = "Эпоха";
            chart2.ChartAreas[0].AxisY.Title = "Высота";
            chart2.ChartAreas[0].AxisY.LabelStyle.Format = "{0:#.####}";
            chart2.ChartAreas[0].AxisY.IsStartedFromZero = false;
            chart2.ChartAreas[0].AxisX.IsStartedFromZero = true;

            for (int i = 1; i < dataTable.Columns.Count; i++)
            {
                Series serie = new Series($"Марка {i}");
                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    serie.Points.AddXY($"{j}", Convert.ToDouble(dataTable.Rows[j][i]));
                    serie.Points[j].Label = $"{j}";
                    serie.Points[j].Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, FontStyle.Bold);
                }

                
                serie.ChartType = SeriesChartType.Spline;
                serie.MarkerBorderColor = System.Drawing.Color.Black;
                serie.MarkerStyle = MarkerStyle.Circle;
                serie.MarkerSize = 6;

                chart2.Series.Add(serie);
                checkedListBox2.Items.Add(serie.Name);

                for (int index = 0; index < checkedListBox2.Items.Count; index++)
                {
                    checkedListBox2.SetItemChecked(index, true);
                }

                checkedListBox2.SelectedIndex = -1; //"вызов" функции selectedIndexChanged
                checkedListBox2.SelectedIndex = 0;
            }
        }
       

        private void checkedListBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                if (checkedListBox2.GetItemChecked(i))
                {
                    chart2.Series[Convert.ToString(checkedListBox2.Items[i])].Enabled = true;
                }
                else chart2.Series[Convert.ToString(checkedListBox2.Items[i])].Enabled = false;
            }

            if (checkedListBox2.CheckedItems.Count == 0)
            {
                chart2.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
                chart2.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemChecked(i, false);
            }

            checkedListBox2.SelectedIndex = -1;
            checkedListBox2.SelectedIndex = 0;
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void firstLevelTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            this.textBox3.AutoSize = false;
            
        }
    }
}
