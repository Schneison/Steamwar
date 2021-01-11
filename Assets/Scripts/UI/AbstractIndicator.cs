using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Steamwar.UI
{
    public abstract class AbstractIndicator : SteamBehaviour
    {

        public Color[] colors;
        public GameObject container;
        public GameObject barPrefab;
        private int maxValue;
        private int barCount;
        private int value;
        private readonly List<Image> indicatorBars = new List<Image>();

        public abstract int MaxBarCount
        {
            get;
        }

        public void InitIndicator(int value, int maxValue)
        {
            this.maxValue = maxValue;
            barCount = Math.Min(MaxBarCount, maxValue);
            this.value = value;
            for (int i = 0; i < barCount; i++)
            {
                GameObject obj = Instantiate(barPrefab, container.transform);
                obj.name = $"IndicatorBar_{i:D2}";
                Image image = obj.GetComponent<Image>();
                image.color = GetColor(i);
                indicatorBars.Add(image);
            }
        }

        public void ClearIndicator()
        {
            foreach(Image image in indicatorBars)
            {
                Destroy(image.gameObject);
            }
            indicatorBars.Clear();
        }

        public void UpdateIndicator(int value, int maxValue = int.MaxValue)
        {
            bool dirty = this.value != value;
            if(maxValue  != int.MaxValue && maxValue != this.maxValue)
            {
                dirty = true;
                int diff = maxValue - this.maxValue;
                if (diff > 0)
                {
                    for (int i = 0; i < diff; i++)
                    {
                        GameObject obj = Instantiate(barPrefab, container.transform);
                        obj.name = $"IndicatorBar_{indicatorBars.Count:D2}";
                        indicatorBars.Add(obj.GetComponent<Image>());
                    }
                }
                else
                {
                    int oldCount = indicatorBars.Count;
                    for (int i = indicatorBars.Count - 1;i >= oldCount - diff;i--)
                    {
                        Image image = indicatorBars[i];
                        Destroy(image.gameObject);
                        indicatorBars.RemoveAt(i);
                    }
                }
            }
            value = Math.Min(value, this.maxValue);
            if (dirty)
            {
                this.value = value;
                for (int i = 0; i < barCount; i++)
                {
                    indicatorBars[i].color = GetColor(i);
                }
            }
        }

        public Color GetColor(int index)
        {
            if (barCount == MaxBarCount && colors.Length > 2)
            {
                int lastLevelIndex = value % MaxBarCount;
                int level = (value - lastLevelIndex) / MaxBarCount + 1;
                //Is index on this level ?
                if (lastLevelIndex > (index))
                {
                    return colors[level];
                }
                return colors[level - 1];

            }
            return (index + 1) <= value ? colors[1] : colors[0];
        }
    }
}