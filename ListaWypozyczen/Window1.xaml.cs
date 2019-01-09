using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;


namespace ListaWypozyczen
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        SqlConnection conn;
        int dni;

        public Window1()
        {
            InitializeComponent();
        }

        public Window1(SqlConnection conn)
        {
            InitializeComponent();
            this.conn = conn;

            SqlCommand query;
            SqlDataAdapter sqldatawyp;
            DataTable tabelawyp;


            string polecenie = "select w.id_wypozyczenia,k.imie,f.tytul from wypozyczenia w join klienci k on w.id_klienta = k.id  join filmy f on w.id_filmu = f.id";
            query = new SqlCommand(polecenie, conn);
            query.ExecuteNonQuery();

            sqldatawyp = new SqlDataAdapter(query);
            tabelawyp = new DataTable("wypozyczenia");
            sqldatawyp.Fill(tabelawyp);
            listViewwyp.ItemsSource = tabelawyp.DefaultView;
            sqldatawyp.Update(tabelawyp);



        }

       
    }
}
