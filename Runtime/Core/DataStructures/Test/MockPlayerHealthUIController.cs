using UnityEngine;

namespace NekoLib.DataStructures.Test
{
    public class MockPlayerHealthUIController : MonoBehaviour
    {
        [SerializeField] MockPlayer _player;

        private void OnEnable()
        {
            _player.Health.ValueChanged += OnPlayerHealthChanged;
        }

        private void OnDisable()
        {
            _player.Health.ValueChanged -= OnPlayerHealthChanged;
        }

        private void OnPlayerHealthChanged(float value)
        {
            // ...
        }
    }
}