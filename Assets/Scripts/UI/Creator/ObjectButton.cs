using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Steamwar.Units;
using Steamwar.Objects;

namespace Steamwar.UI
{
    public class ObjectButton : MonoBehaviour
    {
        public Image icon;
        private ObjectType type;

        public ObjectType Type
        {
            get
            {
                return type;
            }

            set
            {
                icon.sprite = value.spriteBlue;
                type = value;
            }
        }

        void Start()
        {

        }

        void Update()
        {

        }

        void OnMouseEnter()
        {

        }

        void OnMouseExit()
        {
        }
    }
}
