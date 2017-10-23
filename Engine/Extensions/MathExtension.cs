using System;

namespace Engine.Extensions
{
    public static class MathStuff
    {
        public static double Sigmoid(double value)
        {
            double k = Math.Exp(value);
            return k / (1.0d + k);
        }

        public static float Sigmoid(float value)
        {
            double k = Math.Exp(value);
            return (float) (k / (1.0f + k));
        }
    }
}