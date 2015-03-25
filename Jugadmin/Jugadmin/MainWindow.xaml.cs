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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using EstructuraClasses;
using MySql.Data.MySqlClient;
namespace Jugadmin
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string,Equip> equips;
        string nomBuscat;
        string nomEquipBuscat;
        public MainWindow()
        {
            InitializeComponent();
            equips = new Dictionary<string, Equip>();
            ConexioCargaEquips();
            ConexioCargaJugadors();
            spDadesPrincipal.Height = 0;
        }  
        private void ConexioCargaEquips()
        {

            MySqlConnection con = new MySqlConnection("server=localhost;port=3306;user=root;password=futadmin1;database=efadb;");
            MySqlDataReader rdr = null;
            try
            {
                con.Open();
                Equip eq;
                string stm = "Select * from Equips";
                MySqlCommand cmd = new MySqlCommand(stm, con);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    eq = new Equip(rdr.GetString("nom").ToLower(), rdr.GetInt32("tipus"), rdr.GetString("categoria")); //0 nom, 1 futbol 11 o 7 , 2 categoria
                    equips.Add(eq.Nom, eq);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message + " Equips");
            }
            finally
            {
                if (rdr != null)
                    rdr.Close();
                if (con != null)
                    con.Close();
            }
        }

        private void ConexioCargaJugadors()
        {
            MySqlConnection con = new MySqlConnection("server=localhost;port=3306;user=root;password=futadmin1;database=efadb;");
            MySqlDataReader rdr = null;
            try
            {
                con.Open();
                Jugador j;
                string stm = "Select * from Jugadors";
                MySqlCommand cmd = new MySqlCommand(stm, con);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    j = new Jugador(rdr.GetString("dni"));
                    j.NomImg = rdr.GetString("nom_imatge"); // es aixi?
                    j.Nom = rdr.GetString("nom");
                    j.Cognoms = rdr.GetString("cognoms");
                    j.Altura = Convert.ToDouble(rdr.GetDouble("altura"));
                    j.Pes = Convert.ToDouble(rdr.GetDouble("pes"));
                    j.NomEquip = rdr.GetString("nom_equip").ToLower();
                    if (rdr.GetString("posicio") == Posicio.Porter.ToString().ToLower())
                        j.P = Posicio.Porter;

                    else if (rdr.GetString("posicio") == Posicio.Defensa.ToString().ToLower())
                        j.P = Posicio.Defensa;

                    else if (rdr.GetString("posicio") == Posicio.Migcampista.ToString().ToLower())
                        j.P = Posicio.Migcampista;

                    else if (rdr.GetString("posicio") == Posicio.Davanter.ToString().ToLower())
                        j.P = Posicio.Davanter;
                    equips[j.NomEquip.ToLower()].Afegir(j);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message+" Jugadors");
            }
            finally
            {
                if (rdr != null)
                    rdr.Close();
                if (con != null)
                    con.Close();
            }
        }



        private void btnCerca_Click(object sender, RoutedEventArgs e)
        {
            wndCercar wc = new wndCercar(equips);
            wc.ShowDialog();
            nomBuscat = wc.nom;
            nomEquipBuscat = wc.nomEquip;
            spDadesPrincipal.Height = Double.NaN;
            foreach(Jugador j in equips[nomEquipBuscat])
            {
                if (j.Nom == nomBuscat)
                {
                    string ruta = System.IO.Path.GetFullPath("../../Fotos/" + j.NomImg);
   
                    Uri uri = new Uri(ruta,UriKind.Absolute); // per assignar l'arxiu a una imatge
                    BitmapImage bim = new BitmapImage(uri); // tenim la imatge 
                    imgImatge.Source = bim; //bit a bit fem la imatge en el image panel
                    imgImatge.Stretch = Stretch.Uniform;
                }
            }
        }

        private void btnSortir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDadesPersonals_Click(object sender, RoutedEventArgs e)
        {
            
            if(wpDadesPersonals.Height > 0 || wpDadesPersonals.Height.Equals(Double.NaN))
            {
                wpDadesPersonals.Height = 0;
            }
            else
            {
                wpDadesPersonals.Height = Double.NaN; // aixo es  height l'auto del xaml
            }
        }

        private void btnAfageix_Click(object sender, RoutedEventArgs e)
        {
            spDadesPrincipal.Height = Double.NaN;
        }
    }
}
