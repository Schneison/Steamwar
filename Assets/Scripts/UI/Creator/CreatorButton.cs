using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Steamwar.UI
{
    public class CreatorButton : MonoBehaviour
    {
        public static CreatorButton selectedButton;

        public CreatorType type;
        public GameObject activeBackground;
        public ObjectCreator objectCreator;
        internal bool selected;

        void Start()
        {

        }

        void Update()
        {

        }

        public void OnPressed()
        {
            if (selectedButton != null)
            {
                if (selectedButton == this)
                {
                    selectedButton = null;
                    selected = false;
                    StartCoroutine(FadeOut());
                    return;
                }
                selectedButton.activeBackground.SetActive(false);
                selectedButton.selected = false;
            }
            selectedButton = this;
            selected = true;
            StartCoroutine(FadeIn());
        }

        public IEnumerator FadeIn()
        {
            objectCreator.gameObject.SetActive(true);
            objectCreator.GetComponent<ObjectCreator>().SetType(type);
            activeBackground.SetActive(true);
            Rect screenRect = new Rect(0f, 0f, Screen.width, Screen.height);
            Vector3[] objectCorners = new Vector3[4];
            objectCreator.GetComponent<RectTransform>().GetWorldCorners(objectCorners);
            float delta = objectCorners[3].x - screenRect.xMax;
            float change = delta / 16.0F;
            Transform container = transform.parent.parent;
            while (delta > 0)
            {

                container.position -= new Vector3(change, 0F, 0F);
                objectCreator.transform.position -= new Vector3(change, 0F, 0F);
                delta -= change;
                yield return new WaitForSeconds(0.0625F);
            }
        }

        public IEnumerator FadeOut()
        {
            Vector3[] objectCorners = new Vector3[4];
            objectCreator.GetComponent<RectTransform>().GetWorldCorners(objectCorners);
            objectCreator.GetComponent<ObjectCreator>().ClearType();
            float delta = objectCorners[3].x - objectCorners[0].x;
            float change = delta / 16.0F;
            Transform container = transform.parent.parent;
            while (delta > 0)
            {

                container.position += new Vector3(change, 0F, 0F);
                objectCreator.transform.position += new Vector3(change, 0F, 0F);
                delta -= change;
                yield return new WaitForSeconds(0.0625F);
            }
            activeBackground.SetActive(false);
            objectCreator.gameObject.SetActive(false);
        }

    }

    [Serializable]
    public enum CreatorType
    {
        UNITS, BUILDINGS, MISC, NONE
    }
}
