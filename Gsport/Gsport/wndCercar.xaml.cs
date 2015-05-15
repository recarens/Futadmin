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
    /// Lógica de interacción para wndCercar.xaml
    /// </summary>
    public partial class wndCercar : Window
    {
        efadbDataSet dataSetAux = new efadbDataSet();
        DataView dvEquips;
        DataView dvEntrenadors;
        DataView dvJugadors;
        DataView dvEquipsRivals;
        public int id;
        public string queEs = "";
        public wndCercar()
        {
            InitializeComponent();
        }
        public wndCercar(efadbDataSet dataSet)
        {
            cbCategoriaCerca = new ComboBox();
            dgResultat = new DataGrid();
            InitializeComponent();
            dataSetAux = dataSet;          
        }

        private void rbJugador_Checked(object sender, RoutedEventArgs e)
        {
            cbCategoriaCerca.Items.Clear();
            cbCategoriaCerca.IsEnabled = true;
            cbCategoriaCerca.Items.Add("DNI");
            cbCategoriaCerca.Items.Add("NOM");
            cbCategoriaCerca.Items.Add("COGNOM");
            cbCategoriaCerca.SelectedIndex = 0;
            dvJugadors = new DataView(dataSetAux.jugadors);
            dgResultat.ItemsSource = dvJugadors;
            foreach (DataGridColumn col in dgResultat.Columns)
            {
                if (col.DisplayIndex < 1 || col.DisplayIndex > 3)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 3)
                    col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            
        }

        private void rbEntrenador_Checked(object sender, RoutedEventArgs e)
        {
            cbCategoriaCerca.Items.Clear();
            cbCategoriaCerca.IsEnabled = true;
            cbCategoriaCerca.Items.Add("DNI");
            cbCategoriaCerca.Items.Add("NOM");
            cbCategoriaCerca.Items.Add("COGNOM");
            cbCategoriaCerca.SelectedIndex = 0;
            dvEntrenadors = new DataView(dataSetAux.entrenadors);
            dgResultat.ItemsSource = dvEntrenadors;
            foreach (DataGridColumn col in dgResultat.Columns)
            {
                if (col.DisplayIndex == 0)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 5)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 6)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 4)
                    col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void rbEquip_Checked(object sender, RoutedEventArgs e)
        {
            cbCategoriaCerca.Items.Clear();
            cbCategoriaCerca.IsEnabled = false;
            dvEquips = new DataView(dataSetAux.equips);
            dgResultat.ItemsSource = dvEquips;
            foreach (DataGridColumn col in dgResultat.Columns)
            {
                if (col.DisplayIndex == 0)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 2)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 3)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 4)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 6)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 7)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 8)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 5)
                    col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
        private void rbEquipRivals_Checked(object sender, RoutedEventArgs e)
        {
            cbCategoriaCerca.Items.Clear();
            cbCategoriaCerca.IsEnabled = false;
            dvEquipsRivals = new DataView(dataSetAux.equips_rivals);
            dgResultat.ItemsSource = dvEquipsRivals;
            foreach (DataGridColumn col in dgResultat.Columns)
            {
                if (col.DisplayIndex == 0)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 3)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 5)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 6)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 4)
                    col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
        private void tbBusca_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(rbEntrenador.IsChecked == true)
            {
                if (cbCategoriaCerca.SelectedIndex == 0)
                    dvEntrenadors.RowFilter = "dni LIKE '%" + tbBusca.Text.Trim().ToLower() + "%'";
                else if(cbCategoriaCerca.SelectedIndex == 1)
                    dvEntrenadors.RowFilter = "nom LIKE '%" + tbBusca.Text.Trim().ToLower() + "%'";
                else if (cbCategoriaCerca.SelectedIndex == 2)
                    dvEntrenadors.RowFilter = "cognom LIKE '%" + tbBusca.Text.Trim().ToLower() + "%'";
                dgResultat.ItemsSource = dvEntrenadors;
            }
            else if(rbJugador.IsChecked == true)
            {
                if (cbCategoriaCerca.SelectedIndex == 0)
                    dvJugadors.RowFilter = "dni LIKE '%" + tbBusca.Text.Trim().ToLower() + "%'";
                else if (cbCategoriaCerca.SelectedIndex == 1)
                    dvJugadors.RowFilter = "nom LIKE '%" + tbBusca.Text.Trim().ToLower() + "%'";
                else if (cbCategoriaCerca.SelectedIndex == 2)
                    dvJugadors.RowFilter = "cognoms LIKE '%" + tbBusca.Text.Trim().ToLower() + "%'";
                dgResultat.ItemsSource = dvJugadors;
            }
            else if(rbEquip.IsChecked == true)
            {
                dvEquips.RowFilter = "nom LIKE '%"+tbBusca.Text.Trim().ToLower()+"%'";
                dgResultat.ItemsSource = dvEquips;
            }
            else if(rbEquipRivals.IsChecked == true)
            {
                dvEquipsRivals.RowFilter = "nom LIKE '%" + tbBusca.Text.Trim().ToLower() + "%'";
                dgResultat.ItemsSource = dvEquipsRivals;
            }
        }
        private void dgResultat_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataRowView dtr = (DataRowView)dgResultat.SelectedItem;
            try
            {
                id = Convert.ToInt32(dtr.Row.ItemArray[0]);
                if (rbEntrenador.IsChecked == true)
                    queEs = "entrenador";
                else if (rbJugador.IsChecked == true)
                    queEs = "jugador";
                else if (rbEquip.IsChecked == true)
                    queEs = "equip";
                else if (rbEquipRivals.IsChecked == true)
                    queEs = "equiprival";
                this.Close();
            }
            catch
            {

            }
        }

        private void cbCategoriaCerca_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbBusca.Text = "";
        }
    }
}
