namespace Core
{

    public static partial class Utils
    {
        public static void Swap<T>(ref T valA, ref T valB)
        {
            (valA, valB) = (valB, valA);
        }
    }
}