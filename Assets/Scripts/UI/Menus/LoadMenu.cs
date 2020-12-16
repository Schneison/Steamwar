using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Steamwar.UI.Menus
{
    public class LoadMenu : MonoBehaviour
    {
        public Text Count;
        public Image gamePreview;
        public Text gameName;
        public Button nextButton;
        public Button prevButton;

        private SaveGame[] games;
        private int selectedGame = 0;

        public void Start()
        {
            games = SaveGame.GetGames();
            Count.text = "1 / " + games.Length;
            UpdateSection();
        }

        public void Load()
        {
            SaveManager.Load(games[selectedGame].path);
            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                MainMenu.Instance.mainButtons.gameObject.SetActive(true);
                MainMenu.Instance.loadMenu.gameObject.SetActive(false);
            }
            else if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                EscMenu.Instance.mainButtons.gameObject.SetActive(true);
                EscMenu.Instance.loadMenu.gameObject.SetActive(false);
            }
        }

        public void Next()
        {
            selectedGame++;
            UpdateSection();
        }

        public void Previous()
        {
            selectedGame--;
            UpdateSection();
        }

        public void UpdateSection()
        {
            if(selectedGame >= games.Length)
            {
                selectedGame = games.Length - 1;
            }
            prevButton.gameObject.SetActive((selectedGame - 1) >= 0);
            nextButton.gameObject.SetActive((selectedGame + 1) < games.Length);
            SaveGame game = games[selectedGame];
            gameName.text = game.name;
            Count.text = selectedGame + 1 + " / " + games.Length;
        }
    }
}
