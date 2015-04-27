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

namespace Gsport
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCerca_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnAfageix_Click(object sender, RoutedEventArgs e)
        {
            spDadesPrincipal.Height = Double.NaN;
        }

        private void btnAfageixEntrenador_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCrearEquip_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNovaTemporada_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSortir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDadesPersonals_Click(object sender, RoutedEventArgs e)
        {

            if (wpDadesPersonals.Height > 0 || wpDadesPersonals.Height.Equals(Double.NaN))
            {
                wpDadesPersonals.Height = 0;
            }
            else
            {
                wpDadesPersonals.Height = Double.NaN; // aixo es  height l'auto del xaml
            }

        }

        private void btnDadesTemporada_Click(object sender, RoutedEventArgs e)
        {
            if (wpDadesTemporada.Height > 0 || wpDadesTemporada.Height.Equals(Double.NaN))
            {
                wpDadesTemporada.Height = 0;
            }
            else
            {
                wpDadesTemporada.Height = Double.NaN; // aixo es  height l'auto del xaml
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Gsport.efadbDataSet efadbDataSet = ((Gsport.efadbDataSet)(this.FindResource("efadbDataSet")));
            // Cargar datos en la tabla jugadors. Puede modificar este código según sea necesario.
            Gsport.efadbDataSetTableAdapters.jugadorsTableAdapter efadbDataSetjugadorsTableAdapter = new Gsport.efadbDataSetTableAdapters.jugadorsTableAdapter();
            efadbDataSetjugadorsTableAdapter.Fill(efadbDataSet.jugadors);
            System.Windows.Data.CollectionViewSource jugadorsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("jugadorsViewSource")));
            jugadorsViewSource.View.MoveCurrentToFirst();
        }
    }
}
