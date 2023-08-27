namespace NekoLib.Stats
{
    /// <summary>
    /// Modifies the value of a stat.
    /// </summary>
    [System.Serializable]
    public class StatModifier
    {
        public enum ModifierEffectType
        {
            Add,
            Mult,
        }

        public float Value;
        public ModifierEffectType EffectType;

        public StatModifier(float value, ModifierEffectType effectType)
        {
            Value = value;
            EffectType = effectType;
        }
    }
}