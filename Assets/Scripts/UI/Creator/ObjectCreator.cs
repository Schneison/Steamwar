using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamwar.Utils;
using Steamwar.Units;

namespace Steamwar.UI {
    public class ObjectCreator : MonoBehaviour
    {
        public GameObject container;
        public GameObject buttonPrefab;
        internal ObjectButton selectedButton;

        void Start()
        {
            foreach (UnitType type in ScriptableObjectUtility.GetAllInstances<UnitType>())
            {
                GameObject buttonObj = Instantiate(buttonPrefab, container.transform);
                ObjectButton button = buttonObj.GetComponent<ObjectButton>();
                buttonObj.GetComponent<Button>().onClick.AddListener(()=>OnButtonSelection(button)); 
                buttonObj.transform.name = "CreationButton(" + type.id + ")";
                Image image = buttonObj.GetComponent<Image>();
                button.Type = type;
            }
        }

        void Update()
        {
        
        }

        public void OnButtonSelection(ObjectButton button)
        {
            selectedButton = button;
            UnitController.instance.spawn.selectedUnit = button.Type;
        }

    }
}
