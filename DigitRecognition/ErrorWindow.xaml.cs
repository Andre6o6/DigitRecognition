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

namespace DigitRecognition
{
    public partial class ErrorWindow : Window
    {
        double[,] errorMtx;

        public ErrorWindow()
        {
            InitializeComponent();
        }

        public ErrorWindow(double[,] errorMtx)
        {
            InitializeComponent();
            this.errorMtx = errorMtx;
            if (errorMtx == null)
                return;

            System.Data.DataTable t = new System.Data.DataTable();
            for (int i = 0; i < errorMtx.GetLength(0); i++)
                t.Columns.Add(i.ToString(), typeof(double));

            for (int i = 0; i < errorMtx.GetLength(0); i++)
            {
                t.Rows.Add( errorMtx[i,0], errorMtx[i,1], errorMtx[i,2], errorMtx[i,3], errorMtx[i,4],
                            errorMtx[i,5], errorMtx[i,6], errorMtx[i,7], errorMtx[i,8], errorMtx[i,9]);
            }
            
            dataGrid.ItemsSource = t.DefaultView;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
