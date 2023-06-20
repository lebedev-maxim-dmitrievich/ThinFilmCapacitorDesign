using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace ThinFilmCapacitorDesign
{
    public partial class Form1 : Form
    {
        //Вводит пользователь
        private double C;
        private double Yc;
        private double Urab;
        private double Frab;
        private double tgdelta;
        private double Yc1 = 15;
        private double Tmax;

        //Материал
        long id;
        string DielectricMaterial;
        string CoverMaterial;
        double CoverResistivity;
        double CapacitanceDensity;
        double ElectricStrength;
        double DielectricPermittivity;
        double TgDelta;
        double WorkingFrequency;
        double TKE;

        //Константы
        double Yc0 = 10;
        double Kz = 2;
        double epsil0 = 8.85 + Math.Pow(10, -3);
        double deltaL = 0.01;
        double Smin = 1;
        double Ycstar = 1;
        double Kf = 1;
        double q = 0.2;
        double f = 0.1;


        public Form1()
        {
            InitializeComponent();
            DatabaseManager databaseManager = new DatabaseManager();
            databaseManager.CreateDatabase();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.PlaceholderText = "C - номинальная ёмкость (пФ)";
            textBox2.PlaceholderText = "γC - для конденсатора с дискретной подгонкой (%)";
            textBox3.PlaceholderText = "Uраб - рабочее напряжение (В)";
            textBox4.PlaceholderText = "fmax - max рабочая частота (Гц)";
            textBox5.PlaceholderText = "tgδ  - тангенс угла диэлектрических потерь";
            textBox6.PlaceholderText = "γC1 - для конденсатора простой формы (%)";
            textBox7.PlaceholderText = "Tmax - max допустимая температура (градус)";

            // Создание экземпляра DatabaseManager
            DatabaseManager dbManager = new DatabaseManager();
            // Получение всех данных из таблицы
            DataTable dataTable = dbManager.GetAllData();

            // Проверка наличия данных
            if (dataTable.Rows.Count > 0)
            {
                // Проход по данным и добавление их в comboBox
                foreach (DataRow row in dataTable.Rows)
                {
                    string dielectricMaterial = row["DielectricMaterial"].ToString();

                    // Добавление значения в comboBox
                    comboBox1.Items.Add(dielectricMaterial);
                }

                // Установка выбранного элемента 
                comboBox1.SelectedIndex = 0;
            }

            // Установка предустановленного текста в comboBox
            comboBox1.Text = "Выберите материал";

            // Блокировка comboBox для предотвращения редактирования
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            // Привязка события SelectedIndexChanged
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                C = double.Parse(textBox1.Text);
                Yc = double.Parse(textBox2.Text);
                Urab = double.Parse(textBox3.Text);
                Frab = double.Parse(textBox4.Text);
                tgdelta = double.Parse(textBox5.Text);
                Yc1 = double.Parse(textBox6.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка при вводе числового значения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // return; // Прерывание выполнения метода
            }


            // Создаем экземпляр Form3
            Form3 form3 = new Form3();

            TKE *= Math.Pow(10, -4);
            WorkingFrequency *= Math.Pow(10, 6);
            Frab*= Math.Pow(10, 3);

            if (WorkingFrequency < Frab)
            {
                MessageBox.Show("Максимальная рабочая частота привышает рабочую частоту материала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Прерывание выполнения метода
            }

            double dmin = Kz * (Urab / ElectricStrength);
            if (dmin < 0.1)
            {
                dmin = 0.1;
            }

            double C0v = (DielectricPermittivity * epsil0 * Math.Pow(10, 3)) / dmin;
            double YCt = TKE * (Tmax - 20) * 100;

            double Ysdop = (Yc1 - Yc0 - YCt - Ycstar);
            if (Ysdop < 0)
            {
                MessageBox.Show("Допустимая погрешность площади конденсатора меньше нуля, необходимо выбрать другой материал. Либо увеличить значение относительного отклонения номинального значения ёмкости для конденсатора простой формы ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Прерывание выполнения метода
            }

            double C0to4n = C * Math.Pow(((Ysdop / 100) / (2 * deltaL)), 2);

            double C0min = CapacitanceDensity * Smin;
            double C0 = Math.Min(C0v, Math.Min(C0min, C0to4n));
            double d = epsil0 * DielectricPermittivity / C0;
            d *= Math.Pow(10, -3);
            d = Math.Round(d, 5);

            double K = 0;
            if ((C / C0) >= 5)
            {
                K = 1;
            }
            else if ((C / C0) < 5 && (C / C0) >= 1)
            {
                K = 1.3 - 0.06 * (C / C0);
            }
            else
            {
                MessageBox.Show("Краевой эффект меньше единицы, невозможно спроектировать конденсатор. Необходимо выбрать другой материал, либо увеличить номиналь", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Прерывание выполнения метода
            }

            double S = C / (C0 * K);
            double L = Math.Round(Math.Sqrt(S), 1);
            double B = L;

            double Ln = L + 2 * q;
            double Bn = Ln;

            double Ld = Ln + 2 * f;
            double Bd = Ld;
            double Sd = Ld * Bd;

            double w = 2 * 3.14 * Frab;
            double tgOB = (2 / 3) * w * CoverResistivity * C * Math.Pow(10, -12);
            double tgRAB = TgDelta + tgOB;
            if (tgdelta < tgRAB)
            {
                MessageBox.Show("tgDelta меньше возможного рабочего", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Прерывание выполнения метода
            }

            double Ysrab = deltaL * ((1 + Kf) / Math.Sqrt(Kf * Smin));
            if (Ysdop <= Ysrab)
            {
                MessageBox.Show("Выберите другой материал", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Прерывание выполнения метода
            }

            double Ys = deltaL / Math.Sqrt(S) * (Math.Sqrt(Kf) + 1 / Math.Sqrt(Kf));
            double n = Math.Ceiling((Yc0 + Ys) / (Yc - Ycstar));

            double Cmin = C * (1 - Yc / 100);
            C0v = DielectricPermittivity * epsil0 / (d * Math.Pow(10, -3));
            double Cmin0 = C / (3 * (2 * n - 1));
            C0 = Math.Min(C0v, Cmin0);
            C0min = C0 * (1 - Yc0 / 100);

            double Smax = Cmin / C0min;
            double C0max = C0 * (1 + Yc0 / 100);
            double Cmax1 = Smax * C0max;

            double Cmax = C * (1 + Yc / 100);
            double deltaC = Cmax1 - Cmax;
            double Cc = deltaC / n;
            double Sc = Cc / C0max;
            if (Sc < 1) { Sc = 1; }
            double Sosn = Smax - n * Sc;


            double L1 = Math.Round(Math.Sqrt(((deltaL / 2) * (deltaL / 2)) * (1 + Kf) * (1 + Kf) + Sosn * Kf) - deltaL / 2 * (1 + Kf), 2);
            double B1 = Math.Round(Math.Sqrt(((deltaL / 2) * (deltaL / 2)) * ((1 + Kf) * (1 + Kf)) / (Kf * Kf) + Sosn / Kf) - (deltaL / 2) * (1 + Kf), 2);

            double S1 = (Smax - Sosn) / n;
            double L2 = Math.Sqrt(S1);
            double B2 = Math.Round(L2, 2);



            double m = 0;
            if (n >= 5)
            {
                m = 1;
            }
            else if (n < 5 && n >= 1)
            {
                m = 2;
            }

            double Ln1 = L1 + L2 * 2 / m;
            double Bn1 = Ln1;

            double Ld1 = Math.Round(Ln1 + 2 * f, 2);
            double Bd1 = Ld1;
            double Skond = Math.Round(Ld1 * Bd1, 2);


            // Устанавливаем значение result в свойство Form3
            form3.Result = d;
            form3.Sosn = Skond;
            form3.n = n;
            form3.Lc = L2;
            form3.Bc = B2;
            form3.Ld = Ld1;
            form3.Bd = Bd1;
            form3.Lv = L1;
            form3.Bv = B1;
            form3.Ln = Ln1;
            form3.Bn = Bn1;


            // Открываем Form3
            form3.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Form2 form2 = new Form2(this);
            form2.ShowDialog();

        }

        public void SetRowValues(object[] values)
        {
            // Делай что-то с полученными значениями ячеек из Form2
            // Например, выводи их в MessageBox

            id = (long)values[0];
            DielectricMaterial = (string)values[1];
            CoverMaterial = (string)values[2];
            CoverResistivity = (double)values[3];
            CapacitanceDensity = (double)values[4];
            ElectricStrength = (double)values[5];
            DielectricPermittivity = (double)values[6];
            TgDelta = (double)values[7];
            WorkingFrequency = (double)values[8];
            TKE = (double)values[9];


            string rowValues = string.Join(", ", values);
            label2.Text = DielectricMaterial;
            comboBox1.Text = DielectricMaterial;

            label4.Text = CoverMaterial.ToString();
            label5.Text = CoverResistivity.ToString();
            label6.Text = CapacitanceDensity.ToString();
            label7.Text = ElectricStrength.ToString();
            label8.Text = DielectricPermittivity.ToString();
            label9.Text = WorkingFrequency.ToString();
            label10.Text = TKE.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Получение выбранного значения из comboBox
            string selectedMaterial = comboBox1.SelectedItem.ToString();

            // Создание экземпляра DatabaseManager
            DatabaseManager dbManager = new DatabaseManager();

            // Получение данных по выбранному материалу
            DataTable dataTable = dbManager.GetMaterialData(selectedMaterial);

            // Проверка наличия данных
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];

                // Заполнение переменных
                id = Convert.ToInt64(row["Id"]);
                DielectricMaterial = row["DielectricMaterial"].ToString();
                CoverMaterial = row["CoverMaterial"].ToString();
                CoverResistivity = Convert.ToDouble(row["CoverResistivity"]);
                CapacitanceDensity = Convert.ToDouble(row["CapacitanceDensity"]);
                ElectricStrength = Convert.ToDouble(row["ElectricStrength"]);
                DielectricPermittivity = Convert.ToDouble(row["DielectricPermittivity"]);
                TgDelta = Convert.ToDouble(row["TgDelta"]);
                WorkingFrequency = Convert.ToDouble(row["WorkingFrequency"]);
                TKE = Convert.ToDouble(row["TKE"]);
            }

            label2.Text = DielectricMaterial;

            label4.Text = CoverMaterial.ToString();
            label5.Text = CoverResistivity.ToString() + " Ом/см";
            label6.Text = CapacitanceDensity.ToString() + " пФ/мм^2";
            label7.Text = ElectricStrength.ToString() + " В/см";
            label8.Text = DielectricPermittivity.ToString();
            label9.Text = TgDelta.ToString();
            label10.Text = WorkingFrequency.ToString() + " МГц";
            label19.Text = TKE.ToString() + "*10^-4, 1/град";
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
    }
}