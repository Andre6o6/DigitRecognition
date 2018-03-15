using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Neuro;

namespace DigitRecognition
{
    public class ReLU : IActivationFunction
    {
        public double alpha;

        public ReLU(double alpha = 1)
        {
            this.alpha = alpha;
        }

        public double Function(double x)
        {
            return (x > 0) ? alpha * x : 0.0;
        }

        public double Derivative(double x)
        {
            return (x > 0) ? alpha : 0.0; 
        }

        public double Derivative2(double x)
        {
            return 0;
        }
    }
}
