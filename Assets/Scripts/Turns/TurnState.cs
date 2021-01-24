using Steamwar.Factions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Turns
{
    public class TurnStatInstance
    {
        public static TurnStatInstance CreateInstance(Session session)
        {
            List<TurnState> states = new List<TurnState>();
            Faction[] factions = session.factions;
            TurnState lastState = new TransitionState(null);
            states.Add(lastState);
            foreach (Faction faction in factions)
            {
                lastState = faction.IsPlayer ? new PlayerState(lastState, faction) : new NPFState(lastState, faction);
                states.Add(lastState);
            }
            TurnState startState = new TransitionState(lastState);
            return new TurnStatInstance(states.ToArray(), startState, states.First());
        }


        private TurnState[] states;
        private TurnState startState;
        private TurnState endState;
        private TurnState current;

        private TurnStatInstance(TurnState[] states, TurnState startState, TurnState endState)
        {
            this.states = states;
            this.startState = startState;
            this.endState = endState;
            this.current = startState;
        }

        public bool IsPlayer() => current is PlayerState;

        public bool IsTranstion() => current is TransitionState;

        public bool IsFaction() => current is FactionState;

        public bool IsNPF() => current is NPFState;

        public bool TryTransition()
        {
            if (!current.Transition())
            {
                current.OnDeactivated();
                current = current.nextState;
                current.OnActivated();
                return true;
            }
            return false;
        }
    }

    public abstract class TurnState
    {
        public readonly TurnState nextState;

        protected TurnState(TurnState nextState)
        {
            this.nextState = nextState;
        }

        public virtual void OnActivated()
        {
        }

        public virtual void OnDeactivated()
        {
        }

        public virtual void Update()
        {
        }

        public abstract bool Transition();
    }

    public class TransitionState : TurnState
    {
        public TransitionState(TurnState nextState) : base(nextState)
        {
        }

        public override bool Transition() 
        {
            throw new NotImplementedException();
        }
    }

    public class FactionState : TurnState
    {
        public Faction faction;

        public FactionState(TurnState nextState, Faction faction) : base(nextState)
        {
            this.faction = faction;
        }

        public override bool Transition()
        {
            return false;
        }
    }

    public class NPFState : FactionState
    {
        public NPFState(TurnState nextState, Faction faction) : base(nextState, faction)
        {
        }
    }

    public class PlayerState : FactionState
    {
        /// <summary>
        /// If the player pushed the button to allow the game to progress the turn.
        /// </summary>
        public bool transitionAllowed = false;

        public PlayerState(TurnState nextState, Faction faction) : base(nextState, faction)
        {
        }

        public void AllowTransition()
        {
            this.transitionAllowed = true;
        }

        public override bool Transition()
        {
            return transitionAllowed;
        }
    }
}
