using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Steamwar.Utils
{
    public static class MathHelper
    {
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }

            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            return Mathf.Clamp(value, min, max);
        }
    }
}
