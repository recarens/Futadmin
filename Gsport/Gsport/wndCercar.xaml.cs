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

namespace Gsport
{
    /// <summary>
    /// Lógica de interacción para wndCercar.xaml
    /// </summary>
    public partial class wndCercar : Window
    {
        efadbDataSet dataSetAux;
        public wndCercar()
        {
            InitializeComponent();
        }
        public wndCercar(efadbDataSet dataSet)
        {
            InitializeComponent();
            dataSetAux = dataSet;
        }
        private void rbEntrenador_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbJugador_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rbEntrenador_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rbEquip_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rbJugador_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbEquip_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbBusca_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dgResultat_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Gsport.efadbDataSet efadbDataSet = ((Gsport.efadbDataSet)(this.FindResource("efadbDataSet")));
            // Cargar datos en la tabla equips. Puede modificar este código según sea necesario.
            Gsport.efadbDataSetTableAdapters.equipsTableAdapter efadbDataSetequipsTableAdapter = new Gsport.efadbDataSetTableAdapters.equipsTableAdapter();
            efadbDataSetequipsTableAdapter.Fill(efadbDataSet.equips);
            System.Windows.Data.CollectionViewSource equipsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("equipsViewSource")));
            equipsViewSource.View.MoveCurrentToFirst();
            // Cargar datos en la tabla jugadors. Puede modificar este código según sea necesario.
            Gsport.efadbDataSetTableAdapters.jugadorsTableAdapter efadbDataSetjugadorsTableAdapter = new Gsport.efadbDataSetTableAdapters.jugadorsTableAdapter();
            efadbDataSetjugadorsTableAdapter.Fill(efadbDataSet.jugadors);
            System.Windows.Data.CollectionViewSource jugadorsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("jugadorsViewSource")));
            jugadorsViewSource.View.MoveCurrentToFirst();
        }
    }
}
