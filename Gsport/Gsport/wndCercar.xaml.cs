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
        DataView dvPartits;
        DataTable dt = new DataTable();
        public int id;
        public string queEs = "";
        public DataRow drSelect;
        public wndCercar()
        {
            InitializeComponent();
        }
        public wndCercar(efadbDataSet dataSet,bool convocatoria)
        {
            cbCategoriaCerca = new ComboBox();
            dgResultat = new DataGrid();
            InitializeComponent();
            dataSetAux = dataSet;
            if(convocatoria)
            {
                rbEntrenador.IsEnabled = false;
                rbJugador.IsEnabled = false;
                rbEquip.IsEnabled = false;
                rbEquipRivals.IsEnabled = false;
                rbPartits.IsChecked = true;
            }

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

        private void rbPartits_Checked(object sender, RoutedEventArgs e)
        {
            cbCategoriaCerca.Items.Clear();
            cbCategoriaCerca.Items.Add("Jornada");
            cbCategoriaCerca.Items.Add("Rival");
            cbCategoriaCerca.SelectedIndex = 0;
            dvPartits = new DataView(dataSetAux.partits);
            dt = dvPartits.ToTable();
            dt.Columns.Add("nomEquip", typeof(String));
            dt.Columns.Add("nomEquipRival", typeof(String));
            dt.Columns.Add("SocLocal", typeof(String));
            bool trobatEquip = false;
            int i = 0;
            bool trobatEquipRival = false;
            int j = 0;
            foreach (DataRow dr in dt.Rows)
            {   
                while (!trobatEquip && dataSetAux.equips.Rows.Count > i)
                {
                    if(Convert.ToInt32(dr["id_equip"]) == Convert.ToInt32(dataSetAux.equips.Rows[i]["id_equip"]))
                    {
                        trobatEquip = true;
                        dr["nomEquip"] = dataSetAux.equips.Rows[i]["nom"];
                    }
                    else
                        i++;
                }
                while (!trobatEquipRival && dataSetAux.equips_rivals.Rows.Count > j)
                {
                    if (Convert.ToInt32(dr["id_equip_rival"]) == Convert.ToInt32(dataSetAux.equips_rivals.Rows[j]["id_equip_rival"]))
                    {
                        trobatEquipRival = true;
                        dr["nomEquipRival"] = dataSetAux.equips_rivals.Rows[j]["nom"];
                    }
                    else
                        j++;
                }
                trobatEquip = false;
                i = 0;
                j = 0;
                trobatEquipRival = false;
                if (Convert.ToInt32(dr["visitant"]) == 0)
                    dr["SocLocal"] = "Si";
                else
                    dr["SocLocal"] = "No";
            }
            dgResultat.ItemsSource = dt.DefaultView;
            foreach (DataGridColumn col in dgResultat.Columns)
            {
                if (col.DisplayIndex >= 0 && col.DisplayIndex < 3 || col.DisplayIndex >= 5 && col.DisplayIndex < 7)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 3)
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
            else if(rbPartits.IsChecked == true)
            {
                if (cbCategoriaCerca.SelectedIndex == 0)
                {

                    if (tbBusca.Text.Trim().ToLower().Length > 0)
                        dt.DefaultView.RowFilter = "jornada = " + tbBusca.Text.Trim().ToLower();
                    else
                        dt.DefaultView.RowFilter = "jornada > 0";
                }
                else
                    dt.DefaultView.RowFilter = "nomEquipRival LIKE '%" + tbBusca.Text.Trim().ToLower() + "%'";
                dgResultat.ItemsSource = dt.DefaultView;
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
                else if(rbPartits.IsChecked == true)
                {
                    queEs = "partit";
                    drSelect = dtr.Row;
                }
                this.Close();
            }
            catch//Ho deixo aixi per si fan misclick
            {

            }
        }
        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (cbCategoriaCerca.SelectedIndex == 0 && rbPartits.IsChecked == true)
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cbCategoriaCerca_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbBusca.Text = "";
        }
    }
}
