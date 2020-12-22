using Steamwar.Interaction;
using Steamwar.Objects;
using Steamwar.Renderer;
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
        public delegate bool KeyListener();

        public static Dictionary<KeyCode, SortedList<int, KeyListener>> listeners = new Dictionary<KeyCode, SortedList<int, KeyListener>>();

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

        public static KeyListener AddListener(KeyCode code,  int priority, KeyListener listener)
        {
            listeners.AddIfAbsent(code, ()=>new SortedList<int, KeyListener>()).Add(priority, listener);
            return listener;
        }

        public static KeyListener abortAction = AddListener(KeyCode.Escape, -2, () =>
        {
            return ActionManager.DeselectType();
        });

        public static KeyListener abortSelection = AddListener(KeyCode.Escape, -1, () =>
        {
            return SelectionManager.Instance.Deselect();
        });

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
