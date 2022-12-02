using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekoSystems
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

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
        }

        public override void Init()
        {
            Instance = (T)this;
        }
    }
}