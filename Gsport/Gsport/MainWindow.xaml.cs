using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
        Gsport.efadbDataSetTableAdapters.jugador_temporadaTableAdapter efadbDataSetjugador_temporadaTableAdapter;
        System.Windows.Data.CollectionViewSource jugador_temporadaViewSource;
        Gsport.efadbDataSetTableAdapters.temporadesTableAdapter efadbDataSettemporadesTableAdapter;
        System.Windows.Data.CollectionViewSource equips_rivalsViewSource;
        Gsport.efadbDataSetTableAdapters.equips_rivalsTableAdapter efadbDataSetequips_rivalsTableAdapter;
        Gsport.efadbDataSetTableAdapters.lesionsTableAdapter efadbDataSetlesionsTableAdapter;
        System.Windows.Data.CollectionViewSource jugadorslesionsViewSource;
        Gsport.efadbDataSetTableAdapters.partitsTableAdapter efadbDataSetpartitsTableAdapter;
        System.Windows.Data.CollectionViewSource partitsViewSource;
        Gsport.efadbDataSetTableAdapters.convocatoriesTableAdapter efadbDataSetconvocatoriesTableAdapter;
        System.Windows.Data.CollectionViewSource convocatoriesViewSource;
        
        string mySqlString = "Server=shz24.guebs.net;port=3306;user id=gsportse_remot;password=gsport123.;persistsecurityinfo=True;database=gsportse_efadb";
        MySqlConnection cnMySql;
        string rutaImg = @"C:/Fotos/img.jpg";
        int idTemporada = 1;
        int codiUsuari;
        int idCercat;
        string queEs;
        int posicioRow;
        bool objectaCercat = false;
        bool esborrat = false;
        int idEquipConvocatoria = 0;
        public MainWindow()
        {
            InitializeComponent();
            
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ca-ES");
            idiomes.WrapperIdiomes.ChangeCulture(Thread.CurrentThread.CurrentUICulture);
            data_IniciDatePicker.SelectedDate = DateTime.Now;
            data_FiDatePicker.SelectedDate = DateTime.Now;
            dpAnyNeixament.SelectedDate = DateTime.Now;
            tbSexe.Items.Add("M");
            tbSexe.Items.Add("F");
            cblocalVisitant.Items.Add("Local"); //0
            cblocalVisitant.Items.Add("Visitant"); //1
            for (int i = 0; i < 24;i++)
                cbhora.Items.Add(i);

            for (int i = 0; i < 60;i++)
                cbminut.Items.Add(i);

            tbSexe.SelectedItem = tbSexe.Items.GetItemAt(0);

            //crear  carpeta Fotos si no existeix
            if (Directory.Exists(@"C:/Fotos/") == false)
            {
                Directory.CreateDirectory(@"C:/Fotos");
                
            }

        }

        /// <summary>
        /// obre la finestra de cerca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCerca_Click(object sender, RoutedEventArgs e)
        {
            bool convocatoria = false;
            if (cdConvocatoria.Width.Value > 0)
                convocatoria = true;
            efadbDataSetjugadorsTableAdapter.Fill(efadbDataSet.jugadors);
            efadbDataSetentrenadorsTableAdapter.Fill(efadbDataSet.entrenadors);
            efadbDataSetequips_rivalsTableAdapter.Fill(efadbDataSet.equips_rivals);
            efadbDataSetequipsTableAdapter.Fill(efadbDataSet.equips);
            efadbDataSetpartitsTableAdapter.Fill(efadbDataSet.partits);
            wndCercar wnd = new wndCercar(efadbDataSet,false);
            wnd.ShowDialog();
            idCercat = wnd.id;
            if(wnd.queEs!="")
                queEs = wnd.queEs;
            if (idCercat != 0)
            {
                if (queEs == "jugador")
                {
                    bool trobat = false;
                    int i = 0;
                    while (!trobat && i < efadbDataSet.Tables["jugadors"].Rows.Count)
                    {
                        if (idCercat == Convert.ToInt32(efadbDataSet.Tables["jugadors"].Rows[i][0]))
                            trobat = true;
                        else
                            i++;
                    }
                    btnAfageix_Click(this, null);
                    objectaCercat = true;
                    btnDadesTemporada.IsEnabled = true;
                    btnDadesLesions.IsEnabled = true;
                    btnEsborrarJugadors.IsEnabled = true;
                    wpJugadors.DataContext = FindResource("jugadorsViewSource");
                    jugadorsViewSource.View.MoveCurrentToPosition(i);
                    posicioRow = i;
                    try
                    {
                        imgImatge.Source = new BitmapImage(new Uri(efadbDataSet.Tables["jugadors"].Rows[i]["nomImatge"].ToString()));//actualitza la foto.
                    }
                    catch
                    {
                        imgImatge.Source = new BitmapImage(new Uri(@"C:/Fotos/img.jpg"));
                    }
                    if (Convert.ToInt32(efadbDataSet.Tables["jugadors"].Rows[i]["sexe"]) == 0)
                        tbSexe.SelectedItem = "M";
                    else
                        tbSexe.SelectedItem = "F";
                    tbposicioNom.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["jugadors"].Rows[i]["id_posicio"]);
                    cbequip.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["jugadors"].Rows[i]["id_equip"]);
                    CarregarNomsTemporades();
                    tbDni.IsEnabled = false;
                    foreach (DataGridColumn col in lesionsDataGrid.Columns)
                    {
                        if (col.DisplayIndex == 3)
                            col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                    }   
                }
                else if (queEs == "equip")
                {
                    bool trobat = false;
                    int i = 0;
                    while (!trobat && i < efadbDataSet.Tables["equips"].Rows.Count)
                    {
                        if (idCercat == Convert.ToInt32(efadbDataSet.Tables["equips"].Rows[i][0]))
                            trobat = true;
                        else
                            i++;
                    }
                    btnCrearEquip_Click(this, null);
                    objectaCercat = true;
                    btnEsborrarEquips.IsEnabled = true;
                    wpPrincipalEquip.DataContext = FindResource("equipsjugadorsViewSource");
                    grid1.DataContext = FindResource("equipsViewSource1");
                    equipsViewSource.View.MoveCurrentToPosition(i);
                    posicioRow = i;
                    id_categoriaComboBox.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["equips"].Rows[i]["id_categoria"]);
                    id_divisioComboBox.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["equips"].Rows[i]["id_divisio"]);
                    id_entrenadorComboBox.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["equips"].Rows[i]["id_entrenador"]);
                }
                else if (queEs == "entrenador")
                {
                    bool trobat = false;
                    int i = 0;
                    while (!trobat && i < efadbDataSet.Tables["entrenadors"].Rows.Count)
                    {
                        if (idCercat == Convert.ToInt32(efadbDataSet.Tables["entrenadors"].Rows[i][0]))
                            trobat = true;
                        else
                            i++;
                    }
                    btnAfageixEntrenador_Click(this, null);
                    objectaCercat = true;
                    btnEsborrarEntrenadors.IsEnabled = true;
                    wpPrincipalEntenador.DataContext = FindResource("entrenadorsequipsViewSource1");
                    grid2.DataContext = FindResource("entrenadorsViewSource");
                    entrenadorsViewSource.View.MoveCurrentToPosition(i);
                    posicioRow = i;
                }
                else if (queEs == "equiprival")
                {
                    bool trobat = false;
                    int i = 0;
                    while (!trobat && i < efadbDataSet.Tables["equips_rivals"].Rows.Count)
                    {
                        if (idCercat == Convert.ToInt32(efadbDataSet.Tables["equips_rivals"].Rows[i][0]))
                            trobat = true;
                        else
                            i++;
                    }
                    btnCrearEquipRival_Click(this, null);
                    objectaCercat = true;
                    btnEsborrarEquipsRivals.IsEnabled = true;
                    gridEquisRivals.DataContext = FindResource("equips_rivalsViewSource");
                    equips_rivalsViewSource.View.MoveCurrentToPosition(i);
                    posicioRow = i;
                    id_faseComboBox.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["equips_rivals"].Rows[i]["id_fase"]);
                }
                else if (queEs == "partit")
                {
                    bool trobat = false;
                    int i = 0;
                    while (!trobat && i < efadbDataSet.Tables["partits"].Rows.Count)
                    {
                        if (idCercat == Convert.ToInt32(efadbDataSet.Tables["partits"].Rows[i][0]))
                            trobat = true;
                        else
                            i++;
                    }
                    btnPartits_Click(this, null);
                    objectaCercat = true;
                    //griddadespartit" DataContext="{StaticResource partitsViewSource}"
                    griddadespartit.DataContext = FindResource("partitsViewSource");
                    partitsViewSource.View.MoveCurrentToPosition(i);
                    posicioRow = i;
                    cbEquipClub.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["partits"].Rows[i]["id_equip"]);
                    cbEquipRival.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["partits"].Rows[i]["id_equip_rival"]);
                    cbTipusPartit.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["partits"].Rows[i]["id_tipus_partit"]);
                    cbFasePartit.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["partits"].Rows[i]["id_fase"]);
                    if (Convert.ToInt32(efadbDataSet.Tables["partits"].Rows[i]["visitant"]) == 0)
                        cblocalVisitant.SelectedItem = "Local";
                    else
                        cblocalVisitant.SelectedItem = "Visitant";
                }
            }
            else
            {
                if (objectaCercat)
                {
                    if (queEs == "jugador")
                    {
                        jugadorsViewSource.View.MoveCurrentToPosition(posicioRow);
                        try
                        {
                            imgImatge.Source = new BitmapImage(new Uri(efadbDataSet.Tables["jugadors"].Rows[posicioRow]["nomImatge"].ToString()));//actualitza la foto.
                        }
                        catch
                        {
                            imgImatge.Source = new BitmapImage(new Uri(@"C:/Fotos/img.jpg"));
                        }
                        if (Convert.ToInt32(efadbDataSet.Tables["jugadors"].Rows[posicioRow]["sexe"]) == 0)
                            tbSexe.SelectedItem = "M";
                        else
                            tbSexe.SelectedItem = "F";
                        tbposicioNom.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["jugadors"].Rows[posicioRow]["id_posicio"]);
                        cbequip.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["jugadors"].Rows[posicioRow]["id_equip"]);
                        CarregarNomsTemporades();
                        tbDni.IsEnabled = false;
                    }
                    else if (queEs == "equip")
                    {
                        equipsViewSource.View.MoveCurrentToPosition(posicioRow);
                        id_categoriaComboBox.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["equips"].Rows[posicioRow]["id_categoria"]);
                        id_divisioComboBox.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["equips"].Rows[posicioRow]["id_divisio"]);
                        id_entrenadorComboBox.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["equips"].Rows[posicioRow]["id_entrenador"]);
                    }
                    else if (queEs == "entrenador")
                    {
                        entrenadorsViewSource.View.MoveCurrentToPosition(posicioRow);
                    }
                    else if (queEs == "equiprival")
                    {
                        equips_rivalsViewSource.View.MoveCurrentToPosition(posicioRow);
                        id_faseComboBox.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["equips_rivals"].Rows[posicioRow]["id_fase"]);
                    }
                    else if (queEs == "partit")
                    {
                        if (posicioRow < efadbDataSet.Tables["partits"].Rows.Count)//ultims canvis
                        {
                            partitsViewSource.View.MoveCurrentToPosition(posicioRow);
                            cbEquipClub.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["partits"].Rows[posicioRow]["id_equip"]);
                            cbEquipRival.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["partits"].Rows[posicioRow]["id_equip_rival"]);
                            cbTipusPartit.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["partits"].Rows[posicioRow]["id_tipus_partit"]);
                            cbFasePartit.SelectedValue = Convert.ToInt32(efadbDataSet.Tables["partits"].Rows[posicioRow]["id_fase"]);
                            if (Convert.ToInt32(efadbDataSet.Tables["partits"].Rows[posicioRow]["visitant"]) == 0)
                                cblocalVisitant.SelectedItem = "Local";
                            else
                                cblocalVisitant.SelectedItem = "Visitant";
                        }
                    }
                    if(convocatoria)
                        AmagarTot();
                }
            }
        }

        /// <summary>
        /// Carrega a la taula jugadors el nom de la temproada en cada fila i aixi en el combobox jugadors temporada podem veurei el nom
        /// </summary>
        private void CarregarNomsTemporades()
        {
            bool trobat = false;
            int i = 0;
            foreach(DataRow dr in efadbDataSet.jugador_temporada)
            {
                trobat = false;
                i = 0;
                while(!trobat &&  i<efadbDataSet.Tables["temporades"].Rows.Count)
                {
                    if ((int)dr["id_temporada"] == Convert.ToInt32(efadbDataSet.Tables["temporades"].Rows[i]["id_temporada"]))
                    {
                        dr["nom"] = efadbDataSet.Tables["temporades"].Rows[i]["nom"].ToString();
                        trobat = true;
                    }
                    else
                        i++;
                }
            }
        }

        /// <summary>
        /// segons el boto fa mes gran o mes petit una columna del grid principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAfageix_Click(object sender, RoutedEventArgs e)
        {
            queEs = "jugador";
            objectaCercat = false;
            wpJugadors.DataContext = null;
            imgImatge.Source = new BitmapImage(new Uri(@"C:/Fotos/img.jpg"));
            GridLength gE2 = new GridLength(0, GridUnitType.Star);
            cdequips.Width = gE2;
            GridLength gJ2 = new GridLength(this.ActualWidth,GridUnitType.Star);
            cdjugadors.Width = gJ2;
            GridLength gC2 = new GridLength(0, GridUnitType.Star);
            cdntrenadors.Width = gC2;
            GridLength gT2 = new GridLength(0, GridUnitType.Star);
            cdTemporada.Width = gT2;
            GridLength gP2 = new GridLength(0, GridUnitType.Star);
            cdPartits.Width = gP2;
            GridLength gER2 = new GridLength(0, GridUnitType.Star);
            cdEquipsRivals.Width = gER2;
            GridLength gCon2 = new GridLength(0, GridUnitType.Star);
            cdConvocatoria.Width = gCon2;
            if (efadbDataSet.jugadors.Count > 0)
                posicioRow = efadbDataSet.jugadors.Count - 1;
            else
                posicioRow = 0;
            wpDadesTemporada.Height = 0;
            wpLesions.Height = 0;
            btnDadesTemporada.IsEnabled = false;
            btnDadesLesions.IsEnabled = false;
            btnEsborrarJugadors.IsEnabled = false;
            tbDni.IsEnabled = true;

        }

        /// <summary>
        /// segons el boto fa mes gran o mes petit una columna del grid principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAfageixEntrenador_Click(object sender, RoutedEventArgs e)
        {
            queEs = "entrenador";
            objectaCercat = false;
            wpPrincipalEntenador.DataContext = null;
            grid2.DataContext = null;
            GridLength gE2 = new GridLength(0, GridUnitType.Star);
            cdequips.Width = gE2;
            GridLength gJ2 = new GridLength(0, GridUnitType.Star);
            cdjugadors.Width = gJ2;
            GridLength gC2 = new GridLength(this.ActualWidth, GridUnitType.Star);
            cdntrenadors.Width = gC2;
            GridLength gT2 = new GridLength(0, GridUnitType.Star);
            cdTemporada.Width = gT2;
            GridLength gP2 = new GridLength(0, GridUnitType.Star);
            cdPartits.Width = gP2;
            GridLength gER2 = new GridLength(0, GridUnitType.Star);
            cdEquipsRivals.Width = gER2;
            GridLength gCon2 = new GridLength(0, GridUnitType.Star);
            cdConvocatoria.Width = gCon2;
            btnEsborrarEntrenadors.IsEnabled = false;
            if (efadbDataSet.entrenadors.Count > 0)
                posicioRow = efadbDataSet.entrenadors.Count - 1;
            else
                posicioRow = 0;
        }

        /// <summary>
        /// segons el boto fa mes gran o mes petit una columna del grid principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCrearEquip_Click(object sender, RoutedEventArgs e)
        {
            queEs = "equip";
            objectaCercat = false;
            wpPrincipalEquip.DataContext = null;
            grid1.DataContext = null;
            GridLength gE2 = new GridLength(this.ActualWidth, GridUnitType.Star);
            cdequips.Width = gE2;
            GridLength gJ2 = new GridLength(0, GridUnitType.Star);
            cdjugadors.Width = gJ2;
            GridLength gC2 = new GridLength(0, GridUnitType.Star);
            cdntrenadors.Width = gC2;
            GridLength gT2 = new GridLength(0, GridUnitType.Star);
            cdTemporada.Width = gT2;
            GridLength gP2 = new GridLength(0, GridUnitType.Star);
            cdPartits.Width = gP2;
            GridLength gER2 = new GridLength(0, GridUnitType.Star);
            cdEquipsRivals.Width = gER2;
            GridLength gCon2 = new GridLength(0, GridUnitType.Star);
            cdConvocatoria.Width = gCon2;
            btnEsborrarEquips.IsEnabled = false;
            if (efadbDataSet.equips.Count > 0)
                posicioRow = efadbDataSet.equips.Count - 1;
            else
                posicioRow = 0;
            id_categoriaComboBox.SelectedIndex = 0;
            id_divisioComboBox.SelectedIndex = 0;
            id_entrenadorComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// segons el boto fa mes gran o mes petit una columna del grid principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNovaTemporada_Click(object sender, RoutedEventArgs e)
        {
            GridLength gT2 = new GridLength(this.ActualWidth, GridUnitType.Star);
            cdTemporada.Width = gT2;
            GridLength gJ2 = new GridLength(0, GridUnitType.Star);
            cdjugadors.Width = gJ2;
            GridLength gC2 = new GridLength(0, GridUnitType.Star);
            cdntrenadors.Width = gC2;
            GridLength gE2 = new GridLength(0, GridUnitType.Star);
            cdequips.Width = gE2;
            GridLength gP2 = new GridLength(0, GridUnitType.Star);
            cdPartits.Width = gP2;
            GridLength gER2 = new GridLength(0, GridUnitType.Star);
            cdEquipsRivals.Width = gER2;
            GridLength gCon2 = new GridLength(0, GridUnitType.Star);
            cdConvocatoria.Width = gCon2;
        }

        /// <summary>
        /// Obre una columna del grid per crear un partit o llista d'ells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPartits_Click(object sender, RoutedEventArgs e)
        {
            queEs = "partit";
            objectaCercat = false;
            griddadespartit.DataContext = null;
            GridLength gP2 = new GridLength(this.ActualWidth, GridUnitType.Star);
            cdPartits.Width = gP2;
            GridLength gT2 = new GridLength(0, GridUnitType.Star);
            cdTemporada.Width = gT2;
            GridLength gJ2 = new GridLength(0, GridUnitType.Star);
            cdjugadors.Width = gJ2;
            GridLength gC2 = new GridLength(0, GridUnitType.Star);
            cdntrenadors.Width = gC2;
            GridLength gE2 = new GridLength(0, GridUnitType.Star);
            cdequips.Width = gE2;
            GridLength gER2 = new GridLength(0, GridUnitType.Star);
            cdEquipsRivals.Width = gER2;
            GridLength gCon2 = new GridLength(0, GridUnitType.Star);
            cdConvocatoria.Width = gCon2;
            cbEquipClub.SelectedIndex = 0;
            cbEquipRival.SelectedIndex = 0;
            cbFasePartit.SelectedIndex = 0;
            cbTipusPartit.SelectedIndex = 0;
        }

        private void btnCrearEquipRival_Click(object sender, RoutedEventArgs e)
        {
            queEs = "equiprival";
            objectaCercat = false;
            gridEquisRivals.DataContext = null; 
            GridLength gER2 = new GridLength(this.ActualWidth, GridUnitType.Star);
            cdEquipsRivals.Width = gER2;
            GridLength gT2 = new GridLength(0, GridUnitType.Star);
            cdTemporada.Width = gT2;
            GridLength gJ2 = new GridLength(0, GridUnitType.Star);
            cdjugadors.Width = gJ2;
            GridLength gC2 = new GridLength(0, GridUnitType.Star);
            cdntrenadors.Width = gC2;
            GridLength gE2 = new GridLength(0, GridUnitType.Star);
            cdequips.Width = gE2;
            GridLength gP2 = new GridLength(0, GridUnitType.Star);
            cdPartits.Width = gP2;
            GridLength gCon2 = new GridLength(0, GridUnitType.Star);
            cdConvocatoria.Width = gCon2;
            btnEsborrarEquipsRivals.IsEnabled = false;
            if (efadbDataSet.equips_rivals.Count > 0)
                posicioRow = efadbDataSet.equips_rivals.Count - 1;
            else
                posicioRow = 0;
        }

        private void btnComvocatories_Click(object sender, RoutedEventArgs e)
        {
            lbJugadorsConvocats.Items.Clear();
            gridTotsJugadors.DataContext = null;
            lblPartitVs.Content = "";
            btnGuardarConvocatoria.IsEnabled = false;
            objectaCercat = false;
            GridLength gCon2 = new GridLength(this.ActualWidth, GridUnitType.Star);
            cdConvocatoria.Width = gCon2;
            GridLength gP2 = new GridLength(0, GridUnitType.Star);
            cdPartits.Width = gP2;
            GridLength gER2 = new GridLength(0, GridUnitType.Star);
            cdEquipsRivals.Width = gER2;
            GridLength gT2 = new GridLength(0, GridUnitType.Star);
            cdTemporada.Width = gT2;
            GridLength gJ2 = new GridLength(0, GridUnitType.Star);
            cdjugadors.Width = gJ2;
            GridLength gC2 = new GridLength(0, GridUnitType.Star);
            cdntrenadors.Width = gC2;
            GridLength gE2 = new GridLength(0, GridUnitType.Star);
            cdequips.Width = gE2;     
        }

        /// <summary>
        /// Amaga totes les columnes
        /// </summary>
        private void AmagarTot()
        {
            GridLength gP2 = new GridLength(0, GridUnitType.Star);
            cdPartits.Width = gP2;
            GridLength gER2 = new GridLength(0, GridUnitType.Star);
            cdEquipsRivals.Width = gER2;
            GridLength gT2 = new GridLength(0, GridUnitType.Star);
            cdTemporada.Width = gT2;
            GridLength gJ2 = new GridLength(0, GridUnitType.Star);
            cdjugadors.Width = gJ2;
            GridLength gC2 = new GridLength(0, GridUnitType.Star);
            cdntrenadors.Width = gC2;
            GridLength gE2 = new GridLength(0, GridUnitType.Star);
            cdequips.Width = gE2;
            GridLength gCon2 = new GridLength(0, GridUnitType.Star);
            cdConvocatoria.Width = gCon2;
        }

        /// <summary>
        /// tenca la app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSortir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// fa l'efecta de reduir el Height del border
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// fa l'efecta de reduir el Height del border
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        ///  fa l'efecta de reduir el Height del border
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// fa l'efecta de reduir el Height del border
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// fa l'efecta de reduir el Height del border
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDadesNovaTemporada_Click(object sender, RoutedEventArgs e)
        {
            if (wpNovaTemporada.Height > 0 || wpNovaTemporada.Height.Equals(Double.NaN))
            {
                wpNovaTemporada.Height = 0;
            }
            else
            {
                wpNovaTemporada.Height = Double.NaN; // aixo es  height l'auto del xaml
            }
        }

        /// <summary>
        /// És l'event de validar l'insert dels varis elements que tenim , jugadors entrenadors, equips
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        

        /// <summary>
        /// el ser modificat el text busca a la base de dades si hi ha una coincidencia i si es aixo no deixa guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbDni_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool trobat = false;
            btnGuardar.IsEnabled = true;
            if (tbDni.Text.Length > 8 && !esborrat)
            {
                foreach (DataRow dr in efadbDataSet.Tables["jugadors"].Rows)
                {
                    if (dr["dni"].ToString().ToLower() == tbDni.Text.Trim().ToLower())
                    {
                        trobat = true;
                    }
                }
                if (trobat)//aixo fa que si no es una modificacio deseviliti el boto guardar
                {
                    if (!objectaCercat)
                    {
                        MessageBox.Show("Ja existeix");
                        btnGuardar.IsEnabled = false;
                    }
                }
            }
            else
            {
                btnGuardar.IsEnabled = false;
            }
        }

        /// <summary>
        /// el ser modificat el text busca a la base de dades si hi ha una coincidencia i si es aixo no deixa guardar ,entrenadors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dniTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool trobat = false;
            btnGuardarEntrenador.IsEnabled = true;
            if (cdntrenadors.Width.Value > 0)
            {
                if (dniTextBox.Text.Length > 8 && !esborrat)
                {
                    foreach (DataRow dr in efadbDataSet.Tables["entrenadors"].Rows)
                    {
                        if (dr["dni"].ToString().ToLower() == dniTextBox.Text.Trim().ToLower())
                        {
                            trobat = true;
                        }
                    }
                    if (trobat)//aixo fa que si no es una modificacio deseviliti el boto guardar
                    {
                        if (!objectaCercat)
                        {
                            MessageBox.Show("Ja existeix");
                            btnGuardarEntrenador.IsEnabled = false;
                        }
                    }
                }
                else
                {
                    btnGuardarEntrenador.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// El ser modificat el text mira que no sigui null per tant que l'equip tingui un nom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nomTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(nomTextBox.Text.Length > 0)
                btnGuardarEquip.IsEnabled = true;
            else
                btnGuardarEquip.IsEnabled = false;
        }

        /// <summary>
        /// Obre un dialeg amb el boto dret del ratoli per selecionar una imatge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgImatge_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                imgImatge.Source = new BitmapImage(new Uri(filename));
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                Guid photoID = System.Guid.NewGuid();
                encoder.Frames.Add(BitmapFrame.Create((BitmapImage)imgImatge.Source));
                rutaImg = @"C:/Fotos/" + tbDni.Text.ToLower().Trim()+"img.jpg";
                using (FileStream filestream = new FileStream(rutaImg, FileMode.Create))
                {
                    encoder.Save(filestream);
                }
            }
        }

        /// <summary>
        ///el ser modificat el text busca a la base de dades si hi ha una coincidencia i si es aixo no deixa guardar temporades nom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbNomTemp_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool trobat = false;
            btnGuardarTemporada.IsEnabled = true;
            if (tbNomTemp.Text.Length > 0)
            {
                foreach (DataRow dr in efadbDataSet.Tables["temporades"].Rows)
                {
                    if (dr["nom"].ToString().ToLower() == tbNomTemp.Text.Trim().ToLower())
                    {
                        trobat = true;
                    }
                }
                if (trobat)
                {
                    MessageBox.Show("Ja existeix");
                    btnGuardarTemporada.IsEnabled = false;
                }
            }
            else
            {
                btnGuardarTemporada.IsEnabled = false;
            }
        }

        private void btnDadesEquipRival_Click(object sender, RoutedEventArgs e)
        {
            if (wpDadesEquipRival.Height > 0 || wpDadesEquipRival.Height.Equals(Double.NaN))
            {
                wpDadesEquipRival.Height = 0;
            }
            else
            {
                wpDadesEquipRival.Height = Double.NaN; // aixo es  height l'auto del xaml
            }
        }

        /// <summary>
        /// Comprova que el camp nom de l'Equip tingui un nom si esta buit desavilita el guardat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nomTextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (nomEquipRivalTextBox.Text.Length > 0)
                btnGuardarEquipRival.IsEnabled = true;
            else
                btnGuardarEquipRival.IsEnabled = false;
        }

        /// <summary>
        /// Expandeix el wrap panel de les lesions mostran dades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDadesLesions_Click(object sender, RoutedEventArgs e)
        {
            if (wpLesions.Height > 0 || wpLesions.Height.Equals(Double.NaN))
            {
                wpLesions.Height = 0;
            }
            else
            {
                wpLesions.Height = Double.NaN; // aixo es  height l'auto del xaml
            }
        }

        /// <summary>
        /// Guarda i si es un objecta cercat actualitza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!objectaCercat)
            {
                if (cdjugadors.Width.Value > 0)
                {
                    int idJugador = 0;
                    if (efadbDataSet.Tables["jugadors"].Rows.Count > 0)
                        idJugador = Convert.ToInt32(efadbDataSet.Tables["jugadors"].Rows[efadbDataSet.Tables["jugadors"].Rows.Count - 1]["id_jugador"]);
                    efadbDataSet.Tables["jugadors"].Columns[0].AutoIncrement = true;
                    efadbDataSet.Tables["jugadors"].Columns[0].AutoIncrementSeed = idJugador + 1;
                    efadbDataSet.Tables["jugadors"].Columns[0].AutoIncrementStep = 1;
                    DataRow dr = efadbDataSet.Tables["jugadors"].NewRow();
                    dr["dni"] = tbDni.Text.Trim();
                    dr["nom"] = tbNom.Text.Trim();
                    dr["cognoms"] = tbCognom.Text.Trim();
                    if (tbSexe.SelectedIndex > 0)
                        dr["sexe"] = Convert.ToInt16(tbSexe.SelectedIndex);
                    else
                        dr["sexe"] = 0;

                    dr["nomImatge"] = rutaImg;

                    if (dpAnyInscripcio.SelectedDate.ToString().Trim().Length > 0)
                        dr["data_inscripcio"] = DateTime.Parse(dpAnyInscripcio.SelectedDate.ToString());
                    else
                        dr["data_inscripcio"] = DateTime.Parse(DateTime.Now.ToShortDateString());

                    if (dpAnyNeixament.SelectedDate.ToString().Trim().Length > 0)
                        dr["data_naixement"] = DateTime.Parse(dpAnyNeixament.SelectedDate.ToString());
                    else
                        dr["data_naixement"] = DateTime.Parse(DateTime.Now.ToShortDateString());
                    dr["tarjeta_sanitaria"] = tbTarjetaSanitaria.Text.Trim();
                    dr["malaltia_alergia"] = tbMalalties.Text.Trim();
                    dr["mobil"] = tbMobil.Text.Trim();
                    dr["telefon"] = tbTelefon.Text.Trim();
                    dr["correu_electronic"] = tbcorreuElec.Text.Trim();
                    dr["numero_soci"] = tbnSoci.Text.Trim();
                    dr["lateralitat"] = tbLateralitat.Text.Trim();
                    if (tbEdat.Content.ToString().Trim().Length > 0)
                        dr["edat"] = Convert.ToInt32(tbEdat.Content.ToString().Trim());
                    else
                        dr["edat"] = 0;
                    dr["id_posicio"] = Convert.ToInt32(tbposicioNom.SelectedValue);
                    dr["id_equip"] = Convert.ToInt32(cbequip.SelectedValue);
                    try
                    {
                        efadbDataSet.Tables["jugadors"].Rows.Add(dr);
                        efadbDataSetjugadorsTableAdapter.Update(dr);
                        efadbDataSetjugadorsTableAdapter.Fill(efadbDataSet.jugadors);
                        //for (int i = 0; i < efadbDataSet.Tables["temporades"].Rows.Count; i++)
                        //{
                            DataRow dr2 = efadbDataSet.Tables["jugador_temporada"].NewRow();
                            dr2["id_jugador"] = Convert.ToInt32(efadbDataSet.Tables["jugadors"].Rows[efadbDataSet.Tables["jugadors"].Rows.Count - 1]["id_jugador"]);
                            dr2["id_temporada"] = Convert.ToInt32(efadbDataSet.Tables["temporades"].Rows[efadbDataSet.Tables["temporades"].Rows.Count-1]["id_temporada"]);
                            dr2["gols"] = 0;
                            dr2["ocasions_de_gol"] = 0;
                            dr2["minuts_jugats"] = 0;
                            dr2["faltes_comeses"] = 0;
                            dr2["faltes_rebudes"] = 0;
                            dr2["targetes_grogues"] = 0;
                            dr2["targetes_vermelles"] = 0;
                            dr2["pes"] = 0;
                            dr2["altura"] = 0;
                            dr2["dorsal"] = 0;
                            dr2["faltes_entreno"] = 0;
                            dr2["notaGS"] = 0;
                            efadbDataSet.Tables["jugador_temporada"].Rows.Add(dr2);
                            efadbDataSetjugador_temporadaTableAdapter.Update(dr2);
                            efadbDataSetjugador_temporadaTableAdapter.Fill(efadbDataSet.jugador_temporada);
                        //}
                        MessageBox.Show("S'ha guardat: " + queEs);
                        objectaCercat = true;
                        queEs = "jugador";
                        wpJugadors.DataContext = FindResource("jugadorsViewSource");
                        jugadorsViewSource.View.MoveCurrentToLast();
                        posicioRow = efadbDataSet.jugadors.Count - 1;
                        tbSexe.SelectedIndex = Convert.ToInt32(efadbDataSet.jugadors.Rows[posicioRow]["sexe"]);
                        btnEsborrarJugadors.IsEnabled = true;
                        tbDni.IsEnabled = false;

                        

                    }
                    catch (Exception ex)
                    {
                        if(cbequip.SelectedIndex < 0)
                            MessageBox.Show("No has triat l'equip del jugador");
                        else
                            MessageBox.Show("No s'ha pogut guardar:" + ex.ToString());
                    }
                }
                if (cdntrenadors.Width.Value > 0)
                {
                    DataRow dr = efadbDataSet.Tables["entrenadors"].NewRow();
                    dr["dni"] = dniTextBox.Text.Trim();
                    dr["nom"] = nomTextBox1.Text.Trim();
                    dr["cognom"] = cognomTextBox.Text.Trim();
                    if (data_naixementDatePicker.SelectedDate.ToString().Trim().Length > 0)
                        dr["data_naixement"] = DateTime.Parse(data_naixementDatePicker.SelectedDate.ToString());
                    else
                        dr["data_naixement"] = DateTime.Parse(DateTime.Now.ToShortDateString());
                    try
                    {
                        efadbDataSet.entrenadors.Rows.Add(dr);
                        efadbDataSetentrenadorsTableAdapter.Update(dr);
                        efadbDataSetentrenadorsTableAdapter.Fill(efadbDataSet.entrenadors);
                        MessageBox.Show("S'ha guardat: " + queEs);
                        objectaCercat = true;
                        btnGuardarEntrenador.IsEnabled = false;
                        wpPrincipalEntenador.DataContext = FindResource("entrenadorsequipsViewSource1");
                        grid2.DataContext = FindResource("entrenadorsViewSource");
                        queEs = "entrenador";
                        entrenadorsViewSource.View.MoveCurrentToLast();
                        posicioRow = efadbDataSet.entrenadors.Count - 1;
                        btnEsborrarEntrenadors.IsEnabled = true;
                        btnCrearEquip.IsEnabled = true;
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
                    if (id_divisioComboBox.SelectedIndex >= 0)
                        dr["id_divisio"] = Convert.ToInt32(id_divisioComboBox.SelectedValue);
                    else
                        dr["id_divisio"] = 1;

                    if (id_categoriaComboBox.SelectedIndex >= 0)
                        dr["id_categoria"] = Convert.ToInt32(id_categoriaComboBox.SelectedValue);
                    else
                        dr["id_categoria"] = 1;

                    if (id_entrenadorComboBox.SelectedIndex >= 0)
                        dr["id_entrenador"] = Convert.ToInt32(id_entrenadorComboBox.SelectedValue);
                    else
                        dr["id_entrenador"] = 1;
                    if (puntuacioTextBox.Text.Trim().Length > 0)
                        dr["puntuacio"] = puntuacioTextBox.Text.Trim();
                    else
                        dr["puntuacio"] = 0;
                    dr["id_club"] = 1;
                    try
                    {
                        efadbDataSet.equips.Rows.Add(dr);
                        efadbDataSetequipsTableAdapter.Update(dr);
                        efadbDataSetequipsTableAdapter.Fill(efadbDataSet.equips);
                        MessageBox.Show("S'ha guardat: " + queEs);
                        objectaCercat = true;
                        wpPrincipalEquip.DataContext = FindResource("equipsjugadorsViewSource");
                        grid1.DataContext = FindResource("equipsViewSource1");
                        queEs = "equip";
                        equipsViewSource.View.MoveCurrentToLast();
                        posicioRow = efadbDataSet.equips.Count - 1;
                        btnEsborrarEquips.IsEnabled = true;
                        btnPartits.IsEnabled = true;
                        btnAfageix.IsEnabled = true;
                    }
                    catch (Exception ex)
                    {
                        if (id_entrenadorComboBox.SelectedIndex< 0)
                            MessageBox.Show("Falta l'entrenador de l'equip");
                        else
                            MessageBox.Show("No s'ha pogut guardar:" + ex.ToString());
                    }
                }
                if (cdEquipsRivals.Width.Value > 0)
                {
                    DataRow dr = efadbDataSet.Tables["equips_rivals"].NewRow();
                    dr["nom"] = nomEquipRivalTextBox.Text.Trim();
                    dr["poblacio"] = poblacioRivalTextBox.Text.Trim();
                    if (puntuacioRivalTextBox.Text.Trim().Length > 0)
                        dr["puntuacio"] = puntuacioRivalTextBox.Text.Trim();
                    else
                        dr["puntuacio"] = 0;
                    dr["id_fase"] = Convert.ToInt32(id_faseComboBox.SelectedValue);
                    try
                    {
                        efadbDataSet.equips_rivals.Rows.Add(dr);
                        efadbDataSetequips_rivalsTableAdapter.Update(dr);
                        efadbDataSetequips_rivalsTableAdapter.Fill(efadbDataSet.equips_rivals);
                        MessageBox.Show("S'ha guardat: " + queEs);
                        objectaCercat = true;
                        gridEquisRivals.DataContext = FindResource("equips_rivalsViewSource");
                        queEs = "equiprival";
                        equips_rivalsViewSource.View.MoveCurrentToLast();
                        posicioRow = efadbDataSet.equips_rivals.Count-1;
                        btnEsborrarEquipsRivals.IsEnabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No s'ha pogut guardar:" + ex.ToString());
                    }
                }
            }
            else // si es un objecta que ja existeix i que lem trobat a la finestra de cerca fa l'update de la fila del dataset i no en crea una de nova
            {
                try
                {
                    if (queEs == "jugador")
                    {
                        efadbDataSet.Tables["jugadors"].Rows[posicioRow]["sexe"] = Convert.ToInt16(tbSexe.SelectedIndex);
                        efadbDataSet.Tables["jugadors"].Rows[posicioRow]["id_posicio"] = Convert.ToInt32(tbposicioNom.SelectedValue);
                        efadbDataSet.Tables["jugadors"].Rows[posicioRow]["id_equip"] = Convert.ToInt32(cbequip.SelectedValue);
                        efadbDataSet.Tables["jugadors"].Rows[posicioRow]["nomImatge"] = rutaImg;
                        
                        efadbDataSetjugadorsTableAdapter.Update(efadbDataSet.jugadors);
                        efadbDataSetjugadorsTableAdapter.Fill(efadbDataSet.jugadors);
                        efadbDataSetjugador_temporadaTableAdapter.Update(efadbDataSet.jugador_temporada);
                        
                        // Generem la Nota GS
                        string url = "http://gsports.es/gsport/jugadorEquip.php?idJugador=" + efadbDataSet.Tables["jugadors"].Rows[posicioRow]["id_jugador"];
                        string result = null;
                        WebClient client = new WebClient();
                        result = client.DownloadString(url);
                        //efadbDataSetjugador_temporadaTableAdapter.Fill(efadbDataSet.jugador_temporada);
                        efadbDataSetjugador_temporadaTableAdapter.Fill(efadbDataSet.jugador_temporada);
                        efadbDataSetlesionsTableAdapter.Update(efadbDataSet.lesions);                       
                        efadbDataSetlesionsTableAdapter.Fill(efadbDataSet.lesions);
                        CarregarNomsTemporades();
                        //refresh 
                        jugadorsViewSource.View.MoveCurrentToPosition(posicioRow);
                        tbSexe.SelectedIndex = Convert.ToInt32(efadbDataSet.jugadors.Rows[posicioRow]["sexe"]);
                        MessageBox.Show("S'ha guardat: " + queEs);
                        //
                    }
                    else if (queEs == "equip")
                    {
                        efadbDataSet.Tables["equips"].Rows[posicioRow]["id_divisio"] = Convert.ToInt32(id_divisioComboBox.SelectedValue);
                        efadbDataSet.Tables["equips"].Rows[posicioRow]["id_categoria"] = Convert.ToInt32(id_categoriaComboBox.SelectedValue);
                        efadbDataSet.Tables["equips"].Rows[posicioRow]["id_entrenador"] = Convert.ToInt32(id_entrenadorComboBox.SelectedValue);
                        efadbDataSetequipsTableAdapter.Update(efadbDataSet.equips);
                        efadbDataSetequipsTableAdapter.Fill(efadbDataSet.equips);
                        equipsViewSource.View.MoveCurrentToPosition(posicioRow);
                        MessageBox.Show("S'ha guardat: " + queEs);
                    }
                    else if(queEs == "entrenador")
                    {
                        efadbDataSetentrenadorsTableAdapter.Update(efadbDataSet.entrenadors.Rows[posicioRow]);
                        efadbDataSetentrenadorsTableAdapter.Fill(efadbDataSet.entrenadors);
                        entrenadorsViewSource.View.MoveCurrentToPosition(posicioRow);
                        MessageBox.Show("S'ha guardat: " + queEs);
                    }
                    else if(queEs == "equiprival")
                    {
                        efadbDataSet.Tables["equips_rivals"].Rows[posicioRow]["id_fase"] = Convert.ToInt32(id_faseComboBox.SelectedValue);
                        efadbDataSetequips_rivalsTableAdapter.Update(efadbDataSet.equips_rivals);
                        efadbDataSetequips_rivalsTableAdapter.Fill(efadbDataSet.equips_rivals);
                        equips_rivalsViewSource.View.MoveCurrentToPosition(posicioRow);
                        MessageBox.Show("S'ha guardat: " + queEs);
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex + "");
                }
            }
        }

        /// <summary>
        /// És l'event de validar l'insert a les temporades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardarTemporada_Click(object sender, RoutedEventArgs e)
        {
            if (efadbDataSet.Tables["temporades"].Rows.Count > 0)
                idTemporada = Convert.ToInt32(efadbDataSet.Tables["temporades"].Rows[efadbDataSet.Tables["temporades"].Rows.Count - 1]["id_temporada"]);

            efadbDataSet.Tables["temporades"].Columns[0].AutoIncrement = true;
            efadbDataSet.Tables["temporades"].Columns[0].AutoIncrementSeed = idTemporada + 1;
            efadbDataSet.Tables["temporades"].Columns[0].AutoIncrementStep = 1;
            DataRow dr = efadbDataSet.Tables["temporades"].NewRow();
            dr["nom"] = tbNomTemp.Text.Trim();
            dr["data_Inici"] = DateTime.Parse(data_IniciDatePicker.SelectedDate.ToString());
            dr["data_Fi"] = DateTime.Parse(data_FiDatePicker.SelectedDate.ToString());
            try
            {
                efadbDataSet.temporades.Rows.Add(dr);
                efadbDataSettemporadesTableAdapter.Update(dr);
                efadbDataSettemporadesTableAdapter.Fill(efadbDataSet.temporades);
                MessageBox.Show("Guardat");
                foreach (DataRow drJugador in efadbDataSet.jugadors)
                {
                    DataRow dr2 = efadbDataSet.Tables["jugador_temporada"].NewRow();
                    dr2["id_jugador"] = Convert.ToInt32(drJugador["id_jugador"]);
                    dr2["id_temporada"] = Convert.ToInt32(efadbDataSet.Tables["temporades"].Rows[efadbDataSet.Tables["temporades"].Rows.Count - 1]["id_temporada"]);
                    dr2["gols"] = 0;
                    dr2["ocasions_de_gol"] = 0;
                    dr2["minuts_jugats"] = 0;
                    dr2["faltes_comeses"] = 0;
                    dr2["faltes_rebudes"] = 0;
                    dr2["targetes_grogues"] = 0;
                    dr2["targetes_vermelles"] = 0;
                    dr2["pes"] = 0;
                    dr2["altura"] = 0;
                    dr2["dorsal"] = 0;
                    dr2["faltes_entreno"] = 0;
                    dr2["notaGS"] = 0;
                    efadbDataSet.Tables["jugador_temporada"].Rows.Add(dr2);
                    efadbDataSetjugador_temporadaTableAdapter.Update(dr2);
                    efadbDataSetjugador_temporadaTableAdapter.Fill(efadbDataSet.jugador_temporada);
                    btnAfageix.IsEnabled = true;
                    btnImportador.IsEnabled = false;
                }
                btnGuardarTemporada.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No s'ha pogut guardar:" + ex.ToString());
            }
        }

        /// <summary>
        /// Guarda el partit mitjançant una procedure i els parametres passats per el programa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardarPartit_Click(object sender, RoutedEventArgs e)
        {
            if (!objectaCercat)
            {
                ///Aqui s'ha afagit una llibreria per accedir amb cadena de conexio a mysql i fer commands, en principi cap problema.
                MySqlConnection cnMySql = new MySqlConnection(mySqlString);
                try
                {
                    cnMySql.Open();
                    MySqlCommand cmd;
                    cmd = new MySqlCommand("Insertarpartit", cnMySql);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("inidequip", Convert.ToInt32(cbEquipClub.SelectedValue));
                    cmd.Parameters.AddWithValue("inidequiprival", Convert.ToInt32(cbEquipRival.SelectedValue));
                    cmd.Parameters.AddWithValue("inidfase", Convert.ToInt32(cbFasePartit.SelectedValue));
                    cmd.Parameters.AddWithValue("inidtipus", Convert.ToInt32(cbTipusPartit.SelectedValue));
                    if (Convert.ToInt32(tbJornadaPartit.Text.Trim().Length) > 0)
                        cmd.Parameters.AddWithValue("injornada", Convert.ToInt32(tbJornadaPartit.Text.Trim()));
                    else
                        cmd.Parameters.AddWithValue("injornada", 1);

                    if (data_partitDatePicker.SelectedDate.ToString().Trim().Length > 0)
                        cmd.Parameters.AddWithValue("indatapartit", DateTime.Parse(data_partitDatePicker.SelectedDate.ToString()));
                    else
                        cmd.Parameters.AddWithValue("indatapartit", DateTime.Parse(DateTime.Now.ToShortDateString()));

                    if (tbgolsLocals.Text.Trim().Length > 0)
                        cmd.Parameters.AddWithValue("ingolslocal", Convert.ToInt32(tbgolsLocals.Text.Trim()));
                    else
                        cmd.Parameters.AddWithValue("ingolslocal", 0);

                    if (tbgolsVisitants.Text.Trim().Length > 0)
                        cmd.Parameters.AddWithValue("ingolsvisitant", Convert.ToInt32(tbgolsVisitants.Text.Trim()));
                    else
                        cmd.Parameters.AddWithValue("ingolsvisitant", 0);

                    if (cblocalVisitant.SelectedIndex >= 0)
                        cmd.Parameters.AddWithValue("invisitant", Convert.ToInt16(cblocalVisitant.SelectedIndex));
                    else
                        cmd.Parameters.AddWithValue("invisitant", 0);
                    if (efadbDataSet.partits.Rows.Count > 0)
                        cmd.Parameters.AddWithValue("inidpartit", Convert.ToInt32(efadbDataSet.partits.Rows[efadbDataSet.partits.Rows.Count - 1]["id_partit"]) + 1);
                    else
                        cmd.Parameters.AddWithValue("inidpartit", 1);
                    cmd.ExecuteNonQuery();
                    cnMySql.Close();
                    efadbDataSetpartitsTableAdapter.Fill(efadbDataSet.partits);
                    partitsViewSource.View.MoveCurrentToLast();
                    MessageBox.Show("S'ha guardat: " + queEs);
                    AmagarTot();
                }
                catch (Exception ex)
                {
                    if (cbEquipClub.SelectedIndex < 0 && cbEquipRival.SelectedIndex < 0)
                        MessageBox.Show("No has triat els equips");
                    else if (cbEquipClub.SelectedIndex < 0 || cbEquipRival.SelectedIndex < 0)
                        MessageBox.Show("No has triat un dels equips");
                    else
                        MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                efadbDataSet.Tables["partits"].Rows[posicioRow]["id_equip"] = Convert.ToInt32(cbEquipClub.SelectedValue);
                efadbDataSet.Tables["partits"].Rows[posicioRow]["id_equip_rival"] = Convert.ToInt32(cbEquipRival.SelectedValue);
                efadbDataSet.Tables["partits"].Rows[posicioRow]["id_tipus_partit"] = Convert.ToInt32(cbTipusPartit.SelectedValue);
                efadbDataSet.Tables["partits"].Rows[posicioRow]["id_fase"] = Convert.ToInt32(cbFasePartit.SelectedValue);
                efadbDataSet.Tables["partits"].Rows[posicioRow]["visitant"] = cblocalVisitant.SelectedIndex;
                efadbDataSetpartitsTableAdapter.Update(efadbDataSet.partits.Rows[posicioRow]);
                efadbDataSetpartitsTableAdapter.Fill(efadbDataSet.partits);
                partitsViewSource.View.MoveCurrentToPosition(posicioRow); //aplicat el final perque no em marxi el binding
                MessageBox.Show("S'ha guardat: " + queEs);
            }
        }

        private void btnGuardarConvocatoria_Click(object sender, RoutedEventArgs e)
        {
            ///Aqui s'ha afagit una llibreria per accedir amb cadena de conexio a mysql i fer commands, en principi cap problema.
                cnMySql = new MySqlConnection(mySqlString);
                try
                {
                    cnMySql.Open();
                    MySqlCommand cmd;
                    cmd = new MySqlCommand("inserirconvocatoria", cnMySql);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("inidpartit", idCercat);
                    if (efadbDataSet.convocatories.Rows.Count > 0)
                        cmd.Parameters.AddWithValue("inidconvocatoria", Convert.ToInt32(efadbDataSet.convocatories.Rows[efadbDataSet.convocatories.Rows.Count - 1]["id_convocatoria"]) + 1);
                    else
                        cmd.Parameters.AddWithValue("inidconvocatoria", 1);
                    if (cbhora.SelectedIndex >= 0 && cbminut.SelectedIndex >= 0)
                        cmd.Parameters.AddWithValue("inhoraconvocatoria", Convert.ToDateTime(cbhora.SelectedItem.ToString() + ":" + cbminut.SelectedItem.ToString() + ":00").ToShortTimeString());
                    else
                        cmd.Parameters.AddWithValue("inhoraconvocatoria", Convert.ToDateTime("12:00:00").ToShortTimeString());
                    cmd.ExecuteNonQuery();
                    efadbDataSetconvocatoriesTableAdapter.Fill(efadbDataSet.convocatories);
                    for (int i = 0; i < lbJugadorsConvocats.Items.Count;i++)
                    {
                        MySqlCommand cmd2 = new MySqlCommand("Insertardetallconvocatoria", cnMySql);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("inidconvocatoria", efadbDataSet.convocatories.Rows[efadbDataSet.convocatories.Rows.Count - 1]["id_convocatoria"]);
                        int idjugador = Convert.ToInt32(((System.Data.DataRowView)lbJugadorsConvocats.Items[i]).Row.ItemArray[0]);
                        cmd2.Parameters.AddWithValue("inidjugador", idjugador);
                        cmd2.ExecuteNonQuery();
                    }
                    cnMySql.Close();
                    //ht tp://gsports.es/gsport/notificacioConvocatoria.php?idConvocatoria=1
                    MessageBoxResult resultat = MessageBox.Show("S'ha guardat correctament \n Vols enviar notificació?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if(resultat == MessageBoxResult.Yes)
                    {
                        // Generem la Notificació PUSH a partir del webService
                        string url = "http://gsports.es/gsport/notificacioConvocatoria.php?idConvocatoria=" + efadbDataSet.convocatories.Rows[efadbDataSet.convocatories.Rows.Count - 1]["id_convocatoria"];
                        string result = null;
                        try
                        {
                            WebClient client = new WebClient();
                            result = client.DownloadString(url);
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    btnGuardarConvocatoria.IsEnabled = false;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex + "");
                }
        }

        /// <summary>
        /// Obre la finestra de carga guarda l'usuari que s'ha introduit i segons el nivell de privilegi habilita i deshabilita controls a mes crear els view i els adapters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                efadbDataSet = ((Gsport.efadbDataSet)(this.FindResource("efadbDataSet")));
                // Cargar datos en la tabla jugadors. Puede modificar este código según sea necesario.
                efadbDataSetjugadorsTableAdapter = new Gsport.efadbDataSetTableAdapters.jugadorsTableAdapter();
                efadbDataSetjugadorsTableAdapter.Fill(efadbDataSet.jugadors);
                efadbDataSetusuarisTableAdapter = new Gsport.efadbDataSetTableAdapters.usuarisTableAdapter();
                efadbDataSetusuarisTableAdapter.Fill(efadbDataSet.usuaris,1);     
                jugadorsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("jugadorsViewSource")));
                wndLogin wnd = new wndLogin(efadbDataSet);
                wnd.ShowDialog();
                codiUsuari = wnd.codiUsuari;
                switch (wnd.privilegi)
                {
                    case 1: //Jugadors
                        equipsDataGrid.IsReadOnly = true;
                        jugadorsDataGrid.IsReadOnly = true;
                        wpLesions.Height = 0;
                        wpDadesPersonals.IsEnabled = false;
                        wpLesionsjugador.IsEnabled = false;
                        wpDadesTemporada.IsEnabled = false;
                        wpDadesEntrenador.IsEnabled = false;  
                        wpDadesEquip.IsEnabled = false;
                        wpDadesEquipRival.IsEnabled = false;
                        wpJugadors.IsEnabled = false;
                        wpPrincipalEntenador.IsEnabled = false;
                        wpPrincipalEquip.IsEnabled = false;
                        wpDadesConvocatoria.IsEnabled = false;
                        wpDadesEntrenador.IsEnabled = false;
                        wpPrincipalEquip.IsEnabled = false;
                        spDadesPartit.IsEnabled = false;
                        btnEsborrarEntrenadors.Visibility= Visibility.Hidden;
                        btnEsborrarEquips.Visibility = Visibility.Hidden;
                        btnEsborrarEquipsRivals.Visibility = Visibility.Hidden;
                        btnEsborrarJugadors.Visibility = Visibility.Hidden;
                        btnGuardar.Visibility = Visibility.Hidden;
                        btnGuardarEntrenador.Visibility = Visibility.Hidden;
                        btnGuardarEquip.Visibility = Visibility.Hidden;
                        btnGuardarPartit.Visibility = Visibility.Hidden;
                        btnGuardarEquipRival.Visibility = Visibility.Hidden;
                        imgImatge.IsEnabled = false;
                        break;
                    case 2: //Delegat
                        equipsDataGrid.IsReadOnly = true;
                        jugadorsDataGrid.IsReadOnly = true;
                        btnLlistats.IsEnabled = true;
                        spDadesEquipRival.IsEnabled = false;
                        spDadesPartit.IsEnabled = false;
                        spDadesEntrenador.IsEnabled = false;
                        btnEsborrarEntrenadors.Visibility= Visibility.Hidden;
                        btnEsborrarEquips.Visibility = Visibility.Hidden;
                        btnEsborrarEquipsRivals.Visibility = Visibility.Hidden;
                        btnGuardarEntrenador.Visibility = Visibility.Hidden;
                        btnGuardarEquip.Visibility = Visibility.Hidden;
                        btnGuardarPartit.Visibility = Visibility.Hidden;
                        btnGuardarEquipRival.Visibility = Visibility.Hidden;
                        break;
                    case 3: //Entrenador
                        btnAfageix.IsEnabled = true;
                        btnCrearEquipRival.IsEnabled = true;
                        btnPartits.IsEnabled = true;
                        btnComvocatories.IsEnabled = true;
                        btnLlistats.IsEnabled = true;

                        break;
                    case 4: //Coordinador
                        btnAfageix.IsEnabled = true;
                        btnCrearEquip.IsEnabled = true;
                        btnNovaTemporada.IsEnabled = true;
                        btnAfageixEntrenador.IsEnabled = true;
                        btnCrearEquipRival.IsEnabled = true;
                        btnImportador.IsEnabled = true;
                        btnPartits.IsEnabled = true;
                        btnComvocatories.IsEnabled = true;
                        btnLlistats.IsEnabled = true;
                        btnLlistatEntrenadors.IsEnabled = true;
                        btnLlistatEquips.IsEnabled = true;
                        //jugadorsDataGrid.IsReadOnly = false;
                        break;
                    case 5: //Gsport Admin
                        btnAfageix.IsEnabled = true;
                        btnCrearEquip.IsEnabled = true;
                        btnNovaTemporada.IsEnabled = true;
                        btnAfageixEntrenador.IsEnabled = true;
                        btnCrearEquipRival.IsEnabled = true;
                        btnImportador.IsEnabled = true;
                        btnPartits.IsEnabled = true;
                        btnComvocatories.IsEnabled = true;
                        btnLlistats.IsEnabled = true;
                        btnLlistatEntrenadors.IsEnabled = true;
                        btnLlistatEquips.IsEnabled = true;
                        //jugadorsDataGrid.IsReadOnly = false;
                        break;
                }
                // Cargar datos en la tabla equips. Puede modificar este código según sea necesario.
                efadbDataSetequipsTableAdapter = new Gsport.efadbDataSetTableAdapters.equipsTableAdapter();
                efadbDataSetequipsTableAdapter.Fill(efadbDataSet.equips);
                equipsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("equipsViewSource1")));
                // Cargar datos en la tabla entrenadors. Puede modificar este código según sea necesario.
                efadbDataSetentrenadorsTableAdapter = new Gsport.efadbDataSetTableAdapters.entrenadorsTableAdapter();
                efadbDataSetentrenadorsTableAdapter.Fill(efadbDataSet.entrenadors);
                entrenadorsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("entrenadorsViewSource")));
                // Cargar datos en la tabla jugador_temporada. Puede modificar este código según sea necesario.
                efadbDataSetjugador_temporadaTableAdapter = new Gsport.efadbDataSetTableAdapters.jugador_temporadaTableAdapter();
                efadbDataSetjugador_temporadaTableAdapter.Fill(efadbDataSet.jugador_temporada);
                jugador_temporadaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("jugador_temporadaViewSource")));
                // Cargar datos en la tabla posicions. Puede modificar este código según sea necesario.
                Gsport.efadbDataSetTableAdapters.posicionsTableAdapter efadbDataSetposicionsTableAdapter = new Gsport.efadbDataSetTableAdapters.posicionsTableAdapter();
                efadbDataSetposicionsTableAdapter.Fill(efadbDataSet.posicions);
                System.Windows.Data.CollectionViewSource posicionsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("posicionsViewSource")));
                // Cargar datos en la tabla divisio. Puede modificar este código según sea necesario.
                Gsport.efadbDataSetTableAdapters.divisioTableAdapter efadbDataSetdivisioTableAdapter = new Gsport.efadbDataSetTableAdapters.divisioTableAdapter();
                efadbDataSetdivisioTableAdapter.Fill(efadbDataSet.divisio);
                System.Windows.Data.CollectionViewSource divisioViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("divisioViewSource")));
                // Cargar datos en la tabla categories. Puede modificar este código según sea necesario.
                Gsport.efadbDataSetTableAdapters.categoriesTableAdapter efadbDataSetcategoriesTableAdapter = new Gsport.efadbDataSetTableAdapters.categoriesTableAdapter();
                efadbDataSetcategoriesTableAdapter.Fill(efadbDataSet.categories);
                System.Windows.Data.CollectionViewSource categoriesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("categoriesViewSource")));
                // Cargar datos en la tabla temporades. Puede modificar este código según sea necesario.
                efadbDataSettemporadesTableAdapter = new Gsport.efadbDataSetTableAdapters.temporadesTableAdapter();
                efadbDataSettemporadesTableAdapter.Fill(efadbDataSet.temporades);
                System.Windows.Data.CollectionViewSource temporadesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("temporadesViewSource")));
                // Cargar datos en la tabla equips_rivals. Puede modificar este código según sea necesario.
                efadbDataSetequips_rivalsTableAdapter = new Gsport.efadbDataSetTableAdapters.equips_rivalsTableAdapter();
                efadbDataSetequips_rivalsTableAdapter.Fill(efadbDataSet.equips_rivals);
                equips_rivalsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("equips_rivalsViewSource")));
                // Cargar datos en la tabla fases. Puede modificar este código según sea necesario.
                Gsport.efadbDataSetTableAdapters.fasesTableAdapter efadbDataSetfasesTableAdapter = new Gsport.efadbDataSetTableAdapters.fasesTableAdapter();
                efadbDataSetfasesTableAdapter.Fill(efadbDataSet.fases);
                System.Windows.Data.CollectionViewSource fasesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("fasesViewSource")));
                //Afagim una columna per el nom per mostrar al combobox de jugadors ja que a la taula de jugadors_temporada no hi ha nom
                // Cargar datos en la tabla lesions. Puede modificar este código según sea necesario.
                efadbDataSetlesionsTableAdapter = new Gsport.efadbDataSetTableAdapters.lesionsTableAdapter();
                efadbDataSetlesionsTableAdapter.Fill(efadbDataSet.lesions);
                jugadorslesionsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("jugadorslesionsViewSource")));
                // Cargar datos en la tabla convocatories. Puede modificar este código según sea necesario.
                efadbDataSetconvocatoriesTableAdapter = new Gsport.efadbDataSetTableAdapters.convocatoriesTableAdapter();
                efadbDataSetconvocatoriesTableAdapter.Fill(efadbDataSet.convocatories);
                convocatoriesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("convocatoriesViewSource")));
                // Cargar datos en la tabla partits. Puede modificar este código según sea necesario.
                efadbDataSetpartitsTableAdapter = new Gsport.efadbDataSetTableAdapters.partitsTableAdapter();
                efadbDataSetpartitsTableAdapter.Fill(efadbDataSet.partits);
                partitsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("partitsViewSource")));
                // Cargar datos en la tabla tipus_partit. Puede modificar este código según sea necesario.
                Gsport.efadbDataSetTableAdapters.tipus_partitTableAdapter efadbDataSettipus_partitTableAdapter = new Gsport.efadbDataSetTableAdapters.tipus_partitTableAdapter();
                efadbDataSettipus_partitTableAdapter.Fill(efadbDataSet.tipus_partit);
                System.Windows.Data.CollectionViewSource tipus_partitViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tipus_partitViewSource")));
                //tipus_partitViewSource.View.MoveCurrentToFirst();
                DataColumn dt = new DataColumn("nom");
                dt.DataType = typeof(String);
                efadbDataSet.Tables["jugador_temporada"].Columns.Add(dt); //el nom de la temproada en cada fila i aixi en el combobox jugadors_temporada podem veurei el nom.
                if (efadbDataSet.Tables["entrenadors"].Rows.Count == 0)
                    btnCrearEquip.IsEnabled = false;
                if (efadbDataSet.Tables["equips"].Rows.Count == 0)
                {
                    btnAfageix.IsEnabled = false;
                    btnPartits.IsEnabled = false;
                }
                if(efadbDataSet.Tables["equips_rivals"].Rows.Count == 0)
                    btnPartits.IsEnabled = false;
                if (efadbDataSet.Tables["temporades"].Rows.Count == 0)
                {
                    btnAfageix.IsEnabled = false;
                    btnImportador.IsEnabled = false;
                }
            }
            catch
            {
                MessageBox.Show("El servei no esta disponible");
                btnCerca.IsEnabled = false;
            }   
        }

        /// <summary>
        /// controla els text box que nomes es puguin posar numeros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key <=  Key.Tab)
                e.Handled = false;
            else
                e.Handled = true;
        }

        /// <summary>
        /// obre una nova finestra que importa dades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportador_Click(object sender, RoutedEventArgs e)
        {
            wndImportar wnd = new wndImportar(efadbDataSet);
            wnd.ShowDialog();
            efadbDataSetjugadorsTableAdapter.Fill(efadbDataSet.jugadors);
            efadbDataSetjugador_temporadaTableAdapter.Fill(efadbDataSet.jugador_temporada);
            efadbDataSetequipsTableAdapter.Fill(efadbDataSet.equips);
            efadbDataSetentrenadorsTableAdapter.Fill(efadbDataSet.entrenadors);
            efadbDataSetpartitsTableAdapter.Fill(efadbDataSet.partits);
            efadbDataSetequips_rivalsTableAdapter.Fill(efadbDataSet.equips_rivals);
            AmagarTot();
        }

        /// <summary>
        /// esborra l'objecta de la taula i del dataset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {    
            try
            {
                MessageBoxResult resultat = MessageBox.Show("Vols esborrar auqesta entrada?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (resultat == MessageBoxResult.Yes)
                {
                    esborrat = true;
                    if (queEs == "jugador")
                    {
                        int i = 0;
                        bool trobat = false;
                        
                        efadbDataSetusuarisTableAdapter.Update(efadbDataSet.usuaris);
                        efadbDataSetusuarisTableAdapter.Fill(efadbDataSet.usuaris,1);
                        if (efadbDataSet.Tables["jugadors"].Rows.Count > 0)
                        {
                            while (i < efadbDataSet.Tables["usuaris"].Rows.Count && !trobat)
                            {
                                if (efadbDataSet.Tables["usuaris"].Rows[i]["username"].ToString() == efadbDataSet.Tables["jugadors"].Rows[posicioRow]["dni"].ToString())
                                {
                                    trobat = true;
                                    efadbDataSet.Tables["usuaris"].Rows[i].Delete();
                                }
                                i++;
                            }
                        }
                        efadbDataSet.Tables["jugadors"].Rows[posicioRow].Delete();
                        efadbDataSetjugadorsTableAdapter.Update(efadbDataSet.jugadors);
                        efadbDataSetjugadorsTableAdapter.Fill(efadbDataSet.jugadors);
                        efadbDataSetusuarisTableAdapter.Update(efadbDataSet.usuaris);
                        efadbDataSetusuarisTableAdapter.Fill(efadbDataSet.usuaris,1);
                        efadbDataSetjugador_temporadaTableAdapter.Fill(efadbDataSet.jugador_temporada);
                        efadbDataSetlesionsTableAdapter.Fill(efadbDataSet.lesions);
                        MessageBox.Show("S'ha esborrat: " + queEs);
                    }

                    else if (queEs == "equip")
                    {
                        int i = 0;
                        bool trobat = false;
                        while (i < efadbDataSet.Tables["jugadors"].Rows.Count && !trobat)
                        {
                            if (idCercat == Convert.ToInt32(efadbDataSet.Tables["jugadors"].Rows[i]["id_equip"]))
                            {
                                trobat = true;
                            }
                            i++;
                        }
                        if(!trobat)
                        {
                            efadbDataSet.Tables["equips"].Rows[posicioRow].Delete();
                            efadbDataSetequipsTableAdapter.Update(efadbDataSet.equips);
                            efadbDataSetequipsTableAdapter.Fill(efadbDataSet.equips);
                            MessageBox.Show("S'ha esborrat: " + queEs);
                        }
                        else
                            MessageBox.Show("No pots esborrar un equip que conte jugadors, Canvia d'equip els corresponents");
                        if(efadbDataSet.Tables["equips"].Rows.Count == 0)
                        {
                            btnAfageix.IsEnabled = false;
                            btnPartits.IsEnabled = false;
                        }
                    }
                    else if (queEs == "entrenador")
                    {
                        int i = 0;
                        bool trobat = false;
                        while (i < efadbDataSet.Tables["equips"].Rows.Count && !trobat)
                        {
                            if (idCercat == Convert.ToInt32(efadbDataSet.Tables["equips"].Rows[i]["id_entrenador"]))
                            {
                                trobat = true;
                            }
                            i++;
                        }
                        if (!trobat)
                        {
                            efadbDataSet.Tables["entrenadors"].Rows[posicioRow].Delete();
                            efadbDataSetentrenadorsTableAdapter.Update(efadbDataSet.entrenadors);
                            efadbDataSetentrenadorsTableAdapter.Fill(efadbDataSet.entrenadors);
                            if(efadbDataSet.Tables["entrenadors"].Rows.Count==0)
                            {
                                btnCrearEquip.IsEnabled = false;
                            }
                            MessageBox.Show("S'ha esborrat: " + queEs);
                        }
                        else
                        {
                            MessageBox.Show("No pots esborrar un entrenador que te un equip, Modifica primer l'equip");
                        }
                        if (efadbDataSet.Tables["entrenadors"].Rows.Count == 0)
                        {
                            btnCrearEquip.IsEnabled = false;
                        }
                        
                    }
                    else if (queEs == "equiprival")
                    {
                        efadbDataSet.Tables["equips_rivals"].Rows[posicioRow].Delete();
                        efadbDataSetequips_rivalsTableAdapter.Update(efadbDataSet.equips_rivals);
                        efadbDataSetequips_rivalsTableAdapter.Fill(efadbDataSet.equips_rivals);
                        if (efadbDataSet.Tables["equips_rivals"].Rows.Count == 0)
                        {
                            btnPartits.IsEnabled = false;
                        }
                        MessageBox.Show("S'ha esborrat: " + queEs);
                    }
                    esborrat = false;
                    objectaCercat = false;
                    AmagarTot();
                    btnCerca_Click(this, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }

        /// <summary>
        /// A DINTRE DE LA INTEFICIE DE CONVOCATORIA obre una cerca nomes de partits i executa el seguent codi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTriaPartit_Click(object sender, RoutedEventArgs e)
        {
            lbJugadorsConvocats.Items.Clear();
            btnGuardarConvocatoria.IsEnabled = true;
            bool trobat = false;
            int i = 0;
            wndCercar wnd = new wndCercar(efadbDataSet, true);
            wnd.ShowDialog();
            idCercat = wnd.id;
            queEs = wnd.queEs;
            objectaCercat = true;
            try
            {
                lblPartitVs.Content = wnd.drSelect.ItemArray[10] + " VS " + wnd.drSelect.ItemArray[11] + " - Data " + Convert.ToDateTime(wnd.drSelect.ItemArray[3].ToString()).ToShortDateString();
                idEquipConvocatoria = Convert.ToInt32(wnd.drSelect.ItemArray[1]);
                while (!trobat)
                {
                    if (Convert.ToInt32(wnd.drSelect.ItemArray[1]) == Convert.ToInt32(efadbDataSet.Tables["equips"].Rows[i][0]))
                        trobat = true;
                    else
                        i++;
                }
                gridTotsJugadors.DataContext = FindResource("equipsjugadorsViewSource");
                equipsViewSource.View.MoveCurrentToPosition(i);
                if (idCercat > 0)
                    btnGuardarConvocatoria.IsEnabled = true;
                lbJugadorsConvocats.Items.Clear();
            }
            catch{}
        }

        /// <summary>
        /// comprova si esta posada en el listbox de jugadors convocats i si no el posa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void id_jugadorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool trobat = false;
            int j = 0;
            try
            {
                while (!trobat && j < lbJugadorsConvocats.Items.Count)
                {
                    if (lbJugadorsConvocats.Items[j].Equals(id_jugadorListBox.SelectedItem))
                    {
                        trobat = true;
                    }
                    j++;
                }
                if (!trobat)
                    lbJugadorsConvocats.Items.Add(id_jugadorListBox.SelectedItem);
                else
                    MessageBox.Show("Ja l'has posat");
                this.UpdateLayout();
            }
            catch{}
        }

        /// <summary>
        /// esborra l'element de la listbox de jugadors convocats
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbJugadorsConvocats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbJugadorsConvocats.Items.Remove(lbJugadorsConvocats.SelectedItem);
            this.UpdateLayout();
        }

        private void dpAnyNeixament_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            tbEdat.Content = "";
            int edat = 0;
            try
            {
                edat = Convert.ToDateTime(DateTime.Now).Year - Convert.ToDateTime(((DatePicker)sender).SelectedDate.ToString()).Year;

                if( (Convert.ToDateTime(DateTime.Now).Month) < (Convert.ToDateTime(((DatePicker)sender).SelectedDate.ToString()).Month)) {
                    edat = edat - 1;
                }

                tbEdat.Content = edat + "";
            }
            catch{}
        }

        private void tbcorreuElec_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(tbcorreuElec.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                MessageBox.Show("L'email no és correcte");
                tbcorreuElec.Text = "";
            }
        }

        private void btnLlistats_Click(object sender, RoutedEventArgs e)
        {
            wndLlistats wnd = new wndLlistats(efadbDataSet);
            wnd.ShowDialog();
        }

        private void btnLlistatEquips_Click(object sender, RoutedEventArgs e)
        {
            wndLlistatEquips wnd = new wndLlistatEquips(efadbDataSet);
            wnd.ShowDialog();
        }

        private void btnLlistatEntrenadors_Click(object sender, RoutedEventArgs e)
        {
            wndLlistatEntrenador wnd = new wndLlistatEntrenador(efadbDataSet);
            wnd.ShowDialog();
        }

        //private void btnGrafics_Click(object sender, RoutedEventArgs e)
        //{

        //} 
    }
}
