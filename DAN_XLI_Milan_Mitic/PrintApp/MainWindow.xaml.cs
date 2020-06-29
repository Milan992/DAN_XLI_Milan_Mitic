using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace PrintApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string text;
        static int numberOfCopies = 1;
        private BackgroundWorker worker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();

            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbProgress.Value = e.ProgressPercentage;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Printing completed.");
            pbProgress.Value = 0;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < numberOfCopies; i++)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Thread.Sleep(1000);
                string location = (i + 1).ToString() + "." + DateTime.Now.ToString("dd_MM_yyyy_hh_mm");
                try
                {
                    using (StreamWriter sw = new StreamWriter(@"..\..\" + location + ".txt"))
                    {
                        sw.Write(text);
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
                worker.ReportProgress((i + 1) * (100 / numberOfCopies));
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                worker.RunWorkerAsync();
            }
            catch 
            {
                MessageBox.Show("Already printing. . .");
            }
        }

        private void TxtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox objTextBox = (TextBox)sender;
            text = objTextBox.Text;
        }

        private void TxtNumberOfCopies_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox objTextBox = (TextBox)sender;
            int.TryParse(objTextBox.Text, out numberOfCopies);
            if (!int.TryParse(objTextBox.Text, out numberOfCopies))
            {
                numberOfCopies = 1;
            }
        }

        private void PbProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }
    }
}
