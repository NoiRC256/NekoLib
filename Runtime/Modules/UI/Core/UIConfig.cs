using System.Collections.Generic;
using UnityEngine;

namespace NekoLib.UI
{
    [CreateAssetMenu(menuName = "Nap UI/UI Config", fileName = "UIConfig")]
    public class UIConfig : ScriptableObject
    {
        [SerializeField] private UIFrame _uiPrefab;
        [SerializeField] private List<UIControllerBase> _controllerEntries;

        public UIFrame CreateUIFrame()
        {
            UIFrame uiFrame = GameObject.Instantiate(_uiPrefab);
            uiFrame.Init();
            foreach(var controllerEntry in _controllerEntries)
            {
                var controller = GameObject.Instantiate(controllerEntry);
                if (controller == null) break;
                uiFrame.RegisterScreen(controller.name, controller, uiFrame.GetUILayer(controller.LayerId));
                controller.gameObject.SetActive(false);
            }
            return uiFrame;
        }
    }
}