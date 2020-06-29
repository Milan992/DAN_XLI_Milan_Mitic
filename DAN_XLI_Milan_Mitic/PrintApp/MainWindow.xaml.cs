using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

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

            btnCancel.IsEnabled = false;
            btnPrint.IsEnabled = false;
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbProgress.Value = e.ProgressPercentage;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnCancel.IsEnabled = false;

            MessageBox.Show("Printing completed.");
            pbProgress.Value = 0;
        }

        /// <summary>
        /// Creates number of txt files depending on number of copies and writes the text from text box in them.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //btnCancel.IsEnabled = true;

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

        /// <summary>
        /// Cancels futher progress during printing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            btnCancel.IsEnabled = true;

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
            btnPrint.IsEnabled = true;

            TextBox objTextBox = (TextBox)sender;
            // text from text box
            text = objTextBox.Text;

            if (string.IsNullOrEmpty(text))
            {
                btnPrint.IsEnabled = false;
            }
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
