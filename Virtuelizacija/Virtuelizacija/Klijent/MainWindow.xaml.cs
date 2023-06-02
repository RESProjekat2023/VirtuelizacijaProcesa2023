using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
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
using WinForms = System.Windows.Forms;

namespace Klijent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ChannelFactory<IFileHandler> factory;
        IFileHandler proxy;
        string putanja=null;
        List<object> lista;

        public MainWindow()
        {
            InitializeComponent();
            factory = new ChannelFactory<IFileHandler>("FileService");
            proxy = factory.CreateChannel();
        }

        private void IzaberiPutanju_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                TextBlockPutanja.Text = openFileDlg.SelectedPath;
                putanja = openFileDlg.SelectedPath;
            }
            
        }

        private void PokreniCitanje_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                TextBlockGreska.Text = "";
                string measuredFilePath = "";
                string forcastedFilePath = "";

                string[] filePaths = Directory.GetDirectories(putanja); // returns: // "c:\MyDir\my-car.csv"

                foreach (string s in filePaths)
                {
                    if (s.Contains("measured"))
                    {
                        measuredFilePath = s;
                    }
                    else if (s.Contains("forecast"))
                    {
                        forcastedFilePath = s;
                    }
                }
                string[] measured = Directory.GetFiles(measuredFilePath, "*.csv");
                string[] forecast = Directory.GetFiles(forcastedFilePath, "*.csv");

                List<FileCSV> listMeasured = new List<FileCSV>();
                List<FileCSV> listForecasted = new List<FileCSV>();


                foreach (string s in measured)
                {
                    string str = (s.Split(new string[] { "measured\\" }, StringSplitOptions.None))[1];
                    MemoryStream fileStream = new MemoryStream(File.ReadAllBytes(s));
                    listMeasured.Add(new FileCSV() { FileName = str, FileStream = fileStream });
                }
                foreach (string s in forecast)
                {
                    string str = (s.Split(new string[] { "forecast\\" }, StringSplitOptions.None))[1];
                    MemoryStream fileStream = new MemoryStream(File.ReadAllBytes(s));
                    listForecasted.Add(new FileCSV() { FileName = str, FileStream = fileStream });
                }

               proxy.UploadFiles(listMeasured, listForecasted);
                List<Audit> audit = proxy.VratiKlijentuAudit();
                List<Load> load =proxy.VratiKlijentuLoad();
                List<ImportedFile> imported = proxy.VratiKlijentuImported();
                ViewDataGrid1.ItemsSource = audit;
                ViewDataGrid.ItemsSource = load;
                ViewDataGrid2.ItemsSource = imported;
                   
                   
                    // MessageBox.Show();
                
               
                DisposeStreams(listMeasured, listForecasted);
            }
            catch (Exception ex) {
                TextBlockGreska.Text=ex.Message;
                Console.WriteLine(ex.Message);
            }


            


           
        }

        private static void DisposeStreams(List<FileCSV> listMeasured, List<FileCSV> listForecasted)
        {
            foreach (FileCSV item in listMeasured)
            {
                item.Dispose();
            }

            foreach (FileCSV item in listForecasted)
            {
                item.Dispose();
            }
        }
    }
}
