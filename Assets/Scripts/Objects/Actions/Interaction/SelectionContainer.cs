using Steamwar.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Steamwar.Objects
{
    public class SelectionContainer : IEnumerable<ISelectionListener>
    {
        private readonly Dictionary<ActionType, HashSet<ISelectionListener>> listeners = new Dictionary<ActionType, HashSet<ISelectionListener>> 
        { 
            { ActionType.None, new HashSet<ISelectionListener>() },
            { ActionType.Move, new HashSet<ISelectionListener>() },
            { ActionType.Attack, new HashSet<ISelectionListener>() },
            { ActionType.Skip, new HashSet<ISelectionListener>() },
            { ActionType.Repair, new HashSet<ISelectionListener>() },
            { ActionType.Destroy, new HashSet<ISelectionListener>() }
        };
        private readonly HashSet<ISelectionListener> activeListeners = new HashSet<ISelectionListener>();
        private readonly Func<SelectionData> dataSupplier;

        public SelectionContainer(IEnumerable<ISelectionListener> listenerAll, Func<SelectionData>  dataSupplier)
        {
            this.dataSupplier = dataSupplier;
            foreach (ISelectionListener listener in listenerAll)
            {
                ActionType type = listener.GetActionType();
                listeners[type].Add(listener);
            }
            OnActionUpdate(ActionType.Move);
        }

        public void OnActionUpdate(ActionType activeType)
        {
            HashSet<ISelectionListener> deselectedListeners = new HashSet<ISelectionListener>(activeListeners);

            activeListeners.Clear();
            activeListeners.UnionWith(listeners[activeType]);
            activeListeners.UnionWith(listeners[ActionType.None]);
            HashSet<ISelectionListener> newSelectedListeners = new HashSet<ISelectionListener>(activeListeners);
            newSelectedListeners.ExceptWith(deselectedListeners);
            deselectedListeners.ExceptWith(activeListeners);
            SelectionData data = dataSupplier();
            if (!data.IsEmpty) {
                foreach (ISelectionListener listener in newSelectedListeners)
                {
                    listener.OnSelection(data, SelectionData.EMPTY);
                }
                foreach (ISelectionListener listener in deselectedListeners)
                {
                    listener.OnDeselection(data);
                }
            }
        }

        public IEnumerator<ISelectionListener> GetEnumerator()
        {
            return activeListeners.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return activeListeners.GetEnumerator();
        }
    }
}
