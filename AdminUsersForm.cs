using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Booking3
{
    public partial class AdminUsersForm : Form
    {
        public AdminUsersForm()
        {
            InitializeComponent();

            // заполнение comboBox1 доступными городами
            comboBox1.Items.Clear();
            MainForm.MySelect("SELECT DISTINCT City FROM users").ForEach((string userCity) =>
            {
                if (userCity.Trim() != "")
                    comboBox1.Items.Add(userCity);
            });

            // фильтрация без параметров
            ShowFilteredUsers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowFilteredUsers(comboBox1.Text, textBox1.Text, Convert.ToInt16(numericUpDown1.Value));
        }

        private void ShowFilteredUsers(string _city = "", string _username = "", short _age = -1)
        {
            try
            {
                string sqlCommand =
                    "SELECT Name, City, Age, Login, Password FROM users " +
                    "WHERE City LIKE '%" + _city + "%' AND Login LIKE '%" + _username + "%'";
                if (_age > -1)
                    sqlCommand += " AND Age = " + _age.ToString();

                System.Collections.Generic.List<string> sqlResult = MainForm.MySelect(sqlCommand);

                dataGridView1.Rows.Clear();
                for (int i = 0; i < sqlResult.Count; i += 5)
                {
                    string[] userDataArr = new string[5]
                    {
                        sqlResult[i+0], sqlResult[i+1], sqlResult[i+2], sqlResult[i+3], sqlResult[i+4]
                    };
                    dataGridView1.Rows.Add(userDataArr);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка!");
            }
        }
    }
}
