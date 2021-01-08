using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Timers;

namespace Steamwar.Objects
{
    public class HealthIndicator : MonoBehaviour
    {
        public const uint MAX_BARS = 7;

        private ObjectContainer objBehaviour;
        public Color[] colors;
        public GameObject container;
        public GameObject barPrefab;
        private uint maxHealth;
        private uint barCount;
        private uint health;
        private readonly List<Image> healthBars = new List<Image>();

        void Start()
        {
            objBehaviour = GetComponentInParent<ObjectContainer>();
            if(objBehaviour == null)
            {
                Debug.Log("Failed to find Object for Health Indicator.");
                return;
            }
            maxHealth = objBehaviour.Data.Type.Health;
            barCount = Math.Min(MAX_BARS, maxHealth);
            health  = objBehaviour.Data.Health;
            for(uint i = 0;i < barCount; i++)
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
            if (objBehaviour == null)
            {
                return;
            }
            if (objBehaviour.Data.Health != health)
            {
                health = objBehaviour.Data.Health;
                for (uint i = 0; i < barCount; i++)
                {
                    healthBars[(int)i].color = GetColor(i);
                }
            }
        }



        public Color GetColor(uint index)
        {
            if (barCount == MAX_BARS)
            {
                uint lastLevelIndex = health % MAX_BARS;
                uint level = (health - lastLevelIndex) / MAX_BARS + 1;
                //Is index on this level ?
                if (lastLevelIndex > (index))
                {
                    return colors[level];
                }
                return colors[level - 1];

            }
            return (index + 1) <= health ? colors[1] : colors[0];
        }
    }
}
