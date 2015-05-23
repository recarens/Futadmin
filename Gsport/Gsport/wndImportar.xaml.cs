using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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

namespace Gsport
{
    /// <summary>
    /// Interaction logic for wndImportar.xaml
    /// </summary>
    public partial class wndImportar : Window
    {
        string filename;
        string mySqlString = "Server=shz24.guebs.net;port=3306;user id=gsportse_remot;password=gsport123.;persistsecurityinfo=True;database=gsportse_efadb";
        MySqlConnection cnMySql;
        efadbDataSet dataSetAux= new efadbDataSet();
        public wndImportar(efadbDataSet dt)
        {
            InitializeComponent();
            cnMySql = new MySqlConnection(mySqlString);
            dataSetAux = dt;
        }

        private void btnImportJugadors_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.DefaultExt = ".xls";
            of.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            Nullable<bool> result = of.ShowDialog();
            if (result == true)
            {
                filename = of.FileName;
                string cadenaConexio = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + "; Extended Properties=Excel 12.0;";
                OleDbConnection conexio = new OleDbConnection(cadenaConexio);
                conexio.Open();
                cnMySql.Open();
                OleDbCommand com = new OleDbCommand("SELECT * FROM [jugadors$]", conexio);
                OleDbCommand count = new OleDbCommand("SELECT COUNT(dni) from [jugadors$]", conexio);
                IDataReader idr = com.ExecuteReader();
                try
                {
                    int intcount = Convert.ToInt32(count.ExecuteScalar());
                    pbProgres.Maximum = intcount;
                    while (idr.Read())
                    {
                        MySqlCommand cmd;
                        cmd = new MySqlCommand("Insertarjugador", cnMySql);
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (idr[0] != DBNull.Value)
                        {
                            cmd.Parameters.AddWithValue("indni", idr[0].ToString());
                            cmd.Parameters.AddWithValue("innom", idr[1].ToString());
                            cmd.Parameters.AddWithValue("incognoms", idr[2].ToString());
                            if(idr[3] != DBNull.Value)
                                cmd.Parameters.AddWithValue("insexe", Convert.ToInt32(idr[3]));
                            else
                                cmd.Parameters.AddWithValue("insexe", 0);
                            cmd.Parameters.AddWithValue("inimg", @"C:/Fotos/img.jpg");
                            if (idr[4] != DBNull.Value)
                                cmd.Parameters.AddWithValue("indata_inscripcio", DateTime.Parse(idr[4].ToString()));
                            else
                                cmd.Parameters.AddWithValue("indata_inscripcio", DateTime.Parse(DateTime.Now.ToShortDateString()));
                            if (idr[5] != DBNull.Value)
                                cmd.Parameters.AddWithValue("indata_naixement", DateTime.Parse(idr[5].ToString()));
                            else
                                cmd.Parameters.AddWithValue("indata_naixement", DateTime.Parse(DateTime.Now.ToShortDateString()));
                            cmd.Parameters.AddWithValue("intarjeta_sanitaria", idr[6].ToString());
                            cmd.Parameters.AddWithValue("inmalaltia_alergia", idr[7].ToString());
                            cmd.Parameters.AddWithValue("inmobil", idr[8].ToString());
                            cmd.Parameters.AddWithValue("intelefon", idr[9].ToString());
                            cmd.Parameters.AddWithValue("incorreu_electronic", idr[10].ToString());
                            cmd.Parameters.AddWithValue("innumero_soci", idr[11].ToString());
                            cmd.Parameters.AddWithValue("inlateralitat", idr[12].ToString());
                            if (idr[13] != DBNull.Value)
                                cmd.Parameters.AddWithValue("inedat", Convert.ToInt32(idr[13]));
                            else
                                cmd.Parameters.AddWithValue("inedat", 10);
                            if (idr[14] != DBNull.Value)
                                cmd.Parameters.AddWithValue("inid_posicio", Convert.ToInt32(idr[14]));
                            else
                                cmd.Parameters.AddWithValue("inid_posicio", 1);
                            if (idr[15] != DBNull.Value)
                                cmd.Parameters.AddWithValue("inid_equip", Convert.ToInt32(idr[15]));  
                            else
                                cmd.Parameters.AddWithValue("inid_equip",dataSetAux.Tables["equips"].Rows[0]["id_equip"]);
                            cmd.ExecuteNonQuery();
                            pbProgres.Value += 1;
                            this.UpdateLayout();
                        }
                    }
                    MessageBox.Show("Importat correctament");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Hi ha errors en el fitxer");
                    
                }
                idr.Close();
                conexio.Close();
                cnMySql.Close();
            }
        }
    }
}
