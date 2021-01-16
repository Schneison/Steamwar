using UnityEngine.SceneManagement;

using UnityEngine;
using Steamwar.UI;
using Steamwar.Utils;
using UnityEngine.Events;
using System;

namespace Steamwar
{
    [Serializable]
    public class LifecycleEvent : UnityEvent<LifecycleState>
    {

    }

    public class GameManager : Singleton<GameManager>
    {
        public LifecycleEvent stateChange;

        private static LifecycleState state;

        public static LifecycleState State
        {
            get => state; set
            {
                if (state != value)
                {
                    state = value;
                    if (Instance != null)
                    {
                        Instance.stateChange.Invoke(value);
                    }
                }
            }
        }

        protected override void OnInit()
        {
            base.OnInit();
            DontDestroyOnLoad(gameObject);
        }

        public void StartLoading(int sceneIndex)
        {
            State = LifecycleState.LOADING;
            LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync(sceneIndex));
        }

        public void StartSession()
        {
            State = LifecycleState.SESSION;
        }

        public void StartPlaying()
        {

        }

        public void BackToMenu()
        {

        }

        public static bool Menu()
        {
            return State == LifecycleState.MAIN_MENU;
        }

        public static bool Loading()
        {
            return State == LifecycleState.LOADING;
        }

        public static bool Playing()
        {
            return State == LifecycleState.SESSION;
        }

        public static bool ShuttDown()
        {
            return State == LifecycleState.SHUTTTING_DOWN;
        }

        public void ShuttingDown()
        {
            State = LifecycleState.SHUTTTING_DOWN;
        }
    }

    public enum LifecycleState
    {
        NONE,
        PERSISTENT, 
        MAIN_MENU, 
        LOADING, 
        SESSION,
        SHUTTTING_DOWN
    }
}
