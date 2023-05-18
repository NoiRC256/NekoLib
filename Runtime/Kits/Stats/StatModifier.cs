namespace NekoLib.Stats
{
    /// <summary>
    /// Data class that represents a value modifier for stats.
    /// </summary>
    public class StatModifier : IStatModifier
    {

        public Stat Stat { get; }
        public object Source { get; }
        public StatModifierType Type { get; }
        public int Order { get; }

        public StatModifier(float value, object source, StatModifierType type = StatModifierType.Flat, int order = 0)
        {
            Stat = new Stat(value);
            Source = source;
            Type = type;
            Order = order;
        }
    }
}