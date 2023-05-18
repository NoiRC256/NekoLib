using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace NekoLib.Playables
{
    /// <summary>
    /// Extension methods for playables.
    /// </summary>
    public static class PlayableHelper
    {
        public static void Enable(this Playable playable)
        {
            var adapter = GetAdapter(playable);
            if(adapter != null)
            {
                adapter.Enable();
            }
        }

        public static void Disable(this Playable playable)
        {
            var adapter = GetAdapter(playable);
            if (adapter != null)
            {
                adapter.Disable();
            }
        }

        public static void Enable(this AnimationMixerPlayable mixer, int index)
        {
            mixer.GetInput(index).Enable();
        }

        public static void Disable(this AnimationMixerPlayable mixer, int index)
        {
            mixer.GetInput(index).Disable();
        }

        public static AnimAdapter GetAdapter(this Playable playable) {
            if (typeof(AnimAdapter).IsAssignableFrom(playable.GetPlayableType()))
            {
                return ((ScriptPlayable<AnimAdapter>)playable).GetBehaviour();
            }
            return null;
        }

        public static AnimBehaviour GetAnimBehaviour(this Playable playable)
        {
            AnimAdapter adapter = playable.GetAdapter();
            if (adapter == null) return null;
            return adapter.AnimBehaviour;
        }

        public static void SetOutput(this PlayableGraph graph, string name, Animator animator, AnimBehaviour behaviour)
        {
            var root = new RootNode(graph);
            root.AddInput(behaviour);
            var output = AnimationPlayableOutput.Create(graph, name, animator);
            output.SetSourcePlayable(root.GetAdapterPlayable());
        }

        public static void Start(this PlayableGraph graph, AnimBehaviour behaviour)
        {
            graph.Play();
            behaviour.Enable();
        }

        public static void Start(this PlayableGraph graph)
        {
            graph.Play();
            Playable src = graph.GetOutputByType<AnimationPlayableOutput>(0).GetSourcePlayable();
            AnimAdapter adapter = src.GetAdapter();
            adapter?.Enable();
        }
    }
}