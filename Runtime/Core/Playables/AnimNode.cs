using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace NekoLib.Playables
{
    /// <summary>
    /// Custom animation <see cref="AnimBehaviour"/>.
    /// </summary>
    public class AnimNode : AnimBehaviour
    {
        private AnimationClipPlayable _clip;
        public AnimNode(PlayableGraph graph, AnimationClip animClip, float enterTime = 0f) : base(graph, enterTime)
        {
            _clip = AnimationClipPlayable.Create(graph, animClip);
            _animLength = animClip.length;
            _adapterPlayable.AddInput(_clip, 0, 1f);

            Disable();
        }

        public AnimNode(PlayableGraph graph, AnimParam param) : this(graph, param.animClip, param.enterTime) { }

        public override void Execute(Playable playable, FrameData info)
        {
            base.Execute(playable, info);
            _clip.SetTime(_adapterPlayable.GetTime());
        }

        public override void Enable()
        {
            base.Enable();
            _clip.SetTime(0f);
            _adapterPlayable.SetTime(0f);
            _clip.Play();
            _adapterPlayable.Play();
        }

        public override void Disable()
        {
            base.Disable();
            _clip.Pause();
            _adapterPlayable.Pause();
        }
    }
}