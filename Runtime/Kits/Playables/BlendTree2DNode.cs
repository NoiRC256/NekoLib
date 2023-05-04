using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace NekoLib.Playables
{
    /// <summary>
    /// Custom 2D blendtree <see cref="AnimBehaviour"/>.
    /// </summary>
    public class BlendTree2DNode : AnimBehaviour
    {
        private struct DataPair
        {
            public float x;
            public float y;
            public float output;
        }

        private AnimationMixerPlayable _mixer;
        private DataPair[] _datas;

        public BlendTree2DNode(PlayableGraph graph, BlendClip2D[] clips) : base(graph)
        {
            _datas = new DataPair[clips.Length];

            _mixer = AnimationMixerPlayable.Create(graph);
            _adapterPlayable.AddInput(_mixer, 0, 1f);
            for (int i = 0; i < clips.Length; i++)
            {
                _mixer.AddInput(AnimationClipPlayable.Create(graph, clips[i].animClip), 0);
                _datas[i].x = clips[i].pos.x;
                _datas[i].y = clips[i].pos.y;
            }
        }
    }

    [System.Serializable]
    public struct BlendClip2D
    {
        public AnimationClip animClip;
        public Vector2 pos;
    }
}