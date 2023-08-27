using System;
using NekoLib.ReactiveProps;

namespace NekoLib.Stats
{
    /// <summary>
    /// Interface for property that represents a modifiable stat.
    /// </summary>
    public interface IStat : NekoLib.ReactiveProps.IReadOnlyProp<float>
    {
        public event Action<float> ValueChanged;
        public event Action<IStatModifier> ModifierAdded;
        public event Action<IStatModifier> ModifierRemoved;

        public void AddModifier(IStatModifier modifier);
        public bool RemoveModifier(IStatModifier modifier);
        public bool RemoveModifiersBySource(object source);
    }
}