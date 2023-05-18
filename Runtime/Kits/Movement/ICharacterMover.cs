using System;
using UnityEngine;

namespace NekoLib.Movement
{
    public interface ICharacterMover
    {
        bool IsOnGround { get; }
        bool IsOnFlatGround { get; }
        Vector3 GroundNormal { get; }
        Vector3 GroundPoint { get; }
        float Speed { get; }
        Vector3 Direction { get; }

        #region Events
        event Action GainedGroundContact;
        event Action LostGroundContact;
        #endregion

        void Move(float inputSpeed, Vector3 inputDirection);
        void SetActiveVelocity(Vector3 velocity);
        void ClearActiveVelocity();
        void SetPassiveVelocity(Vector3 velocity);
        void ClearPassiveVelocity();
        void AddExtraVelocity(Vector3 vel);
        void ClearExtraVelocity();
        public void SetOverrideVelocity(Vector3 velocity,
            bool clearActiveVelocity = false, bool ignoreConnectedGround = false);
        void MoveDeltaPosition(Vector3 deltaPosition,
            bool alignToGround = true, bool restrictToGround = false);
        void AddImpulse(Impulse impulse);
        void RemoveImpulse(Impulse impulse);
    }
}
