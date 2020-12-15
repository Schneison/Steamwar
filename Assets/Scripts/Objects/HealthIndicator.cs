using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Steamwar.Objects
{
    [RequireComponent(typeof(Image), typeof(RectTransform))]
    public class HealthIndicator : MonoBehaviour
    {
        private ObjectBehaviour objBehaviour;
        private Image image;
        private RectTransform rectTransform;
        void Awake()
        {
            objBehaviour = GetComponentInParent<ObjectBehaviour>();
            image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
