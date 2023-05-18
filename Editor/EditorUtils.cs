#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace NekoLib
{
    public static class EditorUtils
    {
        /// <summary>
        /// <script src="https://gist.github.com/vildninja/fefddf7390646a113ba7ee2a5da0525e.js"></script>
        /// </summary>
        [MenuItem("NekoLib Editor/Remove All Missing Scripts In Selected")]
        private static void RemoveAllMissingScripts()
        {
            // EditorUtility.CollectDeepHierarchy does not include inactive children
            var deeperSelection = Selection.gameObjects.SelectMany(go => go.GetComponentsInChildren<Transform>(true))
                .Select(t => t.gameObject);
            var prefabs = new HashSet<Object>();
            int compCount = 0;
            int goCount = 0;
            foreach (var go in deeperSelection)
            {
                int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
                if (count > 0)
                {
                    if (PrefabUtility.IsPartOfAnyPrefab(go))
                    {
                        RecursivePrefabSource(go, prefabs, ref compCount, ref goCount);
                        count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
                        // if count == 0 the missing scripts has been removed from prefabs
                        if (count == 0)
                            continue;
                        // if not the missing scripts must be prefab overrides on this instance
                    }

                    Undo.RegisterCompleteObjectUndo(go, "Remove missing scripts");
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                    compCount += count;
                    goCount++;
                }
            }

            Debug.Log($"Found and removed {compCount} missing scripts from {goCount} GameObjects");
        }

        // Prefabs can both be nested or variants, so best way to clean all is to go through them all
        // rather than jumping straight to the original prefab source.
        private static void RecursivePrefabSource(GameObject instance, HashSet<Object> prefabs, ref int compCount,
            ref int goCount)
        {
            var source = PrefabUtility.GetCorrespondingObjectFromSource(instance);
            // Only visit if source is valid, and hasn't been visited before
            if (source == null || !prefabs.Add(source))
                return;

            // go deep before removing, to differantiate local overrides from missing in source
            RecursivePrefabSource(source, prefabs, ref compCount, ref goCount);

            int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(source);
            if (count > 0)
            {
                Undo.RegisterCompleteObjectUndo(source, "Remove missing scripts");
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(source);
                compCount += count;
                goCount++;
            }
        }
    }
}
#endif