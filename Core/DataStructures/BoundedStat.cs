using UnityEngine;

namespace Nep.DataStructures
{
    /// <summary>
    /// Data class that represents a modifiable stat.
    /// The value is bounded inclusively by lower and higher bounds.
    /// </summary>
    public class BoundedStat : Stat
    {
        public override float BaseValue { set => base.BaseValue = Mathf.Clamp(value, MinValue, MaxValue); }
        public BindableProperty<float> MinProperty { get; }
        public BindableProperty<float> MaxProperty { get; }
        public float MinValue => MinProperty.Value;
        public float MaxValue => MaxProperty.Value;

        public BoundedStat(float baseValue, float minValue, float maxValue) : base(baseValue)
        {
            MinProperty = new BindableProperty<float>(minValue);
            MaxProperty = new BindableProperty<float>(maxValue);
        }

        public BoundedStat(float baseValue, BindableProperty<float> minProperty, BindableProperty<float> maxProperty) : base(baseValue)
        {
            MinProperty = minProperty;
            MaxProperty = maxProperty;
        }

        public BoundedStat(float baseValue, float minValue, BindableProperty<float> maxProperty) : base(baseValue)
        {
            MinProperty = new BindableProperty<float>(minValue);
            MaxProperty = maxProperty;
        }

        public BoundedStat(float baseValue, BindableProperty<float> minProperty, float maxValue) : base(baseValue)
        {
            MinProperty = MinProperty;
            MaxProperty = new BindableProperty<float>(maxValue);
        }

        protected override float CalculateValue()
        {
            return Mathf.Clamp(base.CalculateValue(), MinValue, MaxValue);
        }
    }
}