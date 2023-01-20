using System;
using NekoLib.DataStructures;

namespace NekoLib.Stats
{
    /// <summary>
    /// Interface for property that represents a modifiable stat.
    /// </summary>
    public interface IStat : NekoLib.DataStructures.IReadonlyProperty<float>
    {
        public event Action<float> ValueChanged;
        public event Action<IStatModifier> ModifierAdded;
        public event Action<IStatModifier> ModifierRemoved;

        public void AddModifier(IStatModifier modifier);
        public bool RemoveModifier(IStatModifier modifier);
        public bool RemoveModifiersBySource(object source);
    }
}