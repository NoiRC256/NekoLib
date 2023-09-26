using UnityEditor;

namespace NekoLib.Pool
{
    [CustomEditor(typeof(ObjectPoolManager))]
    public class ObjectPoolManagerEditor : Editor
    {
        private ObjectPoolManager _objectPoolManager;
        private bool _showPool;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _objectPoolManager = (ObjectPoolManager)target;
            var prefabPools = _objectPoolManager.PrefabPools;
            var referencePools = _objectPoolManager.ReferencePools;

            EditorGUILayout.LabelField("Prefab pools: " + prefabPools.Count);

            foreach (var poolEntry in prefabPools)
            {
                var pool = poolEntry.Value;
                _showPool = EditorGUILayout.Foldout(_showPool, poolEntry.Value.GetType().ToString(), true);
                if (_showPool)
                {
                    EditorGUILayout.LabelField("Count: " + pool.Count.ToString());
                    EditorGUILayout.LabelField("Capacity: " + pool.Capacity.ToString());
                    EditorGUILayout.LabelField("AutoExpand: " + pool.AutoExpand.ToString());
                    if(pool.AutoExpand)
                    {
                        EditorGUILayout.LabelField("MinCapacity: " + pool.MinCapacity.ToString());
                        EditorGUILayout.LabelField("MaxCapacity: " + pool.MaxCapacity.ToString());
                    }
                    EditorGUILayout.LabelField("Expire Interval: " + pool.ExpireInterval.ToString());
                    EditorGUILayout.LabelField("Tick Interval: " + _objectPoolManager.TickInterval);
                    EditorGUILayout.LabelField("Tick Timer: " + _objectPoolManager.Timer);
                    if (EditorApplication.isPlaying)
                        Repaint();
                }
            }

            EditorGUILayout.LabelField("Reference pools: " + referencePools.Count);
        }
    }
}