namespace Core.Maths
{
    public class Interpolation
    {
        public static float Linear(float start, float end, float fraction)
        {
            return (float)(start * (1.0 - fraction) + (end * fraction));
        }

        public static float SmoothStep(float start, float end, float x)
        {
            if (x < start)
            {
                return 0;
            }

            if (x >= end)
            {
                return 1;
            }

            x = (x - start) / (end - start);
            return x * x * (3 - 2 * x);
        }

        public static float SmootherStep(float start, float end, float x)
        {
            x = ((x - start) / (end - start)).Clamp(0.0f, 1.0f);
            return x * x * x * (x * (x * 6 - 15) + 10);
        }
    }
}