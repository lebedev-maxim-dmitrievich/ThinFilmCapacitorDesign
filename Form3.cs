using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThinFilmCapacitorDesign
{
    public partial class Form3 : Form
    {
        public double Result { get; set; }
        public double Sosn { get; set; }
        public double n { get; set; }
        public double Lc { get; set; }
        public double Bc { get; set; }

        public double Ld { get; set; }

        public double Bd { get; set; }
        public double Lv { get; set; }
        public double Bv { get; set; }
        public double Ln { get; set; }
        public double Bn { get; set; }

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label1.Text = "Толщина диэлектрика d: " + Result.ToString() + " мм";
            label2.Text = "S конденсатора: " + Sosn.ToString() + " мм^2";
            label3.Text = "Кол-во секций: " + n.ToString();
            label4.Text = "Длина одной секции Lc: " + Lc.ToString() + " мм";
            label5.Text = "Длина нижней обкладки Lн: " + Ln.ToString() + " мм";
            label6.Text = "Длина верхней обкладки Lв: " + Lv.ToString() + " мм";
            label7.Text = "Длина диэлектрика Lд: " + Ld.ToString() + " мм";
            label8.Text = "Ширина одной секции Bc: " + Bc.ToString() + " мм";
            label9.Text = "Ширина нижней обкладки Bн: " + Bn.ToString() + " мм";
            label10.Text = "Ширина верхней обкладки Bв: " + Bv.ToString() + " мм";
            label11.Text = "Ширина диэлектрика Bд: " + Bd.ToString() + " мм";



            // Проверяем значение переменной "n"
            if (n >= 1 && n <= 10)
            {
                string imageName = $"{n}.png"; // Формируем имя файла изображения
                string resourcePath = $"ThinFilmCapacitorDesign.source.{imageName}";

                // Загружаем изображение из внедренных ресурсов
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
                {
                    if (stream != null)
                    {
                        pictureBox1.Image = Image.FromStream(stream);
                    }
                }
            }
            else
            {
                // Выводим сообщение об ошибке, если значение "n" не соответствует ожидаемому диапазону
                MessageBox.Show("Неверное значение переменной 'n'");
            }
        }
    }
}