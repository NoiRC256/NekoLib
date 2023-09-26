namespace NekoLib.FSM
{
    public class NotCondition<T> : FSMCondition<T>
    {
        private FSMCondition<T> _condition;

        public NotCondition(FSMCondition<T> condition)
        {
            _condition = condition;
        }

        public override bool Condition(T owner)
        {
            return !_condition.Condition(owner);
        }
    }
}