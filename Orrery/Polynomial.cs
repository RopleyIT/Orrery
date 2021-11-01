namespace Orrery
{
    public class Polynomial
    {
        private readonly double[] coefficients;

        public Polynomial(params double[] values)
            => coefficients = values;

        public double At(double x)
        {
            double power = 1;
            double result = 0;
            for (int i = 0; i < coefficients.Length; i++)
            {
                result += power * coefficients[i];
                power *= x;
            }
            return result;
        }

        public override string ToString()
        {
            string result = string.Empty;
            for (int i = 0; i < coefficients.Length; i++)
            {
                if (i > 0 && coefficients[i] > 0)
                    result += "+";
                result += $"{coefficients[i]}";
                for (int j = 0; j < i; j++)
                    result += "*x";
            }
            return result;
        }
    }
}
