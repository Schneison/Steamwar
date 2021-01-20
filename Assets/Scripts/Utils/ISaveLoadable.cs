using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Utils
{
    public interface ISaveLoadable
    {
        void Serialize(BinaryWriter writer);

        void Deserialize(BinaryReader reader);
    }
}
