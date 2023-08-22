using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace redactorwpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void saveFile_Click(object sender, RoutedEventArgs e)
        {
            save_file();
        }

        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? res = openFileDialog.ShowDialog();
            if (res != false)
            {
                Stream myStream;
                if ((myStream = openFileDialog.OpenFile()) != null)
                {
                    string file_name = openFileDialog.FileName;
                    string text_file = File.ReadAllText(file_name);
                    textBox.Text = text_file;
                }
            }
        }

        private void timesNewRoman_Click(object sender, RoutedEventArgs e)
        {
            textBox.FontFamily = new FontFamily("Times New Roman");
            timesNewRoman.IsCheckable = false;
        }

        private void arial_Click(object sender, RoutedEventArgs e)
        {
            textBox.FontFamily = new FontFamily("Arial");
            arial.IsCheckable = false;

        }

        private void selectfontsize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fontSize = selectfontsize.SelectedItem.ToString();
            fontSize = fontSize.Substring(fontSize.Length - 2);
            switch (fontSize)
            {
                case "10":
                    textBox.FontSize = 10;
                    break;
                case "12":
                    textBox.FontSize = 12;
                    break;
                case "14":
                    textBox.FontSize = 14;
                    break;
                case "16":
                    textBox.FontSize = 16;
                    break;
                case "20":
                    textBox.FontSize = 20;
                    break;
                case "26":
                    textBox.FontSize = 26;
                    break;
                case "34":
                    textBox.FontSize = 34;
                    break;

            }
        }

        private void createNewFile_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text != "")
            {
                save_file();
            }

            textBox.Text = "";
        }

        private void save_file()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            bool? res = saveFileDialog.ShowDialog();
            if (res != false)
            {
                using (Stream s = File.Open(saveFileDialog.FileName, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        sw.Write(textBox.Text);
                    }
                }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            Registration();


        }

        private void button_aut_Click(object sender, RoutedEventArgs e)
        {
            string connect_str = ConfigurationManager.AppSettings["connectionString"];
            SqlConnection sql = new SqlConnection(connect_str);

            try
            {
                if (sql.State == System.Data.ConnectionState.Closed)
                {
                    sql.Open();
                }
                string qwery = "SELECT COUNT(1) FROM Users WHERE login=@login AND password=@pass";
                SqlCommand sqlCom = new SqlCommand(qwery, sql);
                sqlCom.CommandType = System.Data.CommandType.Text;
                sqlCom.Parameters.Add("@login", login_user.Text);
                sqlCom.Parameters.Add("@pass", password_user.Text);

                int countUser = (int)(sqlCom.ExecuteScalar());
                if (countUser == 1)
                {
                    MessageBox.Show("Вы авторизовались");
                    redactor.Visibility = Visibility.Visible;
                    textBox.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Сперва зарегистрируйтесь");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { sql.Close(); }
            if (textBox.Visibility==Visibility.Visible && redactor.Visibility==Visibility.Visible)
            {
                auto.Visibility = Visibility.Hidden;
                reg.Visibility = Visibility.Hidden;
            }
        }
        private void Registration()
        {
            string connect_str = ConfigurationManager.AppSettings["connectionString"];
            SqlConnection sql = new SqlConnection(connect_str);
            try
            {
                if (sql.State == System.Data.ConnectionState.Closed)
                {
                    sql.Open();
                }
                string qwery = "SELECT COUNT(1) FROM Users WHERE login=@login AND password=@pass";
                SqlCommand sqlCom = new SqlCommand(qwery, sql);
                sqlCom.CommandType = System.Data.CommandType.Text;
                sqlCom.Parameters.Add("@login", loginfile.Text);
                sqlCom.Parameters.Add("@pass", passwordfile.Text);

                int countUser = (int)(sqlCom.ExecuteScalar());
                if (countUser == 0)
                {
                    qwery = "INSERT INTO Users(login, password) VALUES(@login,@pass)";
                    SqlCommand com = new SqlCommand(qwery, sql);
                    com.CommandType = System.Data.CommandType.Text;
                    com.Parameters.Add("@login", loginfile.Text);
                    com.Parameters.Add("@pass", passwordfile.Text);

                    com.ExecuteNonQuery();
                    MessageBox.Show("Мы добавили вас в базу даннах");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { sql.Close(); }
        }
        
    }
}
