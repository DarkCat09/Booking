using System;
using System.Collections.Generic;
using System.Drawing;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Windows.Forms;

namespace Booking3
{
    public partial class MainForm : Form
    {
        public static string Login = "";

        /// <summary>
        /// Select-запрос. Возвращает список строк
        /// </summary>
        public static List<string> MySelect(string cmdText)
        {
            List<string> list = new List<string>();

            MySqlCommand cmd = new MySqlCommand(cmdText, Program.CONN);

            DbDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    list.Add(reader.GetValue(i).ToString());
            }

            reader.Close();

            return list;
        }

        /// <summary>
        /// Insert/Update/Delete-запрос
        /// </summary>
        public static void MyUpdate(string cmdText)
        {
            MySqlCommand cmd = new MySqlCommand(cmdText, Program.CONN);
            cmd.ExecuteReader();
            cmd.Dispose();
        }


        public MainForm()
        {
            InitializeComponent();
            Filter(null, null);   
            
            List<string> cities = MySelect("SELECT DISTINCT Name FROM cities ORDER BY Name");
            CityComboBox.Items.Clear();
            CityComboBox.Items.Add("");
            foreach (string city in cities)
                CityComboBox.Items.Add(city);
        }

        private void FilterButton_Click(object sender, EventArgs e)
        {
            if (FilterPanel.Size.Height < 50)
                FilterPanel.Size = new Size(FilterPanel.Size.Width, 120);
            else
                FilterPanel.Size = new Size(FilterPanel.Size.Width, FilterButton.Size.Height);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;            
            HotelForm hf = new HotelForm(pb.Tag.ToString());
            hf.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Label pb = (Label)sender;
            HotelForm hf = new HotelForm(pb.Tag.ToString());
            hf.Show();
        }

        /// <summary>
        /// ФИльтр
        /// </summary>
        private void Filter(object sender, EventArgs e)
        {
            HotelsPanel.Controls.Clear();
            string command = "SELECT id, Name, City, Image, Rating FROM hotels WHERE 1";
            if (CityComboBox.Text != "")
                command += " AND city = '" + CityComboBox.Text + "'";
            if (RatingComboBox.Text != "")
                command += " AND Rating >= " + RatingComboBox.Text;

            List<string> hotels = MySelect(command);

            int x = 15;
            for (int i = 0; i < hotels.Count; i += 5)
            {
                PictureBox pb = new PictureBox();
                try
                {
                    pb.Load("../../Pictures/" + hotels[i + 3]);
                }
                catch (Exception) { }
                pb.Location = new Point(x, 10);
                pb.Size = new Size(190, 120);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Tag = hotels[i];
                pb.Click += new EventHandler(pictureBox1_Click);
                HotelsPanel.Controls.Add(pb);

                Label lbl = new Label();
                lbl.Location = new Point(x, 140);
                lbl.Size = new Size(200, 30);
                lbl.Text = hotels[i + 1];
                lbl.Tag = hotels[i];
                lbl.Click += new EventHandler(label4_Click);
                HotelsPanel.Controls.Add(lbl);

                x += 205;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            AdminForm af = new AdminForm();
            af.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> user_data = MainForm.MySelect(
                "SELECT * FROM users WHERE Login = '" + LoginTextBox.Text + "'");

            //Авторизация успешна
            if (user_data.Count > 0)
            {
                Login = LoginTextBox.Text;
                AuthPanel.Controls.Clear();
                AuthPanel.Controls.Add(AdminButton);
                AuthPanel.Controls.Add(HelloLabel);
                HelloLabel.Text = "Привет, " + Login;
            }
        }
    }
}
