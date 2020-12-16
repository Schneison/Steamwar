using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Steamwar.Objects
{
    [RequireComponent(typeof(Image), typeof(RectTransform))]
    public class HealthIndicator : MonoBehaviour
    {
        private ObjectBehaviour objBehaviour;
        public Color activeColor;
        public Color inactiveColor;
        public GameObject container;
        private int maxHealth;
        private int healt;
        private List<Image> healthBars = new List<Image>();
        void Start()
        {
            objBehaviour = GetComponentInParent<ObjectBehaviour>();
            objBehaviour.Data.Type
            foreach(GameObject healBar in container.transform)
            {
                healthBars.Add(healBar.GetComponent<Image>());
            }
            //image = GetComponent<Image>();
            //rectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
