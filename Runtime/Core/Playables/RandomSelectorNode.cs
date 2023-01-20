using UnityEngine;
using UnityEngine.Playables;

namespace NekoLib.Playables
{
    /// <summary>
    /// Custom <see cref="AnimBehaviour"/> that selects a random child.
    /// </summary>
    public class RandomSelectorNode : AnimSelectorNode
    {
        public RandomSelectorNode(PlayableGraph graph) : base(graph)
        {
        }

        public override int Select()
        {
            CurrentIndex = Random.Range(0, ClipCount);
            return CurrentIndex;
        }
    }
}