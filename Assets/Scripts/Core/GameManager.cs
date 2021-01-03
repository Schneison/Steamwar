using UnityEngine.SceneManagement;

using UnityEngine;
using Steamwar.UI;
using Steamwar.Utils;

namespace Steamwar
{
    public class GameManager : Singleton<GameManager>
    {
        internal static LifcycleState state;

        public void StartLoading(int sceneIndex)
        {
            state = LifcycleState.LOADING;
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
            return state == LifcycleState.MAIN_MENU;
        }

        public static bool Loading()
        {
            return state == LifcycleState.LOADING;
        }

        public static bool Playing()
        {
            return state == LifcycleState.SESSION;
        }

        public static bool ShuttDown()
        {
            return state == LifcycleState.SHUTTTING_DOWN;
        }

        public void ShuttingDown()
        {
            state = LifcycleState.SHUTTTING_DOWN;
        }
    }

    public enum LifcycleState
    {
        NONE,
        PERSISTENT, 
        MAIN_MENU, 
        LOADING, 
        SESSION,
        SHUTTTING_DOWN
    }
}
