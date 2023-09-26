namespace NekoLib.FSM
{
    public class OrCondition<T> : FSMCondition<T>
    {
        private FSMCondition<T> _condition1;
        private FSMCondition<T> _condition2;

        public OrCondition(FSMCondition<T> condition1, FSMCondition<T> condition2)
        {
            _condition1 = condition1;
            _condition2 = condition2;
        }

        public override bool Condition(T owner)
        {
            return _condition1.Condition(owner) || _condition2.Condition(owner);
        }
    }
}