using UnityEngine.SceneManagement;

using UnityEngine;
using Steamwar.UI;
using Steamwar.Utils;

namespace Steamwar
{
    public class GameManager : Singleton<GameManager>
    {
        internal static GameState state;

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
            return state == GameState.MAIN_MENU;
        }

        public static bool Loading()
        {
            return state == GameState.LOADING;
        }

        public static bool Playing()
        {
            return state == GameState.SESSION;
        }

        public static bool ShuttDown()
        {
            return state == GameState.SHUTTTING_DOWN;
        }

        public void ShuttingDown()
        {
            state = GameState.SHUTTTING_DOWN;
        }
    }

    public enum GameState
    {
        PERSISTENT, MAIN_MENU, LOADING, SESSION, SHUTTTING_DOWN
    }
}
