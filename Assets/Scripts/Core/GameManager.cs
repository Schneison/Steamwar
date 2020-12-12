using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

using UnityEngine;
using Steamwar.UI;

namespace Steamwar
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        internal GameState state;

        public void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }else if(instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

        public void StartLoading(int sceneIndex)
        {
            state = GameState.LOADING;
            LoadingScreen.instance.Show(SceneManager.LoadSceneAsync(sceneIndex));
        }

        public void StartPlaying()
        {

        }

        public void BackToMenu()
        {

        }

        public static bool Menu()
        {
            return instance.state == GameState.MAIN_MENU;
        }

        public static bool Loading()
        {
            return instance.state == GameState.LOADING;
        }

        public static bool Playing()
        {
            return instance.state == GameState.SESSION;
        }
    }

    public enum GameState
    {
        PERSISTENT, MAIN_MENU, LOADING, SESSION
    }
}
