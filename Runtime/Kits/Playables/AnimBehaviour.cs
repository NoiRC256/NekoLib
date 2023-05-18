using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace NekoLib.Playables
{
    /// <summary>
    /// Custom behaviour that can be used in playable graphs.
    /// </summary>
    public abstract class AnimBehaviour
    {
        public bool IsEnabled { get; protected set; }
        public float RemainingTime { get; protected set; }

        public virtual float EnterTime { get; protected set; }

        protected Playable _adapterPlayable;
        protected float _enterTime;
        protected float _animLength;

        public AnimBehaviour(float enterTime = 0f)
        {
            _enterTime = enterTime;
        }

        public AnimBehaviour(PlayableGraph graph, float enterTime = 0f)
        {
            _adapterPlayable = ScriptPlayable<AnimAdapter>.Create(graph);
            ((ScriptPlayable<AnimAdapter>)_adapterPlayable).GetBehaviour().Init(this);
            _enterTime = enterTime;
        }

        public virtual void Enable()
        {
            IsEnabled = true;
            RemainingTime = GetAnimLength();
        }

        public virtual void Disable()
        {
            IsEnabled = false;
        }

        public virtual void Execute(Playable playable, FrameData info)
        {
            if (!IsEnabled) return;
            RemainingTime = RemainingTime > 0 ? RemainingTime - info.deltaTime : 0f;
        }

        public virtual void AddInput(Playable playable) { }

        public void AddInput(AnimBehaviour behaviour)
        {
            AddInput(behaviour.GetAdapterPlayable());
        }

        public virtual Playable GetAdapterPlayable()
        {
            return _adapterPlayable;
        }

        public virtual float GetEnterTime()
        {
            return _enterTime;
        }

        public virtual float GetAnimLength()
        {
            return _animLength;
        }
    }
}