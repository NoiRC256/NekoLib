using System.Collections.Generic;

namespace NekoLib.FSM
{
    // Adapted from Iris Fenrir
    /// <summary>
    /// Base class for FSM.
    /// </summary>
    public class FSMBase<T>
    {
        public string CurrentStateName { get; set; }

        private T _owner;
        private Dictionary<string, FSMState<T>> _states;

        private FSMState<T> _defaultState;
        private FSMState<T> _currentState;

        private bool _isInit = false;

        public FSMBase(T owner)
        {
            _owner = owner;
            _states = new Dictionary<string, FSMState<T>>();
        }

        /// <summary>
        /// FSM Update tick.
        /// </summary>
        public void Update()
        {
            Init();

            if(_currentState != null)
            {
                _currentState.OnUpdate(_owner);
                if(_currentState.CheckTransition(_owner, out string stateName))
                {
                    ChangeState(stateName);
                    CurrentStateName = stateName;
                }
            }
            
        }

        /// <summary>
        /// Initialize the FSM.
        /// </summary>
        private void Init()
        {
            if (!_isInit)
            {
                _currentState.OnEnter(_owner);
                _isInit = true;
            }
        }

        /// <summary>
        /// Change the current state to the specified state.
        /// </summary>
        /// <param name="stateName"></param>
        private void ChangeState(string stateName)
        {
            if(_states.TryGetValue(stateName, out FSMState<T> state))
            {
                _currentState.OnExit(_owner);
                state.OnEnter(_owner);
                _currentState = state;
            }
        }

        /// <summary>
        /// Set the default state of the FSM.
        /// </summary>
        /// <param name="stateName"></param>
        public void SetDefault(string stateName)
        {
            if (_states.ContainsKey(stateName))
            {
                _defaultState = _states[stateName];
                _currentState = _defaultState;
                CurrentStateName = stateName;
            }
        }

        /// <summary>
        /// Add a state to the FSM.
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="state"></param>
        public void AddState(string stateName, FSMState<T> state)
        {
            if (string.IsNullOrEmpty(stateName) || state == null) return;
            _states.Add(stateName, state);
        }
    }
}