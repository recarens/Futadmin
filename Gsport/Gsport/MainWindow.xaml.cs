using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        Gsport.efadbDataSet efadbDataSet;
        Gsport.efadbDataSetTableAdapters.jugadorsTableAdapter efadbDataSetjugadorsTableAdapter;
        Gsport.efadbDataSetTableAdapters.usuarisTableAdapter efadbDataSetusuarisTableAdapter;
        System.Windows.Data.CollectionViewSource jugadorsViewSource;
        Gsport.efadbDataSetTableAdapters.equipsTableAdapter efadbDataSetequipsTableAdapter;
        System.Windows.Data.CollectionViewSource equipsViewSource;
        Gsport.efadbDataSetTableAdapters.entrenadorsTableAdapter efadbDataSetentrenadorsTableAdapter;
        System.Windows.Data.CollectionViewSource entrenadorsViewSource;
        string rutaImg;
        int codiUsuari;
        public MainWindow()
        {
            InitializeComponent(); 
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ca-ES");
            idiomes.WrapperIdiomes.ChangeCulture(Thread.CurrentThread.CurrentUICulture);
            tbSexe.Items.Add("Masculi");
            tbSexe.Items.Add("Femeni");
            tbSexe.SelectedIndex = 0;
        }

        private void btnCerca_Click(object sender, RoutedEventArgs e)
        {
            wndCercar wnd = new wndCercar();
        }

        private void btnAfageix_Click(object sender, RoutedEventArgs e)
        {
            GridLength gE2 = new GridLength(0, GridUnitType.Star);
            cdequips.Width = gE2;
            GridLength gJ2 = new GridLength(this.ActualWidth,GridUnitType.Star);
            cdjugadors.Width = gJ2;
            GridLength gC2 = new GridLength(0, GridUnitType.Star);
            cdntrenadors.Width = gC2;

            
        }

        private void btnAfageixEntrenador_Click(object sender, RoutedEventArgs e)
        {
            GridLength gE2 = new GridLength(0, GridUnitType.Star);
            cdequips.Width = gE2;
            GridLength gJ2 = new GridLength(0, GridUnitType.Star);
            cdjugadors.Width = gJ2;
            GridLength gC2 = new GridLength(this.ActualWidth, GridUnitType.Star);
            cdntrenadors.Width = gC2;
        }

        private void btnCrearEquip_Click(object sender, RoutedEventArgs e)
        {
            GridLength gE2 = new GridLength(this.ActualWidth, GridUnitType.Star);
            cdequips.Width = gE2;
            GridLength gJ2 = new GridLength(0, GridUnitType.Star);
            cdjugadors.Width = gJ2;
            GridLength gC2 = new GridLength(0, GridUnitType.Star);
            cdntrenadors.Width = gC2;
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
            efadbDataSet = ((Gsport.efadbDataSet)(this.FindResource("efadbDataSet")));
            // Cargar datos en la tabla jugadors. Puede modificar este código según sea necesario.
            efadbDataSetjugadorsTableAdapter = new Gsport.efadbDataSetTableAdapters.jugadorsTableAdapter();
            efadbDataSetjugadorsTableAdapter.Fill(efadbDataSet.jugadors);
            efadbDataSetusuarisTableAdapter = new Gsport.efadbDataSetTableAdapters.usuarisTableAdapter();
            efadbDataSetusuarisTableAdapter.Fill(efadbDataSet.usuaris);
            jugadorsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("jugadorsViewSource")));
            //jugadorsViewSource.View.MoveCurrentToFirst();
            wndLogin wnd = new wndLogin(efadbDataSet);
            wnd.ShowDialog();
            codiUsuari = wnd.codiUsuari;
            switch (wnd.privilegi)
            {
                case 1: //Jugadors
                    break;
                case 2: //Delegat
                    break;
                case 3: //Entrenador
                    btnAfageix.IsEnabled = true;
                    break;
                case 4: //Coordinador
                    btnAfageix.IsEnabled = true;
                    btnCrearEquip.IsEnabled = true;
                    btnNovaTemporada.IsEnabled = true;
                    btnAfageixEntrenador.IsEnabled = true;
                    break;
                case 5: //Gsport Admin
                    btnAfageix.IsEnabled = true;
                    btnCrearEquip.IsEnabled = true;
                    btnNovaTemporada.IsEnabled = true;
                    btnAfageixEntrenador.IsEnabled = true;
                    break;
            }
            // Cargar datos en la tabla equips. Puede modificar este código según sea necesario.
            efadbDataSetequipsTableAdapter = new Gsport.efadbDataSetTableAdapters.equipsTableAdapter();
            efadbDataSetequipsTableAdapter.Fill(efadbDataSet.equips);
            equipsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("equipsViewSource")));
            equipsViewSource.View.MoveCurrentToFirst();
            // Cargar datos en la tabla entrenadors. Puede modificar este código según sea necesario.
            efadbDataSetentrenadorsTableAdapter = new Gsport.efadbDataSetTableAdapters.entrenadorsTableAdapter();
            efadbDataSetentrenadorsTableAdapter.Fill(efadbDataSet.entrenadors);
            entrenadorsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("entrenadorsViewSource")));
            entrenadorsViewSource.View.MoveCurrentToFirst();
            // Cargar datos en la tabla jugador_temporada. Puede modificar este código según sea necesario.
            Gsport.efadbDataSetTableAdapters.jugador_temporadaTableAdapter efadbDataSetjugador_temporadaTableAdapter = new Gsport.efadbDataSetTableAdapters.jugador_temporadaTableAdapter();
            efadbDataSetjugador_temporadaTableAdapter.Fill(efadbDataSet.jugador_temporada);
            System.Windows.Data.CollectionViewSource jugador_temporadaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("jugador_temporadaViewSource")));
            jugador_temporadaViewSource.View.MoveCurrentToFirst();
            // Cargar datos en la tabla posicions. Puede modificar este código según sea necesario.
            Gsport.efadbDataSetTableAdapters.posicionsTableAdapter efadbDataSetposicionsTableAdapter = new Gsport.efadbDataSetTableAdapters.posicionsTableAdapter();
            efadbDataSetposicionsTableAdapter.Fill(efadbDataSet.posicions);
            System.Windows.Data.CollectionViewSource posicionsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("posicionsViewSource")));
            posicionsViewSource.View.MoveCurrentToFirst();
            // Cargar datos en la tabla divisio. Puede modificar este código según sea necesario.
            Gsport.efadbDataSetTableAdapters.divisioTableAdapter efadbDataSetdivisioTableAdapter = new Gsport.efadbDataSetTableAdapters.divisioTableAdapter();
            efadbDataSetdivisioTableAdapter.Fill(efadbDataSet.divisio);
            System.Windows.Data.CollectionViewSource divisioViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("divisioViewSource")));
            divisioViewSource.View.MoveCurrentToFirst();
            // Cargar datos en la tabla categories. Puede modificar este código según sea necesario.
            Gsport.efadbDataSetTableAdapters.categoriesTableAdapter efadbDataSetcategoriesTableAdapter = new Gsport.efadbDataSetTableAdapters.categoriesTableAdapter();
            efadbDataSetcategoriesTableAdapter.Fill(efadbDataSet.categories);
            System.Windows.Data.CollectionViewSource categoriesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("categoriesViewSource")));
            categoriesViewSource.View.MoveCurrentToFirst();
        }

        private void btnDadesEquip_Click(object sender, RoutedEventArgs e)
        {
            if (wpDadesEquip.Height > 0 || wpDadesEquip.Height.Equals(Double.NaN))
            {
                wpDadesEquip.Height = 0;
            }
            else
            {
                wpDadesEquip.Height = Double.NaN; // aixo es  height l'auto del xaml
                jugadorsDataGrid.Width = Double.NaN;
            }
        }

        private void btnDadesEntrenador_Click(object sender, RoutedEventArgs e)
        {
            if (wpDadesEntrenador.Height > 0 || wpDadesEntrenador.Height.Equals(Double.NaN))
            {
                wpDadesEntrenador.Height = 0;
            }
            else
            {
                wpDadesEntrenador.Height = Double.NaN; // aixo es  height l'auto del xaml
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (cdjugadors.Width.Value > 0)
            {
                DataRow dr = efadbDataSet.Tables["jugadors"].NewRow();
                dr["dni"] = tbDni.Text.Trim();
                dr["nom"] = tbNom.Text.Trim();
                dr["cognoms"] = tbCognom.Text.Trim();
                dr["sexe"] = Convert.ToInt16(tbSexe.SelectedIndex);
                dr["nomImatge"] = rutaImg;
                dr["data_inscripcio"] = DateTime.Parse(dpAnyInscripcio.SelectedDate.ToString());
                dr["data_naixement"] = DateTime.Parse(dpAnyNeixament.SelectedDate.ToString());
                dr["tarjeta_sanitaria"] = tbTarjetaSanitaria.Text.Trim();
                dr["malaltia_alergia"] = tbMalalties.Text.Trim();
                dr["mobil"] = tbMobil.Text.Trim();
                dr["telefon"] = tbTelefon.Text.Trim();
                dr["correu_electronic"] = tbcorreuElec.Text.Trim();
                dr["numero_soci"] = tbnSoci.Text.Trim();
                dr["lateralitat"] = tbLateralitat.Text.Trim();
                dr["edat"] = Convert.ToInt32(tbEdat.Text.Trim());
                dr["id_posicio"] = Convert.ToInt32(tbposicioNom.SelectedValue);
                dr["id_equip"] = Convert.ToInt32(cbequip.SelectedValue);
                try
                {
                    efadbDataSet.jugadors.Rows.Add(dr);
                    efadbDataSetjugadorsTableAdapter.Update(dr);
                    MessageBox.Show("Guardat");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No s'ha pogut guardar:" + ex.ToString());
                }
            }
            if(cdntrenadors.Width.Value > 0)
            {
                DataRow dr = efadbDataSet.Tables["entrenadors"].NewRow();
                dr["dni"] = dniTextBox.Text.Trim();
                dr["nom"] = nomTextBox1.Text.Trim();
                dr["cognom"] = cognomTextBox.Text.Trim();
                dr["data_naixement"] = DateTime.Parse(data_naixementDatePicker.SelectedDate.ToString());
                try
                {
                    efadbDataSet.entrenadors.Rows.Add(dr);
                    efadbDataSetentrenadorsTableAdapter.Update(dr);
                    MessageBox.Show("Guardat");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No s'ha pogut guardar:" + ex.ToString());
                }
            }
            if (cdequips.Width.Value > 0)
            {
                DataRow dr = efadbDataSet.Tables["equips"].NewRow();
                dr["nom"] = nomTextBox.Text.Trim();
                dr["id_divisio"] = Convert.ToInt32(id_divisioComboBox.SelectedValue);
                dr["id_categoria"] = Convert.ToInt32(id_categoriaComboBox.SelectedValue);
                dr["id_entrenador"] = Convert.ToInt32(id_entrenadorComboBox.SelectedValue);
                dr["puntuacio"] = puntuacioTextBox.Text.Trim();
                try
                {
                    efadbDataSet.equips.Rows.Add(dr);
                    efadbDataSetequipsTableAdapter.Update(dr);
                    MessageBox.Show("Guardat");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No s'ha pogut guardar:" + ex.ToString());
                }
            }
        }

        private void tbDni_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool trobat = false;
            btnGuardar.IsEnabled = true;
            if (tbDni.Text.Length > 8)
            {
                
                foreach (DataRow dr in efadbDataSet.Tables["jugadors"].Rows)
                {
                    if (dr["dni"].ToString().ToLower() == tbDni.Text.Trim().ToLower())
                    {
                        trobat = true;
                    }
                }

                if (trobat)
                {
                    MessageBox.Show("Ja existeix");
                    btnGuardar.IsEnabled = false;
                }
            }
            else
            {
                btnGuardar.IsEnabled = false;
            }
        }

        private void dniTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool trobat = false;
            btnGuardarEntrenador.IsEnabled = true;
            if (dniTextBox.Text.Length > 8)
            {

                foreach (DataRow dr in efadbDataSet.Tables["entrenadors"].Rows)
                {
                    if (dr["dni"].ToString().ToLower() == dniTextBox.Text.Trim().ToLower())
                    {
                        trobat = true;
                    }
                }

                if (trobat)
                {
                    MessageBox.Show("Ja existeix");
                    btnGuardarEntrenador.IsEnabled = false;
                }
            }
            else
            {
                btnGuardarEntrenador.IsEnabled = false;
            }
        }

        private void nomTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(nomTextBox.Text.Length > 0)
                btnGuardarEquip.IsEnabled = true;
            else
                btnGuardarEquip.IsEnabled = false;
        }

        private void imgImatge_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                imgImatge.Source = new BitmapImage(new Uri(filename));
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                Guid photoID = System.Guid.NewGuid();
                encoder.Frames.Add(BitmapFrame.Create((BitmapImage)imgImatge.Source));
                rutaImg = @"C:/Fotos/" + tbNom.Text.ToLower().Trim()+"img.jpg";
                using (FileStream filestream = new FileStream(rutaImg, FileMode.Create))
                    encoder.Save(filestream);
            }
        }
    }
}
