using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Steamwar.Grid
{
    public class BoardStorage
    {
        public const int INT_BITS = 32;
        public const long LONG_BITS = INT_BITS * 2;

        private readonly BitArray array;
        private readonly int bitsPerEntry;

        public BoardStorage(int lenght, int bitsPerEntry)
        {
            this.array = new BitArray(((lenght << BoardManager.SHIFT_BITS) | lenght) * bitsPerEntry);
            this.bitsPerEntry = bitsPerEntry;
        }

        public BoardStorage(int bitsPerEntry, long[] longArray)
        {
            this.bitsPerEntry = bitsPerEntry;
            int[] intArray = new int[longArray.Length * 2];
            int index = 0;
            for (int i = 0; i < longArray.Length; i++)
            {
                long data = longArray[i];
                intArray[index] = (int)(data >> INT_BITS & uint.MaxValue);
                intArray[index + 1] = (int)(data & uint.MaxValue);
                index += 2;
            }
            array = new BitArray(intArray);

        }

        public int this[int x, int y]
        {
            get
            {
                return this[x << BoardManager.SHIFT_BITS | y];
            }
            set
            {
                this[x << BoardManager.SHIFT_BITS | y] = value;
            }
        }

        public int this[int index]
        {
            get
            {
                int arrayIndex = index * bitsPerEntry;
                int id = 0;
                for (int bitIndex = 0; bitIndex < bitsPerEntry; bitIndex++)
                {
                    id |= (array.Get(arrayIndex + bitIndex) ? 1 : 0) << bitIndex;
                }
                return id;
            }
            set
            {
                int arrayIndex = index * bitsPerEntry;
                for (int bitIndex = 0; bitIndex < bitsPerEntry; bitIndex++)
                {
                    array.Set(arrayIndex + bitIndex, (value & (1 << bitIndex)) > 0);
                }
            }
        }

        public long[] ToArray()
        {
            int count = array.Count;
            long[] longArray = new long[count / LONG_BITS];
            int[] intArray = new int[count / INT_BITS];
            array.CopyTo(intArray, 0);
            int index = 0;
            for (int i = 0; i < longArray.Length; i++)
            {
                longArray[i] = ((long)intArray[index]) << INT_BITS | (long)intArray[index + 1];
                index += 2;
            }
            return longArray;
        }
    }
}
