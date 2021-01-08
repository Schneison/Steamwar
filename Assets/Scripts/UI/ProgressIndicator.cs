using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Timers;
using Steamwar.Buildings;

namespace Steamwar.Objects
{
    public class ProgressIndicator : MonoBehaviour
    {
        public const int MAX_BARS = 3;

        private ObjectContainer objBehaviour;
        private BuildingFactory factory;
        public Color[] colors;
        public GameObject container;
        public GameObject barPrefab;
        private int progress;
        private readonly List<Image> bars = new List<Image>();

        public ObjectContainer ObjBehaviour
        {
            get
            {
                if (objBehaviour == null)
                {
                    objBehaviour = GetComponentInParent<ObjectContainer>();
                }
                return objBehaviour;
            }
        }

        public BuildingFactory Factory
        {
            get
            {
                if(factory == null)
                {
                    factory = ObjBehaviour.GetComponentInChildren<BuildingFactory>();
                }
                return factory;
            }
        }

        private int GetProgressCount()
        {
            return (int)Math.Round((decimal)MAX_BARS * Factory.progress / Factory.progressMax);
        }

        void Start()
        {
            if(Factory == null)
            {
                return;
            }
            progress = GetProgressCount();
            for (int i = 0;i < MAX_BARS; i++)
            {
                GameObject obj = Instantiate(barPrefab, container.transform);
                obj.name = $"ProgressBar_{i:D2}";
                Image image = obj.GetComponent<Image>();
                image.color = GetColor(i);
                bars.Add(image);
            }
        }

        public bool IsVisible()
        {
            return Factory != null && Factory.Product != null;
        }

        void FixedUpdate()
        {
            int newPorgress = GetProgressCount();
            if(newPorgress != progress)
            {
                progress = newPorgress;
                for (int i = 0; i < MAX_BARS; i++)
                {
                    bars[i].color = GetColor(i);
                }
            }
        }



        public Color GetColor(int index)
        {
            return (index + 1) <= progress ? colors[1] : colors[0];
        }
    }
}
