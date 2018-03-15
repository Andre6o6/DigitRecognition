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
using System.Collections.ObjectModel;

namespace DigitRecognition
{
    public partial class NetConfigurationWindow : Window
    {
        public class LayerInfo
        {
            public int layer { get; set; }
            public int neuronCount { get; set; }
            public string function { get; set; }
        }
        public ObservableCollection<LayerInfo> Collection { get; set; }
        public List<LayerInfo> layersInfo { get; set; }


        Classifier clf;
        int layerCount;

        public NetConfigurationWindow()
        {
            InitializeComponent();
        }

        public NetConfigurationWindow(Classifier clf)
        {
            InitializeComponent();
            this.clf = clf;
            layerCount = clf.network.Layers.Count() - 1;

            layersInfo = new List<LayerInfo>();
            layersInfo.Add(new LayerInfo() { layer = 0, neuronCount = clf.network.Layers[0].Neurons.Count(), function = " " });
            Collection = new ObservableCollection<LayerInfo>();
            ShowCollection();
            layersDataGrid.ItemsSource = Collection;

            layersDataGrid.Columns[0].IsReadOnly = true;
            layersDataGrid.Columns[2].IsReadOnly = true;
        }

        void ShowCollection()
        {
            Collection.Clear();
            foreach (var l in layersInfo)
            {
                Collection.Add(l);
            }
        }

        void ChangeLayerCount(int newCount)
        {
            string func = ActivationFuncComboBox.Text;
            if (layerCount > newCount)
            {
                layersInfo.RemoveRange(newCount, layerCount - newCount);
            }
            if (layerCount < newCount)
            {
                for (int i = layerCount; i < newCount; i++)
                {
                    layersInfo.Add(new LayerInfo() { layer = i, neuronCount = 30, function = func });
                }
            }
            layerCount = newCount;
        }

        bool UpdateGrid()
        {
            if (!layerCountTextBox.Text.IsNumber())
            {
                layerCountTextBox.Text = "1";
                return false;
            }

            int newLayerCount = Convert.ToInt32(layerCountTextBox.Text);
            ChangeLayerCount(newLayerCount);
            ShowCollection();
            return true;
        }

        void CommitUpdates()
        {
            string act = ActivationFuncComboBox.Text;
            double coef = Convert.ToDouble(coefficientTextBox.Text);
            
            int layerCount = Convert.ToInt32(layerCountTextBox.Text);
            int[] neuronCount = new int[layerCount + 1];
            neuronCount[layerCount] = clf.outputSize;

            //Some checking

            for (int i = 0; i < layerCount; i++)
                neuronCount[i] = layersInfo[i].neuronCount;

            clf.CreateNetworkCustom(act, coef, neuronCount);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            CommitUpdates();
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void layerCountTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void layerCountTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!UpdateGrid())
                {
                    layerCountTextBox.SelectAll();
                }
            }
        }

        private void ActivationFuncComboBox_DropDownClosed(object sender, EventArgs e)
        {
            foreach (LayerInfo l in layersInfo)
            {
                l.function = ActivationFuncComboBox.Text;
            }
            ShowCollection();
        }
    }

}
