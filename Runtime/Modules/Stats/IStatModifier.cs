namespace NekoLib.Stats
{
    /// <summary>
    /// Interface for value modifier for stats.
    /// </summary>
    public interface IStatModifier
    {
        public Stat Stat { get; }
        public object Source { get; }
        public StatModifierType Type { get; }
        public int Order { get; }
    }
}