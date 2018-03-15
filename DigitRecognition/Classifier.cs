using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.IO;
//using System.Windows;

namespace DigitRecognition
{
    public class Classifier
    {
        protected Random rnd = new Random(1);

        public ActivationNetwork network;
        public BackPropagationLearning teacher;

        public int inputSize;
        public int outputSize;

        public bool BREAKER = false;

        public Queue<double> trainAccQueue;
        public Queue<double> cvAccQueue;
        public Queue<double> errorQueue;

        public event EventHandler<string> networkUpdated = delegate { };
        public event EventHandler errorQueueUpdated = delegate { };
        public event EventHandler epochFinished = delegate { };
        public event EventHandler trainingFinished = delegate { };

        public int batchSize;
        public int epochs = 10;

        string activation = "Sigmoid";

        //todo: loss, optimizer(use different backprop), metrics
        public void CreateNetwork()
        {
            //инициализация сети и процесса обучения с backpropagation'ом
            network = new ActivationNetwork(new SigmoidFunction(0.4), inputSize, 30, outputSize);
            //рандомизирование весов
            network.Randomize(); //?
            foreach (var l in network.Layers)
                foreach (var n in l.Neurons)
                    for (int i = 0; i < n.Weights.Length; i++)
                        n.Weights[i] = rnd.NextDouble() * 2 - 1;

            teacher = new BackPropagationLearning(network);
            teacher.LearningRate = 1;
            teacher.Momentum = 0.4;

            networkUpdated(this, "Sigmoid");
        }

        public void CreateNetworkCustom(string activation, double coef, params int[] neuronCount)
        {
            this.activation = activation;
            network = null;
            switch (activation) {
                case "Sigmoid":
                    network = new ActivationNetwork(new SigmoidFunction(coef), inputSize, neuronCount);
                    break;
                case "Tanh":
                    network = new ActivationNetwork(new Tanh(coef), inputSize, neuronCount);
                    break;
                case "ReLU":
                    network = new ActivationNetwork(new ReLU(coef), inputSize, neuronCount);
                    break;
                default:
                    network = new ActivationNetwork(new SigmoidFunction(0.4), inputSize, 30, outputSize);
                    this.activation = "Sigmoid";
                    break;
            }
            network.Randomize();
            foreach (var l in network.Layers)
                foreach (var n in l.Neurons)
                    for (int i = 0; i < n.Weights.Length; i++)
                        n.Weights[i] = rnd.NextDouble() * 2 - 1;

            teacher = new BackPropagationLearning(network);
            teacher.LearningRate = 1;
            teacher.Momentum = 0.4;

            networkUpdated(this, activation);
        }

        double[] Normalize(double[] v)
        {
            if (activation == "Tanh") 
                return v.Select(x => (x == 0) ? -1.0 : 1.0).ToArray();
            return v;
        }

        public void Fit(double[][] inputs,  double[][] outputs, double validationSplit = 0)
        {
            trainAccQueue = new Queue<double>();
            cvAccQueue = new Queue<double>();
            errorQueue = new Queue<double>();

            int trainSetSize = (int)Math.Round(inputs.GetLength(0) * (1 - validationSplit));
            int cvSetSize = (int)Math.Round(inputs.GetLength(0) * validationSplit);

            double[][] trainSet = new double[trainSetSize][];
            double[][] trainSetAnswers = new double[trainSetSize][];
            double[][] cvSet = new double[cvSetSize][];
            double[][] cvSetAnswers = new double[cvSetSize][];

            //Shufle data

            //Divide:
            for (int i = 0; i < trainSetSize; i++)
            {
                trainSet[i] = inputs[i];
                trainSetAnswers[i] = outputs[i];
            }
            for (int i = trainSetSize; i < trainSetSize + cvSetSize; i++)
            {
                cvSet[i - trainSetSize] = inputs[i];
                cvSetAnswers[i - trainSetSize] = outputs[i];
            }

            //Run epochs:
            double trainError;
            double trainAcc;
            double cvAcc;
            double picturedError = 0;
            for (int i = 0; i < epochs; i++) {
                if (BREAKER)
                {
                    BREAKER = false;
                    trainingFinished(this, null);
                    return;
                }
                Console.WriteLine("Running epoch {0}/{1}\n", i + 1, epochs);

                trainError = 0;
                trainAcc = 0;
                cvAcc = 0;

                //run train set, compute error on train set
                for (int k = 0; k < trainSetSize; k++) {
                    double error = teacher.Run(trainSet[k], Normalize(trainSetAnswers[k]));
                    trainError += error;
                    picturedError += error;
                    if ((k + 1) % (trainSetSize / 10) == 0) {
                        errorQueue.Enqueue(picturedError);
                        picturedError = 0;
                        errorQueueUpdated(this, null);
                    }
                }

                //compute accuracy on train set
                for (int k = 0; k < trainSetSize; k++)
                {
                    double[] output = network.Compute(trainSet[k]);
                    if (output.Argmax() == trainSetAnswers[k].Argmax())
                    {
                        trainAcc++;
                    }
                }

                //compute accuracy on cv set
                for (int k = 0; k < cvSetSize; k++)
                {
                    double[] output = network.Compute(cvSet[k]);
                    if (output.Argmax() == cvSetAnswers[k].Argmax()) {
                        cvAcc++;
                    }
                }

                //plots
                trainAccQueue.Enqueue(trainAcc / trainSetSize);
                cvAccQueue.Enqueue(cvAcc / cvSetSize);
                epochFinished(this, null);
                //log
                //Console.WriteLine("train set error = {0:0.0000} | train set accuracy = {1:0.0000} | validation set accuracy = {2:0.0000}\n", trainError, trainAcc/trainSetSize, cvAcc/cvSetSize);
                //MessageBox.Show("train set error = " + trainError + " \n train set accuracy = " + (trainAcc / trainSetSize) + " \n validation set accuracy = " + (cvAcc / cvSetSize));
            }
            trainingFinished(this, null);
        }

        public double[,] Test(double[][] inputs, double[][] answers)
        {
            double[,] errorMtx = new double[outputSize, outputSize];
            int[] counts = new int[outputSize];
            int n = inputs.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                double[] output = Predict(inputs[i]);
                int x = output.Argmax();
                int y = answers[i].Argmax();

                errorMtx[x, y] += 1;
                counts[y] += 1;
            }

            for (int i = 0; i < outputSize; i++)
                for (int j = 0; j < outputSize; j++)
                {
                    errorMtx[i, j] /= counts[j];
                    errorMtx[i, j] = Math.Round(errorMtx[i, j], 3);
                }

            return errorMtx;
        }

        public double[] Predict (double[] input)
        {
            return network.Compute(input);
        }

        public void SaveNetwork(string fname)
        {
            network.Save<ActivationNetwork>(fname);
        }

        public void LoadNetwork(string fname)
        {
            network = Serializer.Load<ActivationNetwork>(fname);
            teacher = new BackPropagationLearning(network);
        }
    }
}
