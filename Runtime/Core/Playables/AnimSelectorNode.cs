using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace NekoLib.Playables
{
    /// <summary>
    /// Custom playable behaviour that can select a child.
    /// </summary>
    public class AnimSelectorNode : AnimBehaviour
    {
        public int CurrentIndex { get; protected set; }
        public int ClipCount { get; protected set; }

        private AnimationMixerPlayable _mixer;
        private List<float> _enterTimes;
        private List<float> _animLengths;

        public AnimSelectorNode(PlayableGraph graph) : base(graph)
        {
            _mixer = AnimationMixerPlayable.Create(graph);
            _adapterPlayable.AddInput(_mixer, 0, 1f);

            CurrentIndex = -1;
            _enterTimes = new List<float>();
            _animLengths = new List<float>();
        }

        public override void AddInput(Playable playable)
        {
            _mixer.AddInput(playable, 0);
            ClipCount++;
        }

        public void AddInput(AnimationClip animClip, float enterTime = 0)
        {
            AddInput(new AnimNode(_adapterPlayable.GetGraph(), animClip, enterTime));
            _animLengths.Add(animClip.length);
            _enterTimes.Add(enterTime);
        }

        public override void Enable()
        {
            base.Enable();
            if (CurrentIndex < 0 || CurrentIndex >= ClipCount) return;
            _mixer.SetInputWeight(CurrentIndex, 1f);
            _mixer.Enable(CurrentIndex);
            _adapterPlayable.SetTime(0f);
            _adapterPlayable.Play();
            _mixer.SetTime(0f);
            _mixer.Play();
        }

        public override void Disable()
        {
            base.Disable();
            if (CurrentIndex < 0 || CurrentIndex >= ClipCount) return;
            _mixer.GetInput(CurrentIndex).Disable();
            _adapterPlayable.Pause();
            _mixer.Pause();
            CurrentIndex = -1;
        }

        public virtual int Select()
        {
            return CurrentIndex;
        }

        public void Select(int index)
        {
            CurrentIndex = index;
        }

        public override float GetEnterTime()
        {
            return _enterTimes[CurrentIndex];
        }

        public override float GetAnimLength()
        {
            return _animLengths[CurrentIndex];
        }
    }
}
