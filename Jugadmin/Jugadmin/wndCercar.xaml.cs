using EstructuraClasses;
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

namespace Jugadmin
{
    /// <summary>
    /// Lógica de interacción para wndCercar.xaml
    /// </summary>
    public partial class wndCercar : Window
    {
        Dictionary<string, Equip> equips;
        public string nom;
        public string nomEquip;
        public wndCercar()
        {
            InitializeComponent();
        }

        public wndCercar(Dictionary<string,Equip> equipsIm)
        {
            InitializeComponent();
            this.equips = equipsIm;
            CanviarTaula();
            lbCategoriaCerca.Items.Add("Any");
            lbCategoriaCerca.Items.Add("Edat");
            lbCategoriaCerca.Items.Add("Nom");
            if(rbEquip.IsChecked == true)
            {
                lbCategoriaCerca.Opacity = 0;
                lbCategoriaCerca.IsEnabled = false;
            }
        }

        public void CanviarTaula()
        {
            if (rbEquip.IsChecked == true)
            {
                DataGridTextColumn dtcNom = new DataGridTextColumn();
                DataGridTextColumn dtcCategoria = new DataGridTextColumn();
                DataGridTextColumn dtcTipus = new DataGridTextColumn();
                DataGridTextColumn dtcNumJugadors = new DataGridTextColumn();
                dtcNom.Header = "Nom";
                dtcCategoria.Header = "Categoria";
                dtcTipus.Header = "Tipus";
                dtcNumJugadors.Header = "NumJugadors";
                dtcNom.Binding = new Binding("Nom");
                dtcCategoria.Binding = new Binding("Categoria");
                dtcTipus.Binding = new Binding("Tipus");
                dtcNumJugadors.Binding = new Binding("NumJugadors");
                dgResultat.Columns.Add(dtcNom);
                dgResultat.Columns.Add(dtcCategoria);
                dgResultat.Columns.Add(dtcTipus);
                dgResultat.Columns.Add(dtcNumJugadors);              
            }
            else
            {
                DataGridTemplateColumn dtcImatge = new DataGridTemplateColumn();
                dtcImatge.Header = "Foto";
                FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(Image));
                Binding b1 = new Binding("NomImg");
                b1.Mode = BindingMode.TwoWay;
                factory1.SetValue(Image.SourceProperty, b1);
                DataTemplate cellTemplate1 = new DataTemplate();
                cellTemplate1.VisualTree = factory1;
                dtcImatge.CellTemplate = cellTemplate1;
                dtcImatge.Width = 90;

                DataGridTextColumn dtcNom = new DataGridTextColumn();
                DataGridTextColumn dtcEdat = new DataGridTextColumn();
                DataGridTextColumn dtcAny = new DataGridTextColumn();
                DataGridTextColumn dtcEquip = new DataGridTextColumn();
                DataGridTextColumn dtcCognoms = new DataGridTextColumn();
                
                dtcNom.Header = "Nom";
                dtcImatge.Header = "Foto";
                dtcEquip.Header = "Equip";
                dtcAny.Header = "AnyNeixament";
                dtcEdat.Header = "Edat";
                dtcCognoms.Header = "Cognoms";
                dtcNom.Binding = new Binding("Nom");
                dtcCognoms.Binding = new Binding("Cognoms");
                dtcAny.Binding = new Binding("AnyNeixament");
                dtcEdat.Binding = new Binding("Edat");
                dtcEquip.Binding = new Binding("NomEquip");
                dgResultat.Columns.Add(dtcImatge);
                dgResultat.Columns.Add(dtcNom);
                dgResultat.Columns.Add(dtcCognoms);
                dgResultat.Columns.Add(dtcEdat);
                dgResultat.Columns.Add(dtcAny);
                dgResultat.Columns.Add(dtcEquip);
                
            }
        }

        private void tbBusca_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgResultat.Items.Clear();
            if(rbEquip.IsChecked == true)
            {
                foreach(string eq in equips.Keys)
                {
                        if (eq.Contains(((TextBox)sender).Text.Trim()))
                            dgResultat.Items.Add(new Equip() { Nom = eq, Categoria = equips[eq].Categoria, Tipus = equips[eq].Tipus, NumJugadors = equips[eq].NumJugadors });  
                }
            }
            else
            {
                bool noSelecionat = false ;
                foreach (Equip eq in equips.Values)
                {
                    foreach (Jugador j in eq)
                    {
                        if (lbCategoriaCerca.SelectedItem != null)
                        {
                            if ("nom" == lbCategoriaCerca.SelectedItem.ToString().ToLower())
                            {
                                if (j.Nom.Contains(((TextBox)sender).Text.Trim()))
                                {

                                    dgResultat.Items.Add(new Jugador() { NomImg = "../../Fotos/" + j.NomImg, Nom = j.Nom, Edat = j.Edat, AnyNeixament = j.AnyNeixament, NomEquip = j.NomEquip, Cognoms = j.Cognoms});
                                }
                            }
                            else if ("any" == lbCategoriaCerca.SelectedItem.ToString().ToLower())
                            {
                                if (j.AnyNeixament.ToString().Contains(((TextBox)sender).Text.Trim()))
                                {
                                    dgResultat.Items.Add(new Jugador() { NomImg = "../../Fotos/" + j.NomImg, Nom = j.Nom, Edat = j.Edat, AnyNeixament = j.AnyNeixament, NomEquip = j.NomEquip, Cognoms = j.Cognoms});
                                }
                            }
                            else
                            {
                                if (j.Edat.ToString().Contains(((TextBox)sender).Text.Trim()))
                                {
                                    dgResultat.Items.Add(new Jugador() { NomImg = "../../Fotos/" + j.NomImg, Nom = j.Nom, Edat = j.Edat, AnyNeixament = j.AnyNeixament, NomEquip = j.NomEquip, Cognoms = j.Cognoms});
                                }
                            }
                        }
                        else
                        {
                            noSelecionat = true;
                        }
                    }
                }
                if(noSelecionat)
                {
                    tbBusca.Text = "";
                    MessageBox.Show("No s'ha introduit el parametre de cerca");
                    
                }
            }
        }

        private void rbJugador_Click(object sender, RoutedEventArgs e)
        {
            dgResultat.Columns.Clear();
            dgResultat.Items.Clear();
            CanviarTaula();
        }

        private void rbEquip_Click(object sender, RoutedEventArgs e)
        {
            dgResultat.Columns.Clear();
            dgResultat.Items.Clear();
            CanviarTaula();
        }

        private void rbJugador_Checked(object sender, RoutedEventArgs e)
        {
                lbCategoriaCerca.Opacity = 100;
                lbCategoriaCerca.IsEnabled = true;
        }

        private void dgResultat_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            nom = ((Jugador)(((DataGrid)sender).SelectedItem)).Nom;
            nomEquip = ((Jugador)(((DataGrid)sender).SelectedItem)).NomEquip;
            this.Close();
        }
    }
}
