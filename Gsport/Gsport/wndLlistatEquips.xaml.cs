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
    /// Lógica de interacción para wndLlistatEquips.xaml
    /// </summary>
    public partial class wndLlistatEquips : Window
    {
        efadbDataSet datasetAux = new efadbDataSet();
        public wndLlistatEquips(efadbDataSet ds)
        {
            InitializeComponent();
            datasetAux = ds;
            _reportViewer.Load += _reportViewerEquip_Load;
        }

        void _reportViewerEquip_Load(object sender, EventArgs e)
        {
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new
            Microsoft.Reporting.WinForms.ReportDataSource();
            //efadbDataSet dataset = new efadbDataSet();
            datasetAux.BeginInit();
            reportDataSource1.Name = "dtPartits";//son equips
            //Name of the report dataset in our .RDLC file
            reportDataSource1.Value = datasetAux.equips;
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            this._reportViewer.LocalReport.ReportPath = "../../ReportEquips.rdlc";
            datasetAux.EndInit();
            //fill data into WpfApplication4DataSet
            efadbDataSetTableAdapters.equipsTableAdapter
            accountsTableAdapter = new
            efadbDataSetTableAdapters.equipsTableAdapter();

            accountsTableAdapter.ClearBeforeFill = true;
            accountsTableAdapter.Fill(datasetAux.equips);
            _reportViewer.RefreshReport();
            //_isReportViewerLoaded = true;
        }
    }
}
