using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Steamwar.Units;

namespace Steamwar.UI
{
    public class ObjectButton : MonoBehaviour
    {
        public Image icon;
        private UnitType type;

        public UnitType Type
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
