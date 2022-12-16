using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nep.DataStructures
{
    /// <summary>
    /// Data class that represents a modifiable stat.
    /// Manages a collection of value modifiers, and provides relevant events.
    /// </summary>
    [System.Serializable]
    public class Stat : BindableProperty<float>, IStat
    {
        /// <summary>
        /// Final value of the stat.
        /// </summary>
        public override float Value {
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
                if(_baseValue != value)
                {
                    _baseValue = value;
                    _isDirty = true;
                    OnValueChange();
                }
            }
        }

        public event Action<Stat> StatChanged;
        public event Action<StatModifier> ModifierAdded;
        public event Action<StatModifier> ModifierRemoved;

        [SerializeField] protected float _baseValue;
        protected bool _isDirty = true;
        protected readonly List<StatModifier> _modifiers = new List<StatModifier>();

        public Stat(float baseValue = 0f) : base(baseValue)
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
                StatModifier modifier = _modifiers[i];
                switch (modifier.ModType)
                {
                    case StatModifier.StatModifierType.Flat:
                        // Apply flat value modifier.
                        value += modifier.ModStat.Value;
                        break;
                    case StatModifier.StatModifierType.PercentAdd:
                        // Accumulate consequtive additive percentage modifiers to apply together.
                        additivePercent += modifier.ModStat.Value;
                        if (i + 1 >= _modifiers.Count || _modifiers[i + 1].ModType != StatModifier.StatModifierType.PercentAdd)
                        {
                            value *= (1 + additivePercent);
                            additivePercent = 0f;
                        }
                        break;
                    case StatModifier.StatModifierType.PercentMult:
                        // Apply percentage modifier.
                        value *= (1 + modifier.ModStat.Value);
                        break;
                }
                value = (float)Math.Round(value, 4);
            }
            return value;
        }

        public void AddModifier(StatModifier modifier)
        {
            _isDirty = true;
            _modifiers.Add(modifier);
            _modifiers.Sort(CompareModifierOrder);
            modifier.ModStat.ValueChanged += OnModifierValueChanged;
            OnValueChange();
            ModifierAdded?.Invoke(modifier);
        }

        public bool RemoveModifier(StatModifier modifier)
        {
            _isDirty = true;
            bool isRemoved = _modifiers.Remove(modifier);
            modifier.ModStat.ValueChanged -= OnModifierValueChanged;
            OnValueChange();
            if(isRemoved) ModifierRemoved?.Invoke(modifier);
            return isRemoved;
        }

        private int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order) return -1;
            else if (a.Order > b.Order) return 1;
            return 0;
        }

        private void OnModifierValueChanged(float value)
        {
            OnValueChange();
        }

        protected override void OnValueChange()
        {
            base.OnValueChange();
            StatChanged?.Invoke(this);
        }
    }
}