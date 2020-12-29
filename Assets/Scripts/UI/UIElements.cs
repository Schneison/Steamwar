using UnityEngine;
using Steamwar.Utils;
using Steamwar.UI.Menus;
using UnityEngine.UI;

namespace Steamwar.UI
{

    /// <summary>
    /// Contains all variables of the ui elements.
    /// </summary>
    public class UIElements : Singleton<UIElements>
    {
        public EscMenu escMenu;
        public ObjectInspector inspector;
        public ObjectCreator creator;
        public GameObject creatorButtonContainer;
        public GameObject roundCounter;
        public GameObject roundButton;
        public GameObject resourcePanel;
        public ResourcePanel moneyContainer;
        public Text centerMessage;
    }
}
