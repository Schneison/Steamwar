using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Steamwar.UI;
using Steamwar.Utils;

namespace Steamwar.UI.Menus
{
    public class MainMenu : Singleton<MainMenu>
    {
        public GameObject mainButtons;
        public LoadMenu loadMenu;
        public Button loadButton;
        public SectorMenu sectorMenu;

        protected override void OnInit()
        {
            if (SaveGame.GetGames().Length == 0)
            {
                loadButton.enabled = false;
            }
        }

        public void Update()
        {

        }

        public void DoAction(int id)
        {
            ButtonType type = (ButtonType)id;
            switch (type)
            {
                case ButtonType.CONTINUE:
                    GameManager.Instance.StartLoading(1);
                    break;
                case ButtonType.NEW_GAME:
                    break;
                case ButtonType.LOAD:
                    mainButtons.gameObject.SetActive(false);
                    loadMenu.gameObject.SetActive(true);
                    break;
                case ButtonType.EDITOR:
                    break;
                case ButtonType.SETTINGS:
                    break;
                case ButtonType.EXIT_DESKTOP:
                    Application.Quit();
                    break;
            }
        }
    }
}