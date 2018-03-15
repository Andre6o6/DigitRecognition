using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Accord.Controls;
using ZedGraph;
using Microsoft.Win32;

namespace DigitRecognition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Classifier clf;
        MnistData trainSet = new MnistData();
        MnistData testSet = new MnistData();

        ZedGraphControl zedGraph;
        GraphPane errorGraph;
        PointPairList errorPointsList = new PointPairList();
        double errorIndex = 0;

        ZedGraphControl accZedGraph;
        GraphPane accGraph;
        PointPairList trainAccPointsList = new PointPairList();
        PointPairList cvAccPointsList = new PointPairList();
        double epochIndex = 0;

        double[,] errorMtx;

        Brush defaultSelectionBrush;
        //bool training = false;
        //
        WriteableBitmap wbmap;
        byte[] pixels;
        byte[] smoothPixels;

        public MainWindow()
        {
            InitializeComponent();

            trainSet.ImportSet("data/mnist/train");
            testSet.ImportSet("data/mnist/t10k");

            clf = new Classifier()
            {
                inputSize = 784,
                outputSize = 10
            };
            clf.CreateNetwork();

            clf.networkUpdated += OnNetworkUpdate;
            clf.epochFinished += OnEpochFinish;
            clf.errorQueueUpdated += OnErrorQueueUpdate;
            clf.trainingFinished += OnTrainingFinished;

            zedGraph = errorGraphControl;
            errorGraph = zedGraph.GraphPane;
            errorGraph.Title.Text = "Среднеквадратическая ошибка";
            errorGraph.XAxis.Title.Text = "k";
            errorGraph.YAxis.Title.Text = "MSE";

            accZedGraph = accGraphControl;
            accGraph = accZedGraph.GraphPane;
            accGraph.Title.Text = "Точность";
            accGraph.XAxis.Title.Text = "#epoch";
            accGraph.YAxis.Title.Text = "acc";
            //trainAccPointsList.Add(0, 0);
            //cvAccPointsList.Add(0, 0);

            defaultSelectionBrush = epochsTextBox.SelectionBrush;

            //
            wbmap = new WriteableBitmap(28, 28, 300, 300, PixelFormats.Gray8, null);
            imageDigitToRec.Source = wbmap;

            pixels = new byte[wbmap.PixelHeight * wbmap.PixelWidth * wbmap.Format.BitsPerPixel / 8];
            wbmap.WritePixels(new Int32Rect(0, 0, wbmap.PixelWidth, wbmap.PixelHeight),pixels,wbmap.PixelWidth * wbmap.Format.BitsPerPixel / 8, 0);

            smoothPixels = new byte[pixels.Length];
        }

        void OnNetworkUpdate(object sender, string e) {
            ChangeConfiguration(e);
            buttonSave.IsEnabled = false;
        }

        void OnEpochFinish(object sender, EventArgs e)
        {
            this.accZedGraph.BeginInvoke(new Action(UpdateAccuracyGraph));
        }

        void OnErrorQueueUpdate(object sender, EventArgs e)
        {
            this.zedGraph.BeginInvoke(new Action(UpdateErrorGraph));
        }

        void OnTrainingFinished(object sender, EventArgs e)
        {
            this.zedGraph.BeginInvoke(new Action(() => {
                buttonTrain.IsEnabled = true;
                buttonSave.IsEnabled = true;
                buttonLoad.IsEnabled = true;
                buttonStop.IsEnabled = false;
            }));
           
            MessageBox.Show("Training complete");

            this.zedGraph.BeginInvoke(new Action(Test));
            
        }

        void Test()
        {
            errorMtx = clf.Test(testSet.Inputs, testSet.Answers);

            double testAcc = 0;
            for (int i = 0; i < testSet.outputSize; i++)
                testAcc += errorMtx[i, i];

            labelTestAcc.Content = string.Format("{0:0.0000}", testAcc / clf.outputSize);
        }

        void UpdateAccuracyGraph()
        {
            double trainAcc = clf.trainAccQueue.Dequeue();
            double cvAcc = clf.cvAccQueue.Dequeue();

            accGraph.CurveList.Clear();
            trainAccPointsList.Add(new PointPair(epochIndex, trainAcc));
            cvAccPointsList.Add(new PointPair(epochIndex, cvAcc));
            LineItem trainAccCurve = accGraph.AddCurve("train error", trainAccPointsList, System.Drawing.Color.Blue, SymbolType.None);
            LineItem cvAccCurve = accGraph.AddCurve("validation error", cvAccPointsList, System.Drawing.Color.Red, SymbolType.None);
            accZedGraph.AxisChange();
            accZedGraph.Refresh();

            epochIndex += 1;
        }
        void UpdateErrorGraph()
        {
            double error = clf.errorQueue.Dequeue();

            errorGraph.CurveList.Clear();
            errorPointsList.Add(new PointPair(errorIndex, error));
            LineItem errorCurve = errorGraph.AddCurve("Error", errorPointsList, System.Drawing.Color.Blue, SymbolType.None);
            zedGraph.AxisChange();
            //zedGraph.Invalidate();
            zedGraph.Refresh();

            errorIndex += 1;
        }
        void ClearGraphs()
        {
            accGraph.CurveList.Clear();
            trainAccPointsList.Clear();
            cvAccPointsList.Clear();
            epochIndex = 0;

            accZedGraph.AxisChange();
            accZedGraph.Refresh();

            errorGraph.CurveList.Clear();
            errorPointsList.Clear();
            errorIndex = 0;

            zedGraph.AxisChange();
            zedGraph.Refresh();
        }

        void ChangeConfiguration(string e)
        {
            activationFuncLabel.Content = e;
            string hlNeuronCount = "";
            for (int i = 0; i < clf.network.Layers.Count() - 1; i++)
                hlNeuronCount += clf.network.Layers[i].Neurons.Length + " - ";

            configurationLabel.Content = "784 - " + hlNeuronCount + "10";
        }

        double[] PredictImage()
        {
            double[] input = pixels.Select(x => x / 256.0).ToArray();
            //double[] input = pixels.Select(x => (x > 100) ? 1.0 : 0.0 ).ToArray();
            return clf.Predict(input);
        }

        void DrawPixel(double x, double y, byte c)
        {
            
            int unitX = (int)(x / imageDigitToRec.Width * wbmap.PixelWidth);
            int unitY = (int)(y / imageDigitToRec.Height * wbmap.PixelHeight);
            if (unitX > 27) unitX = 27;
            if (unitY > 27) unitY = 27;
            pixels[unitY * wbmap.PixelWidth + unitX] = c;

            if (unitX > 0 && unitX < 27 && unitY > 0 && unitY < 27)
            {
                pixels[unitY * wbmap.PixelWidth + unitX + 1] = c;
                pixels[unitY * wbmap.PixelWidth + unitX - 1] = c;
                pixels[(unitY + 1) * wbmap.PixelWidth + unitX] = c;
                pixels[(unitY - 1) * wbmap.PixelWidth + unitX] = c;

                //pixels[(unitY + 1) * wbmap.PixelWidth + unitX + 1] = (byte)(Math.Min(pixels[(unitY + 1) * wbmap.PixelWidth + unitX + 1] + c / 10, 255));
                //pixels[(unitY - 1) * wbmap.PixelWidth + unitX + 1] = (byte)(Math.Min(pixels[(unitY - 1) * wbmap.PixelWidth + unitX + 1] + c / 10, 255));
                //pixels[(unitY + 1) * wbmap.PixelWidth + unitX - 1] = (byte)(Math.Min(pixels[(unitY + 1) * wbmap.PixelWidth + unitX - 1] + c / 10, 255));
                //pixels[(unitY - 1) * wbmap.PixelWidth + unitX - 1] = (byte)(Math.Min(pixels[(unitY - 1) * wbmap.PixelWidth + unitX - 1] + c / 10, 255));

                pixels[(unitY + 1) * wbmap.PixelWidth + unitX + 1] = Math.Max(pixels[(unitY + 1) * wbmap.PixelWidth + unitX + 1], (byte)127);
                pixels[(unitY - 1) * wbmap.PixelWidth + unitX + 1] = Math.Max(pixels[(unitY - 1) * wbmap.PixelWidth + unitX + 1], (byte)127);
                pixels[(unitY + 1) * wbmap.PixelWidth + unitX - 1] = Math.Max(pixels[(unitY + 1) * wbmap.PixelWidth + unitX - 1], (byte)127);
                pixels[(unitY - 1) * wbmap.PixelWidth + unitX - 1] = Math.Max(pixels[(unitY - 1) * wbmap.PixelWidth + unitX - 1], (byte)127);
            }

            wbmap.WritePixels(new Int32Rect(0, 0, wbmap.PixelWidth, wbmap.PixelHeight), pixels, wbmap.PixelWidth * wbmap.Format.BitsPerPixel / 8, 0);
        }

        private void buttonTrain_Click(object sender, RoutedEventArgs e)
        {
            new Action<double[][], double[][], double>(clf.Fit).BeginInvoke(trainSet.Inputs, trainSet.Answers, 0.1, null, null);
            buttonTrain.IsEnabled = false;
            buttonSave.IsEnabled = false;
            buttonLoad.IsEnabled = false;
            buttonStop.IsEnabled = true;
            
            //proverka na running, plots, callback? or event uptada graficov
            //loading mnista v otdel thread
        }

        private void reconfigureButton_Click(object sender, RoutedEventArgs e)
        {
            NetConfigurationWindow w = new NetConfigurationWindow(clf);
            w.ShowDialog();
        }

        private void applyLearningRateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clf.teacher.LearningRate = Convert.ToDouble(learningRateTextBox.Text);
            }
            catch (FormatException er)
            {
                MessageBoxResult answer = MessageBox.Show("Неверный формат введенных данных","Ошибка", MessageBoxButton.OKCancel);
                if (answer == MessageBoxResult.OK)
                {
                    learningRateTextBox.Focus();
                    learningRateTextBox.SelectAll();
                }
                else
                {
                    learningRateTextBox.Text = "1";
                }
            }
        }

        private void applyMomentumButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clf.teacher.Momentum = Convert.ToDouble(momentumTextBox.Text);
            }
            catch (FormatException er)
            {
                MessageBoxResult answer = MessageBox.Show("Неверный формат введенных данных", "Ошибка", MessageBoxButton.OKCancel);
                if (answer == MessageBoxResult.OK)
                {
                    momentumTextBox.Focus();
                    momentumTextBox.SelectAll();
                }
                else
                {
                    momentumTextBox.Text = "0";
                }
            }
        }

        private void epochsTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (epochsTextBox.Text.IsNumber())
            {
                epochsTextBox.SelectionBrush = defaultSelectionBrush;
                clf.epochs = Convert.ToInt16(epochsTextBox.Text);
            }
            else
            {
                //epochsTextBox.SelectionBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                epochsTextBox.Text = "0";
            }
        }

        private void epochsTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (epochsTextBox.Text.IsNumber())
                {
                    epochsTextBox.SelectionBrush = defaultSelectionBrush;
                    clf.epochs = Convert.ToInt16(epochsTextBox.Text);
                }
                else
                {
                    epochsTextBox.SelectionBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    epochsTextBox.SelectAll();
                }
            }
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            clf.BREAKER = true;
            buttonStop.IsEnabled = false;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Accord.NET network|*.accord";
            string fname;
            if (saveFileDialog.ShowDialog() == true)
            {
                fname = saveFileDialog.FileName;
                clf.SaveNetwork(fname);
                MessageBox.Show("Saved");
            }
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Accord.NET network|*.accord";
            string fname;
            if (openFileDialog.ShowDialog() == true) {
                fname = openFileDialog.FileName;
                clf.LoadNetwork(fname);
                MessageBox.Show("Loaded");
                this.zedGraph.BeginInvoke(new Action(Test));
            }
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            clf.CreateNetwork();
            MessageBox.Show("NN has been reset.");
        }

        private void buttonClearGraphs_Click(object sender, RoutedEventArgs e)
        {
            ClearGraphs();
        }

        private void buttonShowTestDetails_Click(object sender, RoutedEventArgs e)
        {
            ErrorWindow w = new ErrorWindow(errorMtx);
            w.ShowDialog();
        }

        private void imageDigitToRec_MouseMove(object sender, MouseEventArgs e)
        {
            double x = e.GetPosition(imageDigitToRec).X;
            double y = e.GetPosition(imageDigitToRec).Y;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DrawPixel(x, y, 0xff);
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                DrawPixel(x, y, 0x00);
            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = 0x00;
            wbmap.WritePixels(new Int32Rect(0, 0, wbmap.PixelWidth, wbmap.PixelHeight), pixels, wbmap.PixelWidth * wbmap.Format.BitsPerPixel / 8, 0);
        }

        private void buttonPredict_Click(object sender, RoutedEventArgs e)
        {
            double[] output = PredictImage();
            int index = output.Argmax();
            labelAnswer.Content = index;
            labelProb.Content = String.Format("{0:0.000}", output[index]);
            string s = "";
            for (int i = 0; i < 10; i++)
                s += String.Format("{0}={1:0.00}; ", i, output[i]);
            labelProbVector.Content = s;
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            labelBeforeProbVec.Visibility = Visibility.Visible;
            labelProbVector.Visibility = Visibility.Visible;
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            labelBeforeProbVec.Visibility = Visibility.Hidden;
            labelProbVector.Visibility = Visibility.Hidden;
        }

    }
}
