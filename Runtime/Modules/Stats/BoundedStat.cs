using System;
using NekoLib.ReactiveProps;

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
        public IReadOnlyProp<float> MinProperty { get; }
        public IReadOnlyProp<float> MaxProperty { get; }
        public float MinValue => MinProperty.Value;
        public float MaxValue => MaxProperty.Value;

        public event Action<float> ReachedMax;
        public event Action<float> ReachedMin;

        public BoundedStat(float baseValue, float minValue, float maxValue) : base(baseValue)
        {
            MinProperty = new ReadonlyProp<float>(minValue);
            MaxProperty = new ReadonlyProp<float>(maxValue);
        }

        public BoundedStat(float baseValue, IReadOnlyProp<float> minProperty, IReadOnlyProp<float> maxProperty) : base(baseValue)
        {
            MinProperty = minProperty;
            MaxProperty = maxProperty;
        }

        public BoundedStat(float baseValue, float minValue, IReadOnlyProp<float> maxProperty) : base(baseValue)
        {
            MinProperty = new ReadonlyProp<float>(minValue);
            MaxProperty = maxProperty;
        }

        public BoundedStat(float baseValue, IReadOnlyProp<float> minProperty, float maxValue) : base(baseValue)
        {
            MinProperty = MinProperty;
            MaxProperty = new ReadonlyProp<float>(maxValue);
        }
    }
}