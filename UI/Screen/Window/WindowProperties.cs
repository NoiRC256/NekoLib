using UnityEngine;

namespace NekoSystems.UI
{
    [System.Serializable]
    public class WindowProperties : IWindowProperties
    {
        [SerializeField] protected WindowPriority _windowPriority = WindowPriority.ForceForeground;
        [SerializeField] protected bool _hideOnForegroundLost = true;
        [SerializeField] protected bool _isPopup = false;

        public WindowPriority WindowPriority {
            get => _windowPriority;
            set => _windowPriority = value;
        }
        public bool HideOnForegroundLost {
            get => _hideOnForegroundLost;
            set => _hideOnForegroundLost = value;
        }
        public bool IsPopup {
            get => _isPopup;
            set => _isPopup = value;
        }
        public bool SuppressPrefabProperties { get; set; }

        public WindowProperties()
        {
            _windowPriority = WindowPriority.ForceForeground;
            _hideOnForegroundLost = true;
            _isPopup = false;
        }

        public WindowProperties(bool suppressPrefabProperties = false)
        {
            _windowPriority = WindowPriority.ForceForeground;
            _hideOnForegroundLost = false;
            SuppressPrefabProperties = suppressPrefabProperties;
        }

        public WindowProperties(WindowPriority windowPriority, bool hideOnForegroundLost = false, bool suppressPrefabProperties = false)
        {
            _windowPriority = windowPriority;
            _hideOnForegroundLost = hideOnForegroundLost;
            SuppressPrefabProperties = suppressPrefabProperties;
        }
    }
}