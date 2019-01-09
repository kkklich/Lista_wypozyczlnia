using System;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;



//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

namespace ListaWypozyczen
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn;
        SqlCommand query;
        SqlDataAdapter sqldatadapterklientow;
        DataTable tabelafilmy;
        DataTable tabelaklienci;
        DataTable tabelawyp;
        DataSet ds;



               int czaswyp;


        public MainWindow()
        {

            //tabelawyp = new DataTable("wypozyczenia");

            //DataColumn id_klienta = new DataColumn("id_klienta");
            //id_klienta.DataType = Type.GetType("System.Int32");

            //DataColumn id_filmu = new DataColumn("id_filmu");
            //id_filmu.DataType = Type.GetType("System.Int32");

            //DataColumn data_wypozyczenia = new DataColumn("data_wypozyczenia");
            //data_wypozyczenia.DataType = Type.GetType("System.DateTime");

            //DataColumn data_zwrotu = new DataColumn("data_zwrotu");
            //data_zwrotu.DataType = Type.GetType("System.DateTime");


            //tabelawyp.Columns.Add(id_klienta);
            //tabelawyp.Columns.Add(id_filmu);
            //tabelawyp.Columns.Add(data_wypozyczenia);
            //tabelawyp.Columns.Add(data_zwrotu);





            InitializeComponent();

            try
            {
                string connString = "Database=Wypozyczalnia;Data Source=localhost;Integrated Security=true";
                conn = new SqlConnection(connString);
                conn.Open();

                //wyswietlanie klientow
                Show_client();

                //wyswietlanie filmow
                Show_film();

            }
            catch(SqlException f)
            {
                MessageBox.Show(f.Message);
            }

        }


        private void Show_client()
        {
            //wyswietlanie klientow
            string polecenie = "select * from klienci";
            query = new SqlCommand(polecenie, conn);
            query.ExecuteNonQuery();

            sqldatadapterklientow = new SqlDataAdapter(query);
            tabelaklienci = new DataTable("klienci");
            sqldatadapterklientow.Fill(tabelaklienci);
            listViewklienci1.ItemsSource = tabelaklienci.DefaultView;
            sqldatadapterklientow.Update(tabelaklienci);

        }



        private void Show_film() 
        {
            //Wyswietlanie filmow
            string polecenie2 = "select * from filmy";
            SqlCommand command = new SqlCommand(polecenie2, conn);
            command.ExecuteNonQuery();

            SqlDataAdapter filmysqladapter = new SqlDataAdapter(command);
            tabelafilmy = new DataTable("filmy");
            filmysqladapter.Fill(tabelafilmy);
            listViewfilmy2.ItemsSource = tabelafilmy.DefaultView;
            filmysqladapter.Update(tabelafilmy);



        }



        private void Btnwypozycz1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int indexklienci = listViewklienci1.SelectedIndex;
                int indexfilmy = listViewfilmy2.SelectedIndex;

                
                czaswyp = 30;

                DateTime timeteraz = DateTime.Now;
                DateTime timezwrotu = timeteraz.AddDays(czaswyp);
                

                if (indexfilmy > -1 && indexklienci > -1)
                {
                    string imie = tabelaklienci.Rows[indexklienci]["imie"].ToString();
                    string film = tabelafilmy.Rows[indexfilmy]["tytul"].ToString();

                    string ostrzezenie = "Czy chcesz wypozyczyc klientowi " + imie + " film pt. " + film + "";

                    if (MessageBox.Show(ostrzezenie, "Warning ", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        string idklienta = tabelaklienci.Rows[indexklienci]["id"].ToString();
                        string idfilmu = tabelafilmy.Rows[indexfilmy]["id"].ToString();



                        SqlCommand zadanie = new SqlCommand();
                        string polecenie3 = "INSERT INTO wypozyczenia VALUES (@id_klienta, @id_filmu, @data_wypozyczenia, @data_zwrotu)";

                        zadanie.Parameters.AddWithValue("@id_klienta", idklienta);
                        zadanie.Parameters.AddWithValue("@id_filmu", idfilmu);
                        zadanie.Parameters.AddWithValue("@data_wypozyczenia", timeteraz);
                        zadanie.Parameters.AddWithValue("@data_zwrotu", timezwrotu);

                        zadanie.CommandText = polecenie3;
                        zadanie.Connection = conn;
                        zadanie.ExecuteNonQuery();

                    }
                }
                else
                {
                    
                    MessageBox.Show("Musisz wybrać klienta i/lub film","Warning");
                }

            }

            catch (SqlException f)
            {
                MessageBox.Show(f.Message);
            }

        }

       

        private void Pokaliste_Click(object sender, RoutedEventArgs e)
        {
            Window1 oknowyp = new Window1(conn);
            oknowyp.Show();

        }

        

    }
}
