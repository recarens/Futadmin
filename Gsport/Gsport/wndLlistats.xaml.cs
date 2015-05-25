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
    /// Lógica de interacción para wndLlistats.xaml
    /// </summary>
    public partial class wndLlistats : Window
    {
        efadbDataSet datasetAux = new efadbDataSet();
        public wndLlistats(efadbDataSet ds)
        {
            InitializeComponent();
            datasetAux = ds;
            _reportViewer.Load += _reportViewer_Load;
        }
        void _reportViewer_Load(object sender, EventArgs e)
        {
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new
            Microsoft.Reporting.WinForms.ReportDataSource();
            //efadbDataSet dataset = new efadbDataSet();
            datasetAux.BeginInit();
            reportDataSource1.Name = "dsInformes";
            //Name of the report dataset in our .RDLC file
            reportDataSource1.Value = datasetAux.jugadors;
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            this._reportViewer.LocalReport.ReportPath = "../../Report1.rdlc";
            datasetAux.EndInit();
            //fill data into WpfApplication4DataSet
            efadbDataSetTableAdapters.jugadorsTableAdapter
            accountsTableAdapter = new
            efadbDataSetTableAdapters.jugadorsTableAdapter();

            accountsTableAdapter.ClearBeforeFill = true;
            accountsTableAdapter.Fill(datasetAux.jugadors);
            _reportViewer.RefreshReport();
            //_isReportViewerLoaded = true;
        }
    }
}
