using Steamwar.Factions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace Steamwar.Turns
{
    public class TurnStatInstance
    {

        public static TurnStatInstance CreateInstance(Session session)
        {
            List<TurnState> states = new List<TurnState>();
            Faction[] factions = session.factions;
            TurnState firstState = new TurnState();
            TurnState betweenState = new StageState(TurnStage.BETWEEN);
            TurnState lastState = betweenState;
            states.Add(lastState);
            foreach (int factionIndex in session.factionOrder.Reverse())
            {
                Faction faction = factions[factionIndex];
                FactionState nextState;
                if (faction.index == session.playerIndex) // faction.IsPlayer not valid for testing
                {
                    nextState = new PlayerState(faction);
                }
                else
                {
                    nextState = new NPFState(faction);
                }
                nextState.TransitionTurn(lastState, (instance) => nextState.TransitionAllowed());
                states.Add(nextState);
                lastState = nextState;
            }
            TurnState startState = new StageState(TurnStage.START);
            states.Add(startState);
            startState.TransitionTurn(lastState, (instance) => true);
            firstState.TransitionTurn(startState, (instance) => true);
            //State transitioned to if the game has reached its end.
            TurnState endState = new StageState(TurnStage.END);
            states.Add(endState);
            betweenState.TransitionTurn(startState, (instance) => instance.turnAmount < instance.turnMax);
            betweenState.TransitionTurn(endState, (instance) => instance.turnAmount >= instance.turnMax);
            return new TurnStatInstance(states.ToArray(), firstState, endState, session);
        }

        private readonly TurnState[] states;
        private readonly TurnState startState;
        private readonly TurnState endState;
        private TurnState current;
        public int turnAmount;
        public int turnMax;

        private TurnStatInstance(TurnState[] states, TurnState startState, TurnState endState, Session session)
        {
            this.states = states;
            this.startState = startState;
            this.endState = endState;
            this.current = startState;
            UpdateSession(session);
        }

        public void UpdateSession(Session session)
        {
            this.turnAmount =  session.turnCount;
            this.turnMax = session.turnMax;
        }

        public void OnSaveSession(Session session)
        {
            session.turnCount = turnAmount;
        }

        public bool IsPlayer() => current is PlayerState;

        public void AllowPlayerTransition()
        {
            Assert.IsTrue(IsPlayer());
            ((PlayerState)current).AllowTransition();
        }

        public bool IsStart() => current.IsStart();

        public bool IsTranstion() => current.IsBetween();

        public bool IsEnd() => current.IsEnd();

        public bool IsFaction() => current is FactionState;

        public bool IsNPF() => current is NPFState;

        public void Update()
        {
            current.Update(this);
        }

        public void ChangeState(TurnState newState)
        {
            current.OnDeactivated(this);
            current = newState;
            current.OnActivated(this);
        }
    }

    public class TurnState
    {
        public readonly List<Transition> transitions;
        public TurnState()
        {
            this.transitions = new List<Transition>();
        }

        public void Initialize()
        {

        }

        public virtual void OnActivated(TurnStatInstance instance)
        {
        }

        public virtual void OnDeactivated(TurnStatInstance instance)
        {
        }

        public virtual void Update(TurnStatInstance instance)
        {
            foreach (Transition transition in transitions)
            {
                if (transition.Condition(instance))
                {
                    instance.ChangeState(transition.target);
                    break;
                }
            }
        }

        public void TransitionTurn(TurnState state, Func<TurnStatInstance, bool> callback)
        {
            transitions.Add(new TurnTransaction(this, state, callback));
        }

        public virtual bool IsEnd()
        {
            return false;
        }

        public virtual bool IsStart()
        {
            return false;
        }

        public virtual bool IsBetween()
        {
            return false;
        }
    }

    public class StageState : TurnState
    {
        public readonly TurnStage stage;

        public StageState(TurnStage stage)
        {
            this.stage = stage;
        }

        public override void OnActivated(TurnStatInstance instance)
        {
            if (!IsStart())
            {
                return;
            }
            instance.turnAmount += 1;
            TurnSystem.Instance.turnStart.Invoke(instance);
        }

        public override void OnDeactivated(TurnStatInstance instance)
        {
            if (!IsBetween())
            {
                return;
            }
            TurnSystem.Instance.turnEnd.Invoke(instance);
        }

        public override bool IsEnd()
        {
            return stage == TurnStage.END;
        }

        public override bool IsStart()
        {
            return stage == TurnStage.START;
        }

        public override bool IsBetween()
        {
            return stage == TurnStage.BETWEEN;
        }
    }

    public abstract class FactionState : TurnState
    {
        public readonly Faction faction;

        public FactionState(Faction faction) : base()
        {
            this.faction = faction;
        }

        public override void OnActivated(TurnStatInstance instance)
        {
            FactionManager.Activate(faction.index);
        }

        public abstract bool TransitionAllowed();
    }

    public class NPFState : FactionState
    {
        public NPFState(Faction faction) : base(faction)
        {
        }

        public override bool TransitionAllowed()
        {
            return true;
        }
    }

    public class PlayerState : FactionState
    {
        /// <summary>
        /// If the player pushed the button to allow the game to progress the turn.
        /// </summary>
        public bool transitionAllowed = false;

        public PlayerState(Faction faction) : base(faction)
        {
        }

        public void AllowTransition()
        {
            this.transitionAllowed = true;
        }

        public override void OnDeactivated(TurnStatInstance instance)
        {
            base.OnDeactivated(instance);
            transitionAllowed = false;
        }

        public override bool TransitionAllowed()
        {
            return transitionAllowed;
        }
    }

    public abstract class Transition
    {
        public readonly TurnState source;
        public readonly TurnState target;

        protected Transition(TurnState source, TurnState target)
        {
            this.source = source;
            this.target = target;
        }

        public abstract bool Condition(TurnStatInstance instance);
    }

    public class TurnTransaction : Transition
    {
        private readonly Func<TurnStatInstance, bool> conditionCallback;

        public TurnTransaction(TurnState source, TurnState target, Func<TurnStatInstance, bool> conditionCallback) : base(source, target)
        {
            this.conditionCallback = conditionCallback;
        }

        public override bool Condition(TurnStatInstance instance)
        {
            return conditionCallback(instance);
        }
    }
}
