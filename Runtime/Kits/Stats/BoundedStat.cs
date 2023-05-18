using System;
using NekoLib.DataStructures;

namespace NekoLib.Stats
{
    /// <summary>
    /// Data class that represents a modifiable stat.
    /// The value is bounded inclusively by lower and higher bounds.
    /// </summary>
    public class BoundedStat : Stat
    {
        public override float BaseValue {
            set {
                if (_baseValue != value)
                {
                    _baseValue = value;

                    if (_baseValue <= MinValue)
                    {
                        _baseValue = MinValue;
                        ReachedMin?.Invoke(_baseValue);
                    } else if (_baseValue >= MaxValue)
                    {
                        _baseValue = MaxValue;
                        ReachedMax?.Invoke(_baseValue);
                    }
                    _isDirty = true;
                    OnValueChange();
                }
            }
        }
        public IReadonlyProperty<float> MinProperty { get; }
        public IReadonlyProperty<float> MaxProperty { get; }
        public float MinValue => MinProperty.Value;
        public float MaxValue => MaxProperty.Value;

        public event Action<float> ReachedMax;
        public event Action<float> ReachedMin;

        public BoundedStat(float baseValue, float minValue, float maxValue) : base(baseValue)
        {
            MinProperty = new ReadonlyProperty<float>(minValue);
            MaxProperty = new ReadonlyProperty<float>(maxValue);
        }

        public BoundedStat(float baseValue, IReadonlyProperty<float> minProperty, IReadonlyProperty<float> maxProperty) : base(baseValue)
        {
            MinProperty = minProperty;
            MaxProperty = maxProperty;
        }

        public BoundedStat(float baseValue, float minValue, IReadonlyProperty<float> maxProperty) : base(baseValue)
        {
            MinProperty = new ReadonlyProperty<float>(minValue);
            MaxProperty = maxProperty;
        }

        public BoundedStat(float baseValue, IReadonlyProperty<float> minProperty, float maxValue) : base(baseValue)
        {
            MinProperty = MinProperty;
            MaxProperty = new ReadonlyProperty<float>(maxValue);
        }
    }
}