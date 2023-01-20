using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekoLib
{
    public abstract class MonoSingleton : MonoBehaviour
    {
        public virtual void Init() { }
    }

    /// <summary>
    /// <see cref="MonoBehaviour"/> singleton with no lazy loading. 
    /// Duplicate instances in the scene will be destroyed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoSingleton where T : MonoSingleton<T>
    {
        private static T _instance;

        public static T Instance {
            get => _instance;
            set { if (_instance == null) _instance = value; }
        }

        protected virtual void Awake()
        {
            // If there is already a different instance, destroy self.
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