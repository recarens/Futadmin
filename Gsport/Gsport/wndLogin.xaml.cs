using System;
using System.Collections.Generic;
using System.Data;
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
    /// Lógica de interacción para wndLogin.xaml
    /// </summary>
    public partial class wndLogin : Window
    {
        efadbDataSet dataSetAux;
        public int codiUsuari;
        public int privilegi;
        public wndLogin()
        {
            InitializeComponent();
        }
        public wndLogin(efadbDataSet dataSet)
        {
            InitializeComponent();
            dataSetAux = dataSet;
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            bool trobat = false;
            foreach (DataRow Linia in dataSetAux.usuaris.Rows)
            {
                if (tbUsuari.Text.Trim() == Linia["usuari"].ToString().Trim())
                    if (pbContrasenya.Password.Trim() == Linia["contrasenya"].ToString().Trim())
                    {
                        trobat = true;
                        codiUsuari = (int)Linia["codi"];
                        privilegi = (int)Linia["privilegi"];
                    }
            }
            if (trobat)
            {
                this.Close();
            }
            else
                MessageBox.Show("No és correcte");
        }
    }
}
