using System;

namespace RunkeeperData
{
    public static class Extensions
    {
        public static double ToKilometers(this double distance)
        {
            return Math.Round(distance / 1000, 2);
        }
    }
}
