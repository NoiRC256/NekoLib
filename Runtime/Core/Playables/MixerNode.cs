using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace NekoLib.Playables
{
    /// <summary>
    /// Custom <see cref="AnimBehaviour"/> that controls weights and transitions between childs.
    /// </summary>
    public class MixerNode : AnimBehaviour
    {
        public int InputCount { get; private set; }
        public int CurrentIndex => _currentInput;
        public int TargetIndex => _targetInput;

        AnimationMixerPlayable _mixer;

        private int _currentInput;
        private int _targetInput;
        private bool _isTransitioning;
        private float _timeToNext;
        private float _currentSpeed;

        private List<int> _decreasingInputs;
        private float _decreaseSpeed;
        private float _decreaseWeight;

        public MixerNode(PlayableGraph graph) : base(graph)
        {
            _mixer = AnimationMixerPlayable.Create(graph);
            _adapterPlayable.AddInput(_mixer, 0, 1f);
            _decreasingInputs = new List<int>();
            _currentInput = 0;
            _targetInput = -1;
        }

        public override void AddInput(Playable playable)
        {
            _mixer.AddInput(playable, 0, 0f);
            InputCount++;
            if (InputCount == 1)
            {
                _mixer.SetInputWeight(0, 1f);
                _currentInput = 0;
            }
        }

        public override void Enable()
        {
            base.Enable();

            if (_mixer.GetInputCount() > 0) _mixer.Enable(0);

            _mixer.SetTime(0f);
            _mixer.Play();
            _adapterPlayable.SetTime(0f);
            _adapterPlayable.Play();

            _mixer.SetInputWeight(0, 1f);
            _currentInput = 0;
            _targetInput = -1;
        }

        public override void Disable()
        {
            base.Disable();

            for (int i = 0; i < _mixer.GetInputCount(); i++)
            {
                _mixer.SetInputWeight(i, 0f);
                _mixer.Disable(i);
            }
            _mixer.Pause();
            _adapterPlayable.Pause();
        }

        public override void Execute(Playable playable, FrameData info)
        {
            base.Execute(playable, info);

            UpdateTransition(info.deltaTime);
        }

        private void UpdateTransition(float deltaTime)
        {
            if (!_isTransitioning || _targetInput < 0) return;
            if (_timeToNext > 0f)
            {
                _timeToNext -= deltaTime;
                DoTransition(deltaTime);
                return;
            }
            // Reached target.
            _mixer.SetInputWeight(_currentInput, 0f);
            _mixer.Disable(_currentInput);
            _currentInput = _targetInput;
            _mixer.SetInputWeight(_currentInput, 1f);
            _isTransitioning = false;
            _targetInput = -1;
        }

        private void DoTransition(float deltaTime)
        {
            _decreaseWeight = 0f;
            for (int i = 0; i < _decreasingInputs.Count; i++)
            {
                float weight = ModifyWeight(_decreasingInputs[i], -deltaTime * _decreaseSpeed);
                if (weight <= 0f)
                {
                    _mixer.Disable(_decreasingInputs[i]);
                    _decreasingInputs.Remove(_decreasingInputs[i]);
                }
                else
                {
                    _decreaseWeight += weight;
                }
            }
            _decreaseWeight += ModifyWeight(_currentInput, -deltaTime * _currentSpeed);
            SetWeight(_targetInput, 1f - _decreaseWeight);
        }

        /// <summary>
        /// Transition to a target input.
        /// </summary>
        /// <param name="input">The transition target's input index in the mixer.</param>
        /// <param name="matchNormalizedTime">If true, the target animation's normalized time will be
        /// synced with the current playable time.</param>
        /// <param name="offsetTime">Apply a time offset in seconds to the target animation.</param>
        /// <param name="normalizedOffsetTime">Use normalized time as a unit for the time offset.</param>
        public void TransitionTo(int input, bool matchNormalizedTime = false, 
            float offsetTime = 0f, bool normalizedOffsetTime = false)
        {
            if (input < 0 || input >= _mixer.GetInputCount()) return;
            // Attempt transition to current.
            if (!_isTransitioning && input == _currentInput) return;
            // Attempt transition to the same target.
            if (_isTransitioning && input == _targetInput) return;

            if(_isTransitioning)
            {
                // Transitioning back to current.
                if (input == _currentInput)
                {
                    _currentInput = _targetInput;
                }
                // Transitioning first half.
                else if (GetWeight(_currentInput) > GetWeight(_targetInput))
                {
                    _decreasingInputs.Add(_targetInput);
                }
                // Transitioning second half.
                else
                {
                    _decreasingInputs.Add(_currentInput);
                    _currentInput = _targetInput;
                }
            }
            if (GetWeight(_targetInput) >= 1f) return;

            StartTransition(input);

            Playable currentPlayable = _mixer.GetInput(_currentInput);
            Playable targetPlayable = _mixer.GetInput(_targetInput);
            AnimBehaviour targetBehaviour = null;

            // Start transition with target time setting.
            if (matchNormalizedTime)
            {
                Debug.Log("Match normalized time");
                AnimBehaviour currentBehaviour = currentPlayable.GetAnimBehaviour();
                targetBehaviour = targetPlayable.GetAnimBehaviour();
                if (currentBehaviour != null && targetBehaviour != null)
                {
                    float currentTime = (float)currentPlayable.GetTime();
                    float currentAnimLength = currentBehaviour.GetAnimLength();
                    float normalizedTime = (currentTime % currentAnimLength) / currentAnimLength;
                    targetPlayable.SetTime(normalizedTime * targetBehaviour.GetAnimLength());
                }
            }

            if(offsetTime > 0f)
            {
                if (!normalizedOffsetTime)
                {
                    targetPlayable.SetTime(targetPlayable.GetTime() + offsetTime);
                }
                else
                {
                    targetBehaviour = targetBehaviour ?? targetPlayable.GetAnimBehaviour();
                    if (targetBehaviour != null)
                    {
                        targetPlayable.SetTime(targetPlayable.GetTime() + offsetTime * targetBehaviour.GetAnimLength());
                    }
                }
            }
        }

        private void StartTransition(int input)
        {
            Debug.Log("Transition to " + input);
            _targetInput = input;
            _decreasingInputs.Remove(_targetInput);
            _mixer.Enable(_targetInput);

            _timeToNext = GetTargetEnterTime(_targetInput) * (1f - GetWeight(_targetInput));

            _currentSpeed = GetWeight(_currentInput) / _timeToNext;
            _decreaseSpeed = 2f / _timeToNext;
            _isTransitioning = true;
        }

        public float GetWeight(int index)
        {
            if (index < 0 || index >= _mixer.GetInputCount()) return 0f;
            return _mixer.GetInputWeight(index);
        }

        public void SetWeight(int index, float weight)
        {
            if (index < 0 || index >= _mixer.GetInputCount()) return;
            _mixer.SetInputWeight(index, weight);
        }

        private float GetTargetEnterTime(int index)
        {
            return ((ScriptPlayable<AnimAdapter>)_mixer.GetInput(index)).GetBehaviour().GetEnterTime();
        }

        private float ModifyWeight(int index, float delta)
        {
            if (index < 0 || index >= InputCount) return 0;
            float weight = Mathf.Clamp01(GetWeight(index) + delta);
            _mixer.SetInputWeight(index, weight);
            return weight;
        }
    }
}