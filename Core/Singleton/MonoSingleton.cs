using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nap
{
    public abstract class MonoSingleton : MonoBehaviour
    {
        public virtual void Init() { }
    }

    public class MonoSingleton<T> : MonoSingleton where T : MonoSingleton<T>
    {
        private static T _instance;

        public static T Instance {
            get => _instance;
            set { if (_instance == null) _instance = value; }
        }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                GameObject.Destroy(this.gameObject);
                return;
            }
        }

        public override void Init()
        {
            Instance = (T)this;
        }
    }
}