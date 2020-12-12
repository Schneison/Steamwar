using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamwar.Utils;

namespace Steamwar.Renderer
{
    public struct RenderInfo
    {
        public bool[] showFace;

        public RenderInfo(params RenderFace[] faces)
        {
            this.showFace = new bool[] { false, false, false, false, false, false };
            foreach(RenderFace face in faces)
            {
                this.showFace[(int)face] = true;
            }
        }

        public RenderInfo(bool[] showFace)
        {
            this.showFace = showFace;
        }

        public bool IsFaceVisible(RenderFace face)
        {
            return showFace[(int)face];
        }
    }
}
