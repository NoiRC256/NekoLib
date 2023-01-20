using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace NekoLib.Playables
{
    [System.Serializable]
    public class TimelineConfig
    {
        [field: SerializeField] public TimelineAsset TimelineAsset { get; private set; }
        [field: SerializeField] public DirectorWrapMode WrapMode { get; set; }
    }
}