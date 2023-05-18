using UnityEngine;

namespace NekoLib.Movement
{
    public class VelocitySource : IVelocitySource
    {
        public Vector3 Velocity { get; set; }

        public VelocitySource()
        {
        }

        public virtual void SetVelocity(Vector3 velocity)
        {
            Velocity = velocity;
        }

        public virtual Vector3 Evaluate(float deltaTime)
        {
            Vector3 currentVelocity = Velocity;
            Velocity = Vector3.zero;
            return currentVelocity;
        }
    }

    public class SmoothedVelocitySource : VelocitySource
    {
        public float _velocityChangeSpeed = 30f;

        private Vector3 _targetVelocity = Vector3.zero;

        public override void SetVelocity(Vector3 velocity)
        {
            _targetVelocity = velocity;
        }

        public override Vector3 Evaluate(float deltaTime)
        {
            Vector3 currentVelocity = Velocity;
            Velocity = Vector3.MoveTowards(Velocity, _targetVelocity, _velocityChangeSpeed * deltaTime);
            return currentVelocity;
        }
    }
}