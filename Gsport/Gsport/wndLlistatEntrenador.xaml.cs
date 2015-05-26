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
    /// Lógica de interacción para wndLlistatEntrenador.xaml
    /// </summary>
    public partial class wndLlistatEntrenador : Window
    {
        efadbDataSet datasetAux = new efadbDataSet();
        public wndLlistatEntrenador(efadbDataSet ds)
        {
            InitializeComponent();
            datasetAux = ds;
            _reportViewer.Load += _reportViewerEntrenadors_Load;
        }

        private void _reportViewerEntrenadors_Load(object sender, EventArgs e)
        {
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new
            Microsoft.Reporting.WinForms.ReportDataSource();
            //efadbDataSet dataset = new efadbDataSet();
            datasetAux.BeginInit();
            reportDataSource1.Name = "dsEntrenadors";
            //Name of the report dataset in our .RDLC file
            reportDataSource1.Value = datasetAux.entrenadors;
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            this._reportViewer.LocalReport.ReportPath = "../../ReportEntrenadors.rdlc";
            datasetAux.EndInit();
            //fill data into WpfApplication4DataSet
            efadbDataSetTableAdapters.entrenadorsTableAdapter
            accountsTableAdapter = new
            efadbDataSetTableAdapters.entrenadorsTableAdapter();

            accountsTableAdapter.ClearBeforeFill = true;
            accountsTableAdapter.Fill(datasetAux.entrenadors);
            _reportViewer.RefreshReport();
            //_isReportViewerLoaded = true;
        }
    }
}
