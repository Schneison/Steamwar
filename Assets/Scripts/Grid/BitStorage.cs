using Steamwar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Grid
{
    public class BitStorage
    {
        private const int ITEM_LENGHT = 64;

        private readonly ulong[] internalArray;
        private readonly int bitsPerEntry;
        private readonly ulong maxEntryValue;
        private readonly int lenght;

        public BitStorage(int bitsPerEntry, int lenght)
        {
            this.bitsPerEntry = bitsPerEntry;
            this.maxEntryValue = (ulong)((1 << bitsPerEntry) - 1);
            this.lenght = lenght;
            this.internalArray = new ulong[MathHelper.RoundUp(lenght * this.bitsPerEntry, ITEM_LENGHT) / ITEM_LENGHT];
        }

        public int Lenght => lenght;

        public void Set(int index, int value)
        {
            int entryIndex = index * bitsPerEntry;
            //Item in that the entry starts
            int itemStart = entryIndex / ITEM_LENGHT;
            //Position of the entry in the item 
            int posStart = entryIndex % ITEM_LENGHT;
            // The item where the end of the entry is located
            int itemEnd = ((index + 1) * bitsPerEntry - 1) / ITEM_LENGHT;
            this.internalArray[itemStart] = internalArray[itemStart] & ~(maxEntryValue << posStart) | ((ulong)value & maxEntryValue) << posStart;
            if (itemStart != itemEnd)
            {
                int missingBits = 64 - posStart;
                //The bits in the end item that are not used by the entry, shiftet to right and left to set tthe bits to 0
                int unusedBits = bitsPerEntry - missingBits;
                this.internalArray[itemEnd] = (internalArray[itemEnd]) >> unusedBits << unusedBits | ((ulong)value & maxEntryValue) >> missingBits;
            }
        }

        public int Get(int index)
        {
            int entryIndex = index * bitsPerEntry;
            //Item in that the entry starts
            int itemStart = entryIndex / ITEM_LENGHT;
            //Position of the entry in the item 
            int posStart = entryIndex % ITEM_LENGHT;
            // The item where the end of the entry is located
            int itemEnd = ((index + 1) * bitsPerEntry - 1) / ITEM_LENGHT;
            if(itemStart == itemEnd)
            {
                return (int)(internalArray[itemStart] >> posStart & maxEntryValue);
            }
            else
            {
                int missingBits = 64 - posStart;
                return (int)(((internalArray[itemStart]) >> posStart | (internalArray[itemEnd]) << missingBits) & maxEntryValue);
            }
        }
    }
}
