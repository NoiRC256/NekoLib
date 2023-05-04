using System;
using System.Collections.Generic;
using UnityEngine;

namespace NekoLib.ScriptableEvents
{
    public abstract class ScriptableEventBase : ScriptableObject, IScriptableEvent
    {
        private List<Action> _listeners = new List<Action>();

        public void Invoke()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].Invoke();
            }
        }

        public void Register(Action listener)
        {
            _listeners.Add(listener);
        }

        public void Unregister(Action listener)
        {
            _listeners.Remove(listener);
        }

        public void Clear() => _listeners = null;
    }

    public abstract class ScriptableEventBase<T> : ScriptableObject, IScriptableEvent
    {
        private List<Action<T>> _listeners = new List<Action<T>>();

        public void Invoke(T param)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].Invoke(param);
            }
        }

        public void Register(Action<T> listener)
        {
            _listeners.Add(listener);
        }

        public void Unregister(Action<T> listener)
        {
            _listeners.Remove(listener);
        }

        public void Clear() => _listeners = null;
    }

    public abstract class ScriptableEventBase<T1, T2> : ScriptableObject, IScriptableEvent
    {
        private List<Action<T1, T2>> _listeners = new List<Action<T1, T2>>();

        public void Invoke(T1 param1, T2 param2)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].Invoke(param1, param2);
            }
        }

        public void Register(Action<T1, T2> listener)
        {
            _listeners.Add(listener);
        }

        public void Unregister(Action<T1, T2> listener)
        {
            _listeners.Remove(listener);
        }

        public void Clear() => _listeners = null;
    }

    public interface IScriptableEvent
    {
        void Clear();
    }
}