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
        efadbDataSet dataSetAux = new efadbDataSet();
        System.Windows.Data.CollectionViewSource entrenadorsViewSource;
        public wndCercar()
        {
            InitializeComponent();
        }
        public wndCercar(efadbDataSet dataSet)
        {
            dgResultat = new DataGrid();
            InitializeComponent();
            dataSetAux = dataSet;
            
            
        }

        private void rbJugador_Checked(object sender, RoutedEventArgs e)
        {
            dgResultat.ItemsSource = dataSetAux.jugadors.DefaultView;
            foreach (DataGridColumn col in dgResultat.Columns)
            {
                if (col.DisplayIndex == 0)
                {
                    //DataGridTemplateColumn col1 = new DataGridTemplateColumn();
                    //col1.Header = "Foto";
                    //FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(Image));
                    //Binding b1 = new Binding(dataSetAux.jugadors.Rows["nomImatge"]);
                    //b1.Mode = BindingMode.TwoWay;
                    //factory1.SetValue(Image.SourceProperty, b1);
                    //DataTemplate cellTemplate1 = new DataTemplate();
                    //cellTemplate1.VisualTree = factory1;
                    //col1.CellTemplate = cellTemplate1;
                }

                if (col.DisplayIndex <= 1 && col.DisplayIndex >= 3)
                    col.Visibility = Visibility.Hidden;
                if (col.DisplayIndex == 3)
                    col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void rbEntrenador_Checked(object sender, RoutedEventArgs e)
        {
            dgResultat.ItemsSource = dataSetAux.entrenadors.DefaultView;
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
            dgResultat.ItemsSource = dataSetAux.equips.DefaultView;
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
        private void tbBusca_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void dgResultat_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Gsport.efadbDataSetTableAdapters.entrenadorsTableAdapter efadbDataSetentrenadorsTableAdapter = new Gsport.efadbDataSetTableAdapters.entrenadorsTableAdapter();
            efadbDataSetentrenadorsTableAdapter.Fill(dataSetAux.entrenadors);
            entrenadorsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("entrenadorsViewSource")));
        }
    }
}
