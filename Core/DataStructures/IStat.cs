using System;

namespace Nep.DataStructures
{
    /// <summary>
    /// Interface for property that represents a modifiable stat.
    /// </summary>
    public interface IStat
    {
        public float Value { get; }
        public event Action<float> ValueChanged;
        public event Action<Stat> StatChanged;
        public event Action<StatModifier> ModifierAdded;
        public event Action<StatModifier> ModifierRemoved;
    }
}