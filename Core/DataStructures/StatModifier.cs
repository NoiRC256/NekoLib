namespace Nep.DataStructures
{
    /// <summary>
    /// Data class that represents a value modifier for stats.
    /// </summary>
    public class StatModifier
    {
        public enum StatModifierType
        {
            Flat,
            PercentAdd,
            PercentMult,
        }

        public Stat ModStat { get; private set; }
        public StatModifierType ModType { get; set; }
        public int Order { get; }

        public StatModifier(float value, StatModifierType modType = StatModifierType.Flat, int order = 0)
        {
            ModStat = new Stat(value);
            ModType = modType;
            Order = order;
        }
    }
}