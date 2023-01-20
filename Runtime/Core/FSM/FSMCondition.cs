using System;

namespace NekoLib.FSM
{
    // Adapted from Iris Fenrir
    /// <summary>
    /// Base class for FSM conditions.
    /// </summary>
    public class FSMCondition<T>
    {
        private Func<T, bool> _conditionHandle;

        public FSMCondition() { }

        public FSMCondition(Func<T, bool> handle)
        {
            this._conditionHandle = handle;
        }

        public void BindCondition(Func<T, bool> handle)
        {
            this._conditionHandle = handle;
        }

        /// <summary>
        /// Returns true if condition handle exists and executing it on the specified owner returns true.
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public virtual bool Condition(T owner)
        {
            return _conditionHandle != null && _conditionHandle.Invoke(owner);
        }

        public static FSMCondition<T> operator&(FSMCondition<T> condition1, FSMCondition<T> condition2)
        {
            return new AndCondition<T>(condition1, condition2);
        }

        public static FSMCondition<T> operator|(FSMCondition<T> condition1, FSMCondition<T> condition2)
        {
            return new OrCondition<T>(condition1, condition2);
        }

        public static FSMCondition<T> operator!(FSMCondition<T> condition)
        {
            return new NotCondition<T>(condition);
        }
    }

    public class FSMCondition<T1, T2> : FSMCondition<T1>
    {
        private Func<T1, T2, bool> _conditionHandle;
        private T2 _value;

        public FSMCondition() { }

        public FSMCondition(Func<T1, T2, bool> condition, T2 value)
        {
            BindCondition(condition, value);
        }

        public void BindCondition(Func<T1, T2, bool> condition, T2 value)
        {
            _conditionHandle = condition;
            _value = value;
        }

        public override bool Condition(T1 owner)
        {
            return _conditionHandle != null && _conditionHandle(owner, _value);
        }
    }
}
