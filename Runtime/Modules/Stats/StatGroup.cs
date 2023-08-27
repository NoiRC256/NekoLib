using System.Collections.Generic;
using NekoLib.ReactiveProps;

namespace NekoLib.Stats
{
    /// <summary>
    /// A container for stats.
    /// Each stat is mapped to an enum value.
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class StatGroup<E> where E : System.Enum
    {
        private Dictionary<E, Stat> _stats = new Dictionary<E, Stat>();
        private List<Stat> _statList = new List<Stat>();

        /// <summary>
        /// Initialize stat collections.
        /// </summary>
        public void Init()
        {
            _stats = new Dictionary<E, Stat>();
            _statList = new List<Stat>();
        }

        /// <summary>
        /// Tick stats to enable change monitoring.
        /// </summary>
        public void Tick()
        {
            for (int i = 0; i < _statList.Count; i++)
            {
                _statList[i].Tick();
            }
        }

        /// <summary>
        /// Get stat by enum.
        /// </summary>
        /// <param name="statType"></param>
        /// <param name="stat"></param>
        /// <returns></returns>
        public virtual bool TryGetStat(E statType, out Stat stat)
        {
            return _stats.TryGetValue(statType, out stat);
        }

        /// <summary>
        /// Initialize a stat and map it to an enum value.
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="statType"></param>
        /// <param name="value"></param>
        /// <param name="useBounds"></param>
        /// <param name="upperBound"></param>
        /// <param name="lowerBound"></param>
        protected virtual void RegisterStat(Stat stat, E statType, float value,
            bool useBounds = false, BindableFloat upperBound = null, BindableFloat lowerBound = null)
        {
            stat.BaseValue = value;
            AddStat(statType, stat);
            stat.UseBounds = useBounds;
            if (upperBound != null) stat.SetUpperBound(upperBound);
            if (lowerBound != null) stat.SetLowerBound(lowerBound);
        }

        /// <summary>
        /// Shortcut for initializing a resource stat
        /// with a lower bound of 0 and an upper bound.
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="statType"></param>
        /// <param name="value"></param>
        /// <param name="upperBound"></param>
        protected virtual void RegisterResourceStat(Stat stat, E statType, float value, BindableFloat upperBound)
        {
            RegisterStat(stat, statType, value, useBounds: true, upperBound: upperBound);
        }

        /// <summary>
        /// Shorcut for initializing a stat with a lower bound.
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="statType"></param>
        /// <param name="value"></param>
        /// <param name="lowerBound"></param>
        protected virtual void RegisterLowerBoundedStat(Stat stat, E statType, float value, float lowerBound = 0f)
        {
            RegisterStat(stat, statType, value,
                useBounds: true,
                lowerBound: new BindableFloat(lowerBound));
        }

        protected virtual void AddStat(E statType, Stat stat)
        {
            _stats.Add(statType, stat);
            _statList.Add(stat);
        }
    }
}