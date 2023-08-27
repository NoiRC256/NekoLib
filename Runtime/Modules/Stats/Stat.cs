using System;
using System.Collections.Generic;
using NekoLib.ReactiveProps;
using static NekoLib.Stats.StatModifier;
using UnityEngine;

namespace NekoLib.Stats
{
    /// <summary>
    /// A class representing a stat value. Maintains a collection of modifiers
    /// which contribute to the final value.
    /// <para>The final value is lazily re-calculated on access.</para>
    /// <para>Can be ticked by <see cref="Tick"/> to enable invoking events
    /// whenever marked for re-calculation.</para>
    /// </summary>
    [System.Serializable]
    public class Stat : BindableFloat
    {
        #region Exposed Variables

        [Tooltip("Base value of the stat.")]
        [SerializeField] private float _baseValue = 0f;
        [Tooltip("If true, will limit the final value by an upper bound if present.")]
        [field: SerializeField] public bool UseUpperBound { get; set; } = false;
        [Tooltip("If true, will limit the final value by a lower bound if present.")]
        [field: SerializeField] public bool UseLowerBound { get; set; } = false;
        [field: SerializeField] public BindableFloat UpperBound { get; private set; }
        [field: SerializeField] public BindableFloat LowerBound { get; private set; }

        #endregion

        #region Fields

        private bool _isDirty = true;
        private bool _notifyChangeThisTick = false;
        private bool _hasUpperBound = false;
        private bool _hasLowerBound = false;
        private List<StatModifier> _modifiers = new List<StatModifier>();

        #endregion

        #region Properties

        /// <summary>
        /// The base value of the stat.
        /// <para>Modifying this will mark the stat dirty.</para>
        /// </summary>
        public float BaseValue {
            get => _baseValue;
            set {
                if (value != _baseValue) SetDirty();
                _baseValue = value;
            }
        }

        /// <summary>
        /// The final value of the stat.
        /// Lazily calculated on access if dirty.
        /// <para>Cannot be directly set.</para>
        /// </summary>
        public override float Value {
            get {
                if (_isDirty) RefreshValue();
                return _value;
            }
            set => throw new NotImplementedException();
        }

        public bool UseBounds {
            get => UseUpperBound && UseLowerBound;
            set {
                UseUpperBound = value; UseLowerBound = value;
            }
        }
        public bool HasUpperBound => _hasUpperBound;
        public bool HasLowerBound => _hasLowerBound;

        #endregion

        #region Events

        /// <summary>
        /// When the stat has been marked dirty for value re-calculation.
        /// <para>The base value may have changed, or stat modifiers may have been added / removed.</para>
        /// </summary>
        public event Action<Stat> StatChanged;

        #endregion

        public Stat() : this(0f) { }

        public Stat(float baseValue) : base(baseValue)
        {
            _baseValue = baseValue;
            _modifiers = new List<StatModifier>();
            _isDirty = true;
            _notifyChangeThisTick = false;
            UseUpperBound = false;
        }

        #region Value Change Monitoring

        /// <summary>
        /// Monitor value change.
        /// <para>Call this at most once per frame for optimal performance.</para>
        /// </summary>
        public void Tick()
        {
            // If marked dirty for re-calculation, invoke value change events.
            if (_notifyChangeThisTick)
            {
                OnValueChange();
                StatChanged?.Invoke(this);
            }
            _notifyChangeThisTick = false;
        }

        /// <summary>
        /// Set the stat as dirty.
        /// <para>Will broadcast the changes next tick.</para>
        /// </summary>
        private void SetDirty()
        {
            _isDirty = true;
            _notifyChangeThisTick = true;
        }

        /// <summary>
        /// Re-calculate the final value.
        /// </summary>
        private void RefreshValue()
        {
            _value = CalculateValue();
            _isDirty = false;
        }

        #endregion

        #region Value Calculation

        private float CalculateValue()
        {
            // Aggregate modifier values.
            float addModifiersValue = 0f;
            float multModifiersValue = 0f;
            for (int i = 0; i < _modifiers.Count; i++)
            {
                StatModifier modifier = _modifiers[i];
                if (modifier.EffectType == ModifierEffectType.Add)
                {
                    addModifiersValue += modifier.Value;
                }
                else if (modifier.EffectType == ModifierEffectType.Mult)
                {
                    multModifiersValue += modifier.Value;
                }
            }

            // Calculate final value.
            float finalValue = _baseValue + (_baseValue * multModifiersValue) + addModifiersValue;
            if (UseUpperBound && _hasUpperBound && finalValue > UpperBound.Value) finalValue = UpperBound.Value;
            if (UseLowerBound && _hasLowerBound && finalValue < LowerBound.Value) finalValue = LowerBound.Value;
            return finalValue;
        }

        #endregion

        #region Bounds

        /// <summary>
        /// Assigns an upper bound.
        /// <para>The final value of the stat will be limited by this upper bound 
        /// if <see cref="UseUpperBound"/> is true.</para>
        /// <para>This will take effect next time final value re-calculation occurs.</para>
        /// </summary>
        /// <param name="bindableFloat"></param>
        public void SetUpperBound(BindableFloat bindableFloat)
        {
            if (UpperBound != null) UpperBound.ValueChanged -= OnUpperBoundChanged;
            UpperBound = bindableFloat;
            UpperBound.ValueChanged += OnUpperBoundChanged;
            _hasUpperBound = true;
        }

        public void RemoveUpperBound()
        {
            if (UpperBound != null) UpperBound.ValueChanged -= OnUpperBoundChanged;
            UpperBound = null;
            _hasUpperBound = false;
        }

        /// <summary>
        /// Assigns a lower bound.
        /// <para>The final value of the stat will be limited by this lower bound 
        /// if <see cref="UseLowerBound"/> is true.</para>
        /// <para>This will take effect next time final value re-calculation occurs.</para>
        /// </summary>
        /// <param name="bindableFloat"></param>
        public void SetLowerBound(BindableFloat bindableFloat)
        {
            if (LowerBound != null) LowerBound.ValueChanged -= OnLowerBoundChanged;
            LowerBound = bindableFloat;
            LowerBound.ValueChanged += OnLowerBoundChanged;
            _hasLowerBound = true;
        }

        public void RemoveLowerBound()
        {
            if (LowerBound != null) LowerBound.ValueChanged -= OnLowerBoundChanged;
            LowerBound = null;
            _hasLowerBound = false;
        }

        private void OnUpperBoundChanged(float value) { if (UseUpperBound) SetDirty(); }
        private void OnLowerBoundChanged(float value) { if (UseLowerBound) SetDirty(); }

        #endregion

        #region Stat Modifiers

        /// <summary>
        /// Add a modifier to the stat.
        /// <para>Marks the stat dirty.</para>
        /// </summary>
        /// <param name="modifier"></param>
        public void AddModifier(StatModifier modifier)
        {
            _modifiers.Add(modifier);
            SetDirty();
        }

        /// <summary>
        /// Remove a modifier from the stat.
        /// <para>Marks the stat dirty.</para>
        /// </summary>
        /// <param name="modifier"></param>
        public void RemoveModifier(StatModifier modifier)
        {
            _modifiers.Remove(modifier);
            SetDirty();
        }

        #endregion
    }
}