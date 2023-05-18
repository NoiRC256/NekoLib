using System;
using UnityEngine.Playables;

namespace NekoLib.Playables
{
    /// <summary>
    /// A dummy <see cref="PlayableBehaviour"/> used to integrate custom behaviours into playable graphs.
    /// </summary>
    public class AnimAdapter : PlayableBehaviour
    {
        private AnimBehaviour _behaviour;
        public AnimBehaviour AnimBehaviour => _behaviour;

        public void Init(AnimBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enable()
        {
            _behaviour?.Enable();
        }

        public void Disable()
        {
            _behaviour?.Disable();
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            base.PrepareFrame(playable, info);
            _behaviour?.Execute(playable, info);
        }

        public float GetEnterTime()
        {
            return _behaviour.GetEnterTime();
        }
    }
}
