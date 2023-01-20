#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.GridBrushBase;

namespace Nap
{
    [CustomEditor(typeof(ObjectPoolManager))]
    public class ObjectPoolManagerEditor : Editor
    {
        private bool _showPool;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ObjectPoolManager objectPoolManager = (ObjectPoolManager)target;
            var referencePools = objectPoolManager.ReferencePools;
            var prefabPools = objectPoolManager.PrefabPools;

            foreach (var poolEntry in prefabPools)
            {
                var pool = poolEntry.Value;
                _showPool = EditorGUILayout.Foldout(_showPool, poolEntry.Value.GetType().ToString(), true);
                if (_showPool)
                {
                    EditorGUILayout.LabelField("Count: " + pool.Count.ToString());
                    EditorGUILayout.LabelField("Expire Interval: " + pool.ExpireInterval.ToString());
                    EditorGUILayout.LabelField("Tick Interval: " + objectPoolManager.TickInterval);
                    EditorGUILayout.LabelField("Tick Timer: " + objectPoolManager.Timer);
                    if (EditorApplication.isPlaying)
                        Repaint();
                }
            }
        }
    }
}
#endif