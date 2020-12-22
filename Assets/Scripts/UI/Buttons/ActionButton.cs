using UnityEngine;
using System.Collections;
using Steamwar.Objects;
using UnityEngine.UI;

namespace Steamwar.UI
{
    /// <summary>
    /// Button that is useed to activate and deactivate the actions like move or attack.
    /// </summary>
    public class ActionButton : MonoBehaviour
    {
        /// <summary>
        /// Action type of this button
        /// </summary>
        public ActionType type;
        /// <summary>
        /// Controller obj which contains the rendering and the event listener.
        /// </summary>
        public GameObject controller;

        /// <summary>
        /// Activates the controller object and with that activates the rendering and the click event listener.
        /// 
        /// This object gets not deactivated so the order of the buttons and the spacing between them keeps the same.
        /// </summary>
        public void Activate()
        {
            controller.SetActive(true);
        }

        /// <summary>
        /// Deactivates the controller object and with that deactivates the rendering and the click event listener.
        /// </summary>
        public void Deactivate()
        {
            controller.SetActive(false);
        }

        /// <summary>
        /// Called by the controller object if the button gets clicked.
        /// </summary>
        public void OnClick()
        {
            ActionManager.ActivateType(type);
        }
    }
}
