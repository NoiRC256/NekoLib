using System;
using System.Collections.Generic;
using UnityEngine;

namespace NekoLib.Stats
{
    /// <summary>
    /// Data class that represents a modifiable stat.
    /// Manages a collection of value modifiers, and provides relevant events.
    /// </summary>
    [System.Serializable]
    public class Stat : IStat, NekoLib.DataStructures.IBindableProperty<float>
    {
        /// <summary>
        /// Final value of the stat.
        /// </summary>
        public virtual float Value {
            get {
                if (_isDirty)
                {
                    _value = CalculateValue();
                    _isDirty = false;
                }
                return _value;
            }
            set { }
        }
        /// <summary>
        /// Base value of the stat.
        /// </summary>
        public virtual float BaseValue {
            get => _baseValue;
            set {
                if (_baseValue != value)
                {
                    _baseValue = value;
                    _isDirty = true;
                    OnValueChange();
                }
            }
        }

        public event Action<float> ValueChanged;
        public event Action<IStatModifier> ModifierAdded;
        public event Action<IStatModifier> ModifierRemoved;

        protected float _value;
        [SerializeField] protected float _baseValue;
        protected bool _isDirty = true;
        protected readonly List<IStatModifier> _modifiers = new List<IStatModifier>();

        public Stat(float baseValue = 0f)
        {
            BaseValue = baseValue;
        }

        /// <summary>
        /// Calculate the final value of the stat by applying the existing modifiers.
        /// </summary>
        /// <returns></returns>
        protected virtual float CalculateValue()
        {
            float value = BaseValue;
            float additivePercent = 0f;
            // Apply modifiers.
            for (int i = 0; i < _modifiers.Count; i++)
            {
                var modifier = _modifiers[i];
                switch (modifier.Type)
                {
                    case StatModifierType.Flat:
                        // Apply flat value modifier.
                        value += modifier.Stat.Value;
                        break;
                    case StatModifierType.PercentAdd:
                        // Accumulate consequtive additive percentage modifiers to apply together.
                        additivePercent += modifier.Stat.Value;
                        if (i + 1 >= _modifiers.Count || _modifiers[i + 1].Type != StatModifierType.PercentAdd)
                        {
                            value *= (1 + additivePercent);
                            additivePercent = 0f;
                        }
                        break;
                    case StatModifierType.PercentMult:
                        // Apply percentage modifier.
                        value *= (1 + modifier.Stat.Value);
                        break;
                }
                value = (float)Math.Round(value, 4);
            }
            return value;
        }

        /// <summary>
        /// Add a modifier.
        /// </summary>
        /// <param name="modifier"></param>
        public void AddModifier(IStatModifier modifier)
        {
            _modifiers.Add(modifier);
            _modifiers.Sort(CompareModifierOrder);
            modifier.Stat.ValueChanged += OnModifierValueChanged;
            _isDirty = true;
            OnValueChange();
            ModifierAdded?.Invoke(modifier);
        }

        /// <summary>
        /// Remove a modifier.
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public bool RemoveModifier(IStatModifier modifier)
        {
            bool isRemoved = _modifiers.Remove(modifier);
            if (isRemoved)
            {
                modifier.Stat.ValueChanged -= OnModifierValueChanged;
                _isDirty = true;
                OnValueChange();
                ModifierRemoved?.Invoke(modifier);
            }
            return isRemoved;
        }

        /// <summary>
        /// Remove all modifiers from the specified source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool RemoveModifiersBySource(object source)
        {
            bool isRemoved = false;
            for (int i = _modifiers.Count - 1; i >= 0; i--)
            {
                var modifier = _modifiers[i];
                if (modifier.Source == source)
                {
                    _modifiers.RemoveAt(i);
                    isRemoved = true;
                    modifier.Stat.ValueChanged -= OnModifierValueChanged;
                    _isDirty = true;
                    OnValueChange();
                    ModifierRemoved?.Invoke(modifier);
                }
            }
            return isRemoved;
        }

        private int CompareModifierOrder(IStatModifier a, IStatModifier b)
        {
            if (a.Order < b.Order) return -1;
            else if (a.Order > b.Order) return 1;
            return 0;
        }

        private void OnModifierValueChanged(float value)
        {
            OnValueChange();
        }

        protected virtual void OnValueChange()
        {
            ValueChanged?.Invoke(Value);
        }
    }
}