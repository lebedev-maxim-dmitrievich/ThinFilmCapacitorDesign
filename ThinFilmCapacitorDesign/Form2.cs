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
    public partial class Form2 : Form
    {
        private DatabaseManager databaseManager;
        private Form1 form1;

        public Form2(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;

            databaseManager = new DatabaseManager();

            // Добавление элемента DataGridView в коллекцию Controls формы
            Controls.Add(dataGridView1);

            // Загрузка и отображение данных из базы данных
            LoadData();
        }

        public Form2()
        {
            InitializeComponent();

            databaseManager = new DatabaseManager();

            // Добавление элемента DataGridView в коллекцию Controls формы
            Controls.Add(dataGridView1);

            // Загрузка и отображение данных из базы данных
            LoadData();
        }

        private void LoadData()
        {
            // Получение данных из базы данных
            DataTable dataTable = databaseManager.GetAllData();

            // Назначение DataTable в качестве источника данных для элемента DataGridView
            dataGridView1.DataSource = dataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.ShowDialog();

            databaseManager = new DatabaseManager();

            // Добавление элемента DataGridView в коллекцию Controls формы
            Controls.Add(dataGridView1);

            // Загрузка и отображение данных из базы данных
            LoadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что индекс строки действителен и не равен -1 (не выбран заголовок столбца)
            if (e.RowIndex >= 0 && e.RowIndex != dataGridView1.NewRowIndex)
            {
                // Получаем выбранную строку
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Получаем значения всех ячеек выбранной строки
                object[] cellValues = selectedRow.Cells.Cast<DataGridViewCell>()
                                                        .Select(cell => cell.Value)
                                                        .ToArray();

                form1.SetRowValues(cellValues);

                // Закрываем форму
                this.Close();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
