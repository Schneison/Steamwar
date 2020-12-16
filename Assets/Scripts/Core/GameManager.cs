using UnityEngine.SceneManagement;

using UnityEngine;
using Steamwar.UI;
using Steamwar.Utils;

namespace Steamwar
{
    public class GameManager : Singleton<GameManager>
    {
        internal GameState state;

        public void StartLoading(int sceneIndex)
        {
            state = GameState.LOADING;
            LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync(sceneIndex));
        }

        public void StartPlaying()
        {

        }

        public void BackToMenu()
        {

        }

        public static bool Menu()
        {
            return Instance.state == GameState.MAIN_MENU;
        }

        public static bool Loading()
        {
            return Instance.state == GameState.LOADING;
        }

        public static bool Playing()
        {
            return Instance.state == GameState.SESSION;
        }
    }

    public enum GameState
    {
        PERSISTENT, MAIN_MENU, LOADING, SESSION
    }
}
