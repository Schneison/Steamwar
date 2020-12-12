using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Steamwar.Utils
{
    public class FaceCollections
    {
        public static readonly RenderFace[] VERTICAL = new RenderFace[] { RenderFace.TOP, RenderFace.BOTTOM };
        public static readonly RenderFace[] HORIZONTAL = new RenderFace[] { RenderFace.FRONT, RenderFace.LEFT, RenderFace.BACK, RenderFace.RIGHT };
    }

    public enum RenderFace : int
    {
        TOP, FRONT, LEFT, BACK, RIGHT, BOTTOM
    }
}
