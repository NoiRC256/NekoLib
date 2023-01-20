using System.Collections.Generic;
using UnityEngine;

namespace NekoLib.Playables
{
    [CreateAssetMenu(menuName = "NekoLib/Playable Animator Config")]
    public class PlayableAnimatorConfig : ScriptableObject
    {
        [field: SerializeField] public List<AnimParam> AnimParams { get; private set; }

        public AnimParam GetParam(string name)
        {
            return AnimParams.Find(p => p.name == name);
        }
    }

    [System.Serializable]
    public class AnimParam
    {
        public enum Type
        {
            Single,
            Group,
            InfoGroup,
            BlendClip,
        }

        public string name = "Anim";
        public float enterTime = 0f;
        public Type type = Type.Single;
        [Header("Single Type")]
        public AnimationClip animClip;
        [Header("Group Type")]
        public AnimationClip[] animClipGroup;
        [Header("Info Group Type")]
        public AnimInfo[] infoGroup;
        [Header("Blend Clip Type")]
        public BlendClip2D blendClip;
    }

    [System.Serializable]
    public class AnimInfo
    {
        public AnimationClip animClip;
        public float enterTime;
    }
}
