using UnityEngine.Playables;

namespace NekoLib.Playables
{
    /// <summary>
    /// A dummy root custom <see cref="AnimBehaviour"/> used to connect to the output of a playable graph.
    /// </summary>
    public class RootNode : AnimBehaviour
    {
        public RootNode (PlayableGraph graph) : base(graph)
        {

        }

        public override void AddInput(Playable playable)
        {
            _adapterPlayable.AddInput(playable, 0, 1f);
        }

        public override void Enable()
        {
            base.Enable();
            for(int i = 0; i < _adapterPlayable.GetInputCount(); i++)
            {
                _adapterPlayable.GetInput(i).Enable();
            }
            _adapterPlayable.SetTime(0f);
            _adapterPlayable.Play();
        }

        public override void Disable()
        {
            base.Disable();
            for (int i = 0; i < _adapterPlayable.GetInputCount(); i++)
            {
                _adapterPlayable.GetInput(i).Disable();
            }
            _adapterPlayable.Pause();
        }
    }
}