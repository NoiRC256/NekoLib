using UnityEngine;

namespace Nep.DataStructures.Test
{
    public class MockPlayer : MonoBehaviour
    {
        [field: SerializeField] public BindableProperty<float> Health { get; private set; }
    }
}