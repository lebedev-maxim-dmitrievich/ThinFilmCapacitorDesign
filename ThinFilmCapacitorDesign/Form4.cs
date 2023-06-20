using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThinFilmCapacitorDesign
{
    public partial class Form4 : Form
    {

        private DatabaseManager databaseManager;

        private string DielectricMaterial;
        private string CoverMaterial;
        private double CoverResistivity;
        private double CapacitanceDensity;
        private double ElectricStrength;
        private double DielectricPermittivity;
        private double TgDelta;
        private double WorkingFrequency;
        private double TCE;

        public Form4()
        {
            InitializeComponent();
            databaseManager = new DatabaseManager();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            textBox1.PlaceholderText = "Материал диэлектрика";
            textBox2.PlaceholderText = "Материал обкладок";
            textBox3.PlaceholderText = "Удельное сопротивление обкладок (Ом/см)";
            textBox4.PlaceholderText = "Удельная ёмкость (пФ/мм^2)";
            textBox5.PlaceholderText = "Электр.прочность *10^6 (В/см)";
            textBox6.PlaceholderText = "Диэлектрическая проницаемость";
            textBox7.PlaceholderText = "Тангенс угла диэлектрических потерь";
            textBox8.PlaceholderText = "Рабочая частота (МГц)";
            textBox9.PlaceholderText = "ТКЕ*10^-4 (1/град)";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DielectricMaterial = textBox1.Text;
                CoverMaterial = textBox2.Text;
                CoverResistivity = double.Parse(textBox3.Text);
                CapacitanceDensity = double.Parse(textBox4.Text);
                ElectricStrength = double.Parse(textBox5.Text);
                DielectricPermittivity = double.Parse(textBox6.Text);
                TgDelta = double.Parse(textBox7.Text);
                WorkingFrequency = double.Parse(textBox8.Text);
                TCE = double.Parse(textBox9.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка при вводе числового значения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Прерывание выполнения метода
            }

            databaseManager.InsertData(DielectricMaterial, CoverMaterial, CoverResistivity, CapacitanceDensity, ElectricStrength, DielectricPermittivity, TgDelta, WorkingFrequency, TCE);
            Form1 form1 = new Form1();
            Form2 form2 = new Form2();
            this.Close();
        }
    }
}
