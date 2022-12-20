using UnityEngine;

namespace Nep.UI
{
    [System.Serializable]
    public class WindowContextBase : IWindowContext
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
        public bool SuppressPrefabContext { get; set; }

        public WindowContextBase()
        {
            _windowPriority = WindowPriority.ForceForeground;
            _hideOnForegroundLost = true;
            _isPopup = false;
        }

        public WindowContextBase(bool suppressPrefabContext = false)
        {
            _windowPriority = WindowPriority.ForceForeground;
            _hideOnForegroundLost = false;
            SuppressPrefabContext = suppressPrefabContext;
        }

        public WindowContextBase(WindowPriority windowPriority, bool hideOnForegroundLost = false, bool suppressPrefabContext = false)
        {
            _windowPriority = windowPriority;
            _hideOnForegroundLost = hideOnForegroundLost;
            SuppressPrefabContext = suppressPrefabContext;
        }
    }
}