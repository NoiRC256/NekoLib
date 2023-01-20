using System;
using System.Collections.Generic;

namespace NekoLib.FSM
{
    // Adapted from Iris Fenrir
    /// <summary>
    /// Base class for FSM states.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FSMState<T>
    {
        private Dictionary<FSMCondition<T>, string> _transitionMaps;

        private Action<T> _enterAction;
        private Action<T> _updateAction;
        private Action<T> _exitAction;

        /// <summary>
        /// Define the action on state enter.
        /// </summary>
        /// <param name="action"></param>
        public void BindEnterAction(Action<T> action)
        {
            _enterAction = action;
        }

        /// <summary>
        /// Define the action on state update.
        /// </summary>
        /// <param name="action"></param>
        public void BindUpdateAction(Action<T> action)
        {
            _updateAction = action;
        }

        /// <summary>
        /// Define the action on state exit.
        /// </summary>
        /// <param name="action"></param>
        public void BindExitAction(Action<T> action)
        {
            _exitAction = action;
        }

        /// <summary>
        /// Add a transition.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="targetStateName"></param>
        public void AddTransition(FSMCondition<T> condition, string targetStateName)
        {
            if (condition == null || string.IsNullOrEmpty(targetStateName)) return;
            if (_transitionMaps == null)
            {
                _transitionMaps = new Dictionary<FSMCondition<T>, string>();
            }
            _transitionMaps.Add(condition, targetStateName);
        }

        /// <summary>
        /// Remove a transition.
        /// </summary>
        /// <param name="condition"></param>
        public void RemoveTransition(FSMCondition<T> condition)
        {
            if (condition == null || _transitionMaps == null) return;
            _transitionMaps.Remove(condition);
        }

        /// <summary>
        /// Checks for state transitions. If found an available transition, returns true, and outputs target state name.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="targetStateName"></param>
        /// <returns></returns>
        public bool CheckTransition(T owner, out string targetStateName)
        {
            if (_transitionMaps == null)
            {
                targetStateName = string.Empty;
                return false;
            }
            foreach (FSMCondition<T> condition in _transitionMaps.Keys)
            {
                if (condition.Condition(owner))
                {
                    targetStateName = _transitionMaps[condition];
                    return true;
                }
            }
            targetStateName = string.Empty;
            return false;
        }

        /// <summary>
        /// Called on state enter.
        /// </summary>
        /// <param name="owner"></param>
        public virtual void OnEnter(T owner)
        {
            if (_enterAction != null) _enterAction(owner);
        }

        /// <summary>
        /// Called on state update.
        /// </summary>
        /// <param name="owner"></param>
        public virtual void OnUpdate(T owner)
        {
            if (_updateAction != null) _updateAction(owner);
        }

        /// <summary>
        /// Called on state exit.
        /// </summary>
        /// <param name="owner"></param>
        public virtual void OnExit(T owner)
        {
            if (_exitAction != null) _exitAction(owner);
        }
    }
}