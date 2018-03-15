using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DigitRecognition
{
    class MnistData
    {
        public double[][] Inputs;
        public double[][] Answers;
        public int inputSize { get; private set; }
        public int outputSize { get; private set; }
        int sizeX = 28;
        int sizeY = 28;

        public int ImportSet(string fname)
        {
            int images;
            int columns;
            int rows;

            //Загрузка архива изображений:
            FileStream file = new FileStream(fname + "-images.idx3-ubyte", FileMode.Open);
            //загружаем header
            byte[] data = new byte[16];
            file.Read(data, 0, 16);

            images = data[7] + 256 * data[6];
            rows = data[11];
            columns = data[15];

            //загружаем информацию о пикселах
            data = new byte[rows * columns];
            List<double[]> inputVectors = new List<double[]>();
            for (int i = 0; i < images; i++)
            {
                file.Read(data, 0, rows * columns);

                //TODO downscale to 16*16

                double[] vector = data.Select(z => z > 100 ? 1.0 : 0.0).ToArray();
                inputVectors.Add(vector);
            }
            file.Close();
            //Загрузка архива лэйблов:
            file = new FileStream(fname + "-labels.idx1-ubyte", FileMode.Open);
            //загружаем header
            data = new byte[8];
            file.Read(data, 0, 8);
            //загружаем лэйблы
            int label;
            List<double[]> outputVectors = new List<double[]>();
            for (int i = 0; i < images; i++)
            {
                label = file.ReadByte();
                double[] vector = Enumerable.Range(0, 10).Select(z => z == label ? 1.0 : 0.0).ToArray();
                outputVectors.Add(vector);
            }

            inputSize = rows * columns;
            outputSize = 10;
            Inputs = inputVectors.ToArray();
            Answers = outputVectors.ToArray();

            file.Close();
            return images;
        }
        
    }
}
