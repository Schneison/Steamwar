using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Steamwar.Objects;

namespace Steamwar.UI
{
    public class CreatorButton : MonoBehaviour
    {
        public static CreatorButton selectedButton;

        public CreatorType type;
        public GameObject activeBackground;
        public ObjectCreator objectCreator;
        private RectTransform _creatorTransform;
        internal bool selected;

        void Start()
        {
            _creatorTransform = objectCreator.GetComponent<RectTransform>();
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
                    Close();
                    return;
                }
                selectedButton.activeBackground.SetActive(false);
                selectedButton.selected = false;
            }
            selectedButton = this;
            selected = true;
            ActionManager.DeselectType();
            StartCoroutine(FadeIn());
            ConstructionOverlay.Instance.ActivateLayer();
        }

        private bool Close()
        {
            if(selectedButton != this)
            {
                return false;
            }
            selectedButton = null;
            selected = false;
            StartCoroutine(FadeOut());
            ConstructionOverlay.Instance.DeactivateLayer();
            return true;
        }

        public static bool CloseWindow()
        {
            return selectedButton != null && selectedButton.Close();
        }

        public IEnumerator FadeIn()
        {
            objectCreator.gameObject.SetActive(true);
            objectCreator.SetType(type);
            activeBackground.SetActive(true);
            Rect screenRect = new Rect(0f, 0f, Screen.width, Screen.height);
            Vector3[] objectCorners = new Vector3[4];
            _creatorTransform.GetWorldCorners(objectCorners);
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
            _creatorTransform.GetWorldCorners(objectCorners);
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
            objectCreator.ClearType();
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
