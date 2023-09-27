using UnityEngine;

namespace NekoLab.Singletons
{
    public interface ISingleton
    {
        void Init();
    }

    /// <summary>
    /// <see cref="MonoBehaviour"/> singleton with no lazy loading. 
    /// Duplicate instances in the scene will be destroyed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
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

            Init();
        }

        public void Init()
        {
            Instance = (T)this;
        }
    }
}