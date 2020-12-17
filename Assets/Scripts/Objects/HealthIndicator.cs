using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Steamwar.Objects
{
    public class HealthIndicator : MonoBehaviour
    {
        public const uint MAX_BARS = 8;

        private ObjectBehaviour objBehaviour;
        public Color activeColor;
        public Color inactiveColor;
        public GameObject container;
        public GameObject barPrefab;
        private uint maxHealth;
        private uint health;
        private readonly List<Image> healthBars = new List<Image>();

        void Start()
        {
            objBehaviour = GetComponentInParent<ObjectBehaviour>();
            maxHealth = objBehaviour.Data.Type.Health;
            health  = objBehaviour.Data.Health;
            for(int i = 0;i <maxHealth;i++)
            {
                GameObject obj = Instantiate(barPrefab, container.transform);
                obj.name = $"HealthBar_{i:D2}";
                Image image = obj.GetComponent<Image>();
                image.color = GetColor(i);
                healthBars.Add(image);
            }
        }

        void FixedUpdate()
        {
            if(objBehaviour.Data.Health != health)
            {
                health = objBehaviour.Data.Health;
                for (int i = 0; i < maxHealth; i++)
                {
                    healthBars[i].color = GetColor(i);

                }
            }
        }

        public Color GetColor(int index)
        {
            return (index + 1) <= health ? activeColor : inactiveColor;
        }
    }
}
