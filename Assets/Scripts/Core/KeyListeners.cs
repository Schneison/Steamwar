using Steamwar.Interaction;
using Steamwar.Objects;
using Steamwar.UI;
using Steamwar.UI.Menus;
using Steamwar.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Steamwar.Core
{

    /// <summary>
    /// Collection of all key listeners
    /// </summary>
    public static class KeyListeners
    {
        private class DescendedIntegerComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return 0 - Comparer<int>.Default.Compare(x, y);
            }
        }

        /// <summary>
        /// Delegate for all key listeners.
        /// <para/>
        /// If it returns true no listeners with a lower priority will get called.
        /// </summary>
        /// <returns></returns>
        public delegate bool KeyListener();

        /// <summary>
        /// All key listeners ordered aftter priority (sorted from lowest to highest)
        /// </summary>
        public static Dictionary<KeyCode, SortedList<int, KeyListener>> listeners = new Dictionary<KeyCode, SortedList<int, KeyListener>>();

        /// <summary>
        /// Checks for pressed keys and calls the listeners if the got pressed
        /// </summary>
        public static void KeyUpdate()
        {
            foreach(var pair in listeners)
            {
                if (Input.GetKeyDown(pair.Key))
                {
                    foreach (KeyListener listener in pair.Value.Values)
                    {
                        if (listener())
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calls all listeners for the given key.
        /// </summary>
        /// <param name="code">The key for that the listeners should be called.</param>
        public static void OnKey(KeyCode code)
        {
            listeners.TryGetValue(code, out var keyListeners);
            if (keyListeners == null)
            {
                return;
            }
            foreach(KeyListener listener in keyListeners.Values)
            {
                if (listener())
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Adds a listener
        /// <para/>
        /// Listeners with a higher priority get called before the other listeners. 
        /// If a listener retuns true no listeners with a lower priority will get called.
        /// </summary>
        /// <param name="code">The key the listener should listen to</param>
        /// <param name="priority">The priority of the listener.</param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public static KeyListener AddListener(KeyCode code,  int priority, KeyListener listener)
        {
            listeners.AddIfAbsent(code, ()=>new SortedList<int, KeyListener>(new DescendedIntegerComparer())).Add(priority, listener);
            return listener;
        }

        public static KeyListener moveAction = AddListener(KeyCode.Alpha1, 0, () => {
            return ActionManager.ActivateType(ActionType.Move);
        });

        public static KeyListener attackAction = AddListener(KeyCode.Alpha2, 0, () => {
            return ActionManager.ActivateType(ActionType.Attack);
        });

        public static KeyListener skipAction = AddListener(KeyCode.Alpha3, 0, () => {
            return ActionManager.ActivateType(ActionType.Skip);
        });

        public static KeyListener repairAction = AddListener(KeyCode.Alpha4, 0, () => {
            return ActionManager.ActivateType(ActionType.Repair);
        });

        public static KeyListener abortCreation = AddListener(KeyCode.Escape, 3, () =>
        {
            if (ConstructionOverlay.Instance.DeselectType())
            {
                return true;
            }
            return CreatorButton.CloseWindow();
        });

        /// <summary>
        /// Deselects the currently active action type
        /// </summary>
        public static KeyListener abortAction = AddListener(KeyCode.Escape, 2, () =>
        {
            return ActionManager.DeselectType();
        });

        /// <summary>
        /// Deselects the currently selected object
        /// </summary>
        public static KeyListener abortSelection = AddListener(KeyCode.Escape, 1, () =>
        {
            return SelectionManager.Instance.Deselect();
        });

        /// <summary>
        /// Opens and closes the esc menu
        /// </summary>
        public static KeyListener openEscMenu = AddListener(KeyCode.Escape, 0, () =>
        {
            EscMenu escMenu = UIElements.Instance.escMenu;
            if (escMenu == null)
            {
                return false;
            }
            GameObject menuObj = escMenu.gameObject;
            menuObj.SetActive(!menuObj.activeSelf);
            return true;
        });
    }

}
