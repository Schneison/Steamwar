
using UnityEngine;

namespace Steamwar.Objects
{
    public class ObjectType : ScriptableObject
    {
        public string id;
        public string displayName;
        public Sprite spriteBlue;
        public Sprite spriteRed;
        public Sprite sprite;

        public virtual uint Health
        {
            get
            {
                return 0;
            }
        }
    }
}
