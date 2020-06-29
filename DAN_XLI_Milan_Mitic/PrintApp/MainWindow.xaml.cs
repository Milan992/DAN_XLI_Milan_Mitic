using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrintApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string text = "";
        static int numberOfCopies = 1;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void BtnCancel_Click(object sender, EventArgs e)
        {

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < numberOfCopies; i++)
            {
                string location = (i + 1).ToString() +"." + DateTime.Now.ToString("dd_MM_yyyy_hh_mm");
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
        }

        private void PbProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
