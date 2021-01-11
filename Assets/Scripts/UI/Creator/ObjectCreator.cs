using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamwar.Utils;
using Steamwar.Units;
using Steamwar.Buildings;
using Steamwar.Objects;

namespace Steamwar.UI {
    public class ObjectCreator : MonoBehaviour
    {
        public GameObject container;
        public GameObject buttonPrefab;
        public CreatorType type;
        internal ObjectButton selectedButton;

        void Start()
        {
            for(int i = 0; i < 10; i++)
            {
                GameObject buttonObj = Instantiate(buttonPrefab, container.transform);
                ObjectButton button = buttonObj.GetComponent<ObjectButton>();
                buttonObj.GetComponent<Button>().onClick.AddListener(() => OnButtonSelection(button));
                buttonObj.transform.name = "CreationButton(" + i + ")";
                buttonObj.SetActive(false);
            }
            //Init Buttons
            if(type != CreatorType.NONE)
            {
                SetType(type);
            }
            /*foreach (UnitType type in ScriptableObjectUtility.GetAllInstances<UnitType>())
            {
                GameObject buttonObj = Instantiate(buttonPrefab, container.transform);
                ObjectButton button = buttonObj.GetComponent<ObjectButton>();
                buttonObj.GetComponent<Button>().onClick.AddListener(() => OnButtonSelection(button));
                buttonObj.transform.name = "CreationButton(" + type.id + ")";
                button.Type = type;
            }*/
        }

        public void SetType(CreatorType type)
        {
            this.type = type;
            int i = 0;
            if (type == CreatorType.UNITS || type == CreatorType.BUILDINGS)
            {
                foreach (ObjectType objectType in SessionManager.registry.GetTypes(type == CreatorType.UNITS ? ObjectKind.UNIT : ObjectKind.BUILDING))
                {
                    if(objectType.name == "missing")
                    {
                        continue;
                    }
                    Transform child = container.transform.Find("CreationButton(" + i + ")");
                    if (child == null)
                    {
                        break;
                    }
                    GameObject buttonObj = child.gameObject;
                    ObjectButton button = buttonObj.GetComponent<ObjectButton>();
                    button.Type = objectType;
                    buttonObj.SetActive(true);
                    i++;
                }
            }
            for(;i < 20;i++)
            {
                Transform child = container.transform.Find("CreationButton(" + i + ")");
                if (child == null)
                {
                    break;
                }
                GameObject buttonObj = child.gameObject;
                buttonObj.SetActive(false);
            }
        }

        public void ClearType()
        {
            this.type = CreatorType.NONE;
            for (int i = 0;i < 20; i++)
            {
                Transform child = container.transform.Find("CreationButton(" + i + ")");
                if(child == null)
                {
                    break;
                }
                GameObject buttonObj = child.gameObject;
                buttonObj.SetActive(false);
            }
        }

        void Update()
        {
        
        }

        public void OnButtonSelection(ObjectButton button)
        {
            selectedButton = button;
            ConstructionManager.Instance.SetSelectedType(button.Type);
        }

    }
}
