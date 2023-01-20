using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace NekoLib
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _instance;

        public static T Instance {
            get {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }
}