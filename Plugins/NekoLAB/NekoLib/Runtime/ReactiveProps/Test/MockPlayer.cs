using UnityEngine;

namespace NekoLib.ReactiveProps.Test
{
    public class MockPlayer : MonoBehaviour
    {
        [field: SerializeField] public BindableProp<float> Health { get; private set; }
    }
}