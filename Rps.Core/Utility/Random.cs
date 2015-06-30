using System;

namespace Rps.Core.Utility
{
    public static class Random
    {
        public static System.Random New()
        {
            unchecked
            {
                return new System.Random((int)DateTime.Now.Ticks);
            }
        }
    }
}
