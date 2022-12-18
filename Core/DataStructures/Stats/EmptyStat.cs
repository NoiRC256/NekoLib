using System;

namespace Nep.Stats
{
    /// <summary>
    /// Dummy stat class.
    /// </summary>
    public class EmptyStat : IStat
    {
        public float Value { get; set; }

        public event Action<float> ValueChanged;
        public event Action<IStatModifier> ModifierAdded;
        public event Action<IStatModifier> ModifierRemoved;

        public EmptyStat(float value)
        {
            Value = value;
        }

        public void AddModifier(IStatModifier modifier)
        {
            return;
        }

        public bool RemoveModifier(IStatModifier modifier)
        {
            return false;
        }

        public bool RemoveModifiersBySource(object source)
        {
            return false;
        }
    }
}