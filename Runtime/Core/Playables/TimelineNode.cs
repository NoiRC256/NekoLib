using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace NekoLib.Playables
{
    /// <summary>
    /// Timeline <see cref="AnimBehaviour"/>.
    /// </summary>
    public class TimelineNode : AnimBehaviour
    {
        Playable _timeline;

        public TimelineNode(PlayableGraph graph, GameObject source,
            TimelineConfig timelineConfig) : base(graph)
        {
            _timeline = timelineConfig.TimelineAsset.CreatePlayable(graph, source);
            _timeline.SetDuration(timelineConfig.TimelineAsset.duration);

            _adapterPlayable.AddInput(_timeline, 0, 1f);
            Disable();
        }

        public override void Execute(Playable playable, FrameData info)
        {
            base.Execute(playable, info);
        }

        public override void Enable()
        {
            base.Enable();
            _timeline.SetTime(0f);
            _adapterPlayable.SetTime(0f);
            _timeline.Play();
            _adapterPlayable.Play();
        }

        public override void Disable()
        {
            base.Disable();
            _timeline.Pause();
            _adapterPlayable.Pause();
        }
    }
}
