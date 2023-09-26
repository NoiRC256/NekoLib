using UnityEngine;
using UnityEngine.Events;

namespace NekoLib.ScriptableEvents
{
    public abstract class ScriptableEventListenerBase : MonoBehaviour
    {
        public ScriptableEventBase Event;
        public UnityEvent Response;

        private void OnEnable()
        {
            Event.Register(OnInvoke);
        }

        private void OnDisable()
        {
            Event.Unregister(OnInvoke);
        }

        public void OnInvoke()
        {
            Response?.Invoke();
        }
    }

    public abstract class ScriptableEventListenerBase<TEvent, TParam> : MonoBehaviour
        where TEvent : ScriptableEventBase<TParam> 
    {
        public TEvent Event;
        public UnityEvent<TParam> Response;

        private void OnEnable()
        {
            Event.Register(OnInvoke);
        }

        private void OnDisable()
        {
            Event.Unregister(OnInvoke);
        }

        public void OnInvoke(TParam param)
        {
            Response?.Invoke(param);
        }
    }

    public abstract class ScriptableEventListenerBase<TEvent, TParam1, TParam2> : MonoBehaviour 
        where TEvent : ScriptableEventBase<TParam1, TParam2>
    {
        public TEvent Event;
        public UnityEvent<TParam1, TParam2> Response;

        private void OnEnable()
        {
            Event.Register(OnInvoke);
        }

        private void OnDisable()
        {
            Event.Unregister(OnInvoke);
        }

        public void OnInvoke(TParam1 param1, TParam2 param2)
        {
            Response?.Invoke(param1, param2);
        }
    }
}