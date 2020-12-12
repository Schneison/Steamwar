using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Steamwar.Sectors;

namespace Steamwar.UI.Menus
{
    public class EscMenu : MonoBehaviour
    {
        public static EscMenu instance;
        public GameObject mainButtons;
        public SaveMenu saveMenu;
        public LoadMenu loadMenu;
        public Button loadButton;

        public void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }else if(instance != this)
            {
                Destroy(this);
                return;
            }
        }

        public void Start()
        {
            if(SaveGame.GetGames().Length == 0)
            {
                loadButton.enabled = false;
            }
        }

        public void DoAction(int id)
        {
            ButtonType type = (ButtonType)id;
            switch (type) {
                case ButtonType.CONTINUE:
                    gameObject.SetActive(false);
                    break;
                case ButtonType.SAVE:
                    mainButtons.gameObject.SetActive(false);
                    saveMenu.gameObject.SetActive(true);
                    break;
                case ButtonType.LOAD:
                    mainButtons.gameObject.SetActive(false);
                    loadMenu.gameObject.SetActive(true);
                    break;
                case ButtonType.RESTART:
                    SessionManager.instance.sectors.Restart();
                    break;
                case ButtonType.EXIT_MAIN:
                    SceneManager.LoadScene(0);
                    break;
                case ButtonType.EXIT_DESKTOP:
                    Application.Quit();
                    break;
            }
        }
    }
}
