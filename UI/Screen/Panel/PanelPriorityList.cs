using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NekoSystems.UI
{
    [System.Serializable]
    public class PanelPriorityList
    {
        [SerializeField] private List<PanelPriorityListEntry> _paraLayers;

        public Dictionary<PanelPriority, Transform> ParaLayerLookup {
            get {
                if (_paraLayerLookup == null || _paraLayerLookup.Count == 0)
                {
                    CacheLookup();
                }

                return _paraLayerLookup;
            }
        }

        private Dictionary<PanelPriority, Transform> _paraLayerLookup;

        public PanelPriorityList(List<PanelPriorityListEntry> paraLayers)
        {
            _paraLayers = paraLayers;
        }

        private void CacheLookup()
        {
            _paraLayerLookup = new Dictionary<PanelPriority, Transform>();
            for(int i = 0; i < _paraLayers.Count; i++)
            {
                _paraLayerLookup.Add(_paraLayers[i].Priority, _paraLayers[i].TargetParent);
            }
        }
    }

    [System.Serializable]
    public class PanelPriorityListEntry
    {
        [SerializeField]
        [Tooltip("The panel priority type for a given target para-layer")]
        private PanelPriority priority;
        [SerializeField]
        [Tooltip("The GameObject that should house all Panels tagged with this priority")]
        private Transform targetParent;

        public Transform TargetParent {
            get { return targetParent; }
            set { targetParent = value; }
        }

        public PanelPriority Priority {
            get { return priority; }
            set { priority = value; }
        }

        public PanelPriorityListEntry(PanelPriority priority, Transform parent)
        {
            this.priority = priority;
            targetParent = parent;
        }
    }
}