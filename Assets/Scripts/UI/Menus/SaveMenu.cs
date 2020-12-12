using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Steamwar.UI.Menus
{
    public class SaveMenu : MonoBehaviour
    {
        public InputField input;

        public void Start()
        {
            input.text = SessionManager.session.activeSector.sector.diplayName + " " + SessionManager.session.rounds; 
        }

        public void Save()
        {
            SaveManager.Save(input.text);
            EscMenu.instance.mainButtons.gameObject.SetActive(true);
            EscMenu.instance.saveMenu.gameObject.SetActive(false);
        }
    }
}
