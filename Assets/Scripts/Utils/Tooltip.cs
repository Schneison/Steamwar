using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Steamwar.Utils
{
    [RequireComponent(typeof(RectTransform))]
    public class Tooltip : MonoBehaviour
    {
        public Text title;
        public Image titleBackground;
        public Text desc;
        public Image descBackground;

        private RectTransform rectTransform;
        private RectTransform titleTransform;
        private RectTransform titleBackTransform;


        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            titleTransform = title.GetComponent<RectTransform>();
            titleBackTransform = titleBackground.GetComponent<RectTransform>();
        }

        void Update()
        {
            if (titleTransform.hasChanged)
            {
                titleBackTransform.sizeDelta = rectTransform.sizeDelta = titleTransform.sizeDelta + new Vector2(9, 10);
                titleTransform.hasChanged = false;
            }
            SetPosition(Input.mousePosition);
        }

        public void SetPosition(Vector3 position)
        {
            Vector2 screenStart = Vector2.zero;
            Vector2 screenEnd = new Vector2(Screen.width, Screen.height);
            Vector3 newPos = position + (Vector3.right + Vector3.down) * 25;
            if (newPos.x + rectTransform.sizeDelta.x * rectTransform.localScale.x > screenEnd.x)
            {
                newPos = new Vector3(screenEnd.x - rectTransform.sizeDelta.x * rectTransform.localScale.x, newPos.y, newPos.z);
            }
            if (newPos.y > screenEnd.y)
            {
                newPos = new Vector3(newPos.x, screenEnd.y, newPos.z);
            }
            if (newPos.x < screenStart.x)
            {
                newPos = new Vector3(screenStart.x, newPos.y, newPos.z);
            }
            if (newPos.y - rectTransform.sizeDelta.y * rectTransform.localScale.y < screenStart.y)
            {
                newPos = new Vector3(newPos.x, screenStart.y + rectTransform.sizeDelta.y * rectTransform.localScale.y, newPos.z);
            }

            transform.position = newPos;
        }

        public void SetText(List<string> lines)
        {
            System.Random random = new System.Random();
            string rand = "";
            for (int i = 0; i < random.Next(5, 15); i++)
            {
                rand += (char)('A' + random.Next(0, 26));
            }
            title.text = lines[0] + "\n" + "\n" + rand;
            if (lines.Count > 1)
            {
                desc.text = lines.GetRange(1, lines.Count - 2).ToString();
            }
            gameObject.SetActive(true);
        }
    }
}
