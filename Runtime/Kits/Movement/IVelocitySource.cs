using UnityEngine;

namespace NekoLib.Movement
{
    public interface IVelocitySource
    {
        Vector3 Evaluate(float deltaTime);
    }
}